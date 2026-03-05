/**
 * Cliente HTTP para API MobiCortex Master
 * 
 * Esta classe encapsula todas as operações REST da controladora.
 * Endpoints base: /mbcortex/master/api/v1/
 * Autenticação: Bearer token (session_key, 15min TTL)
 */

const { MbcortexError, AuthError, ValidationError, NotFoundError, ConflictError, ServerError } = require('./exceptions');

class MbcortexClient {
  constructor(baseUrl, password = "admin") {
    this.baseUrl = baseUrl.replace(/\/$/, ''); // Remove trailing slash
    this.password = password;
    this.sessionKey = null; // Armazena token de autenticação
  }

  /**
   * Autentica na controladora e obtém session_key
   * @returns {string} Session key para usar em Authorization header
   */
  async login() {
    // POST /login com {"pass":"admin"}
    // Retorna {"ret":0,"session_key":"<64hex>","expires_in":900}
    try {
      const response = await fetch(`${this.baseUrl}/mbcortex/master/api/v1/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ pass: this.password })
      });
      
      if (!response.ok) {
        throw new AuthError(`Login falhou: ${response.status} - ${await response.text()}`, response.status);
      }
      
      const data = await response.json();
      this.sessionKey = data.session_key; // Salva para próximas chamadas
      return this.sessionKey;
    } catch (error) {
      // Melhora mensagens de erro de conectividade
      if (error.code === 'ECONNREFUSED') {
        throw new MbcortexError('Conexão recusada. Verifique se o IP/porta estão corretos e se a controladora está ligada.');
      } else if (error.code === 'ENOTFOUND') {
        throw new MbcortexError('Host não encontrado. Verifique o IP/hostname da controladora.');
      } else if (error.code === 'ECONNRESET' || error.cause?.code === 'ECONNRESET') {
        throw new MbcortexError('Conexão resetada. Verifique se a porta HTTPS está correta (normalmente 443 ou 4449).');
      } else if (error.message?.includes('certificate') || error.message?.includes('SSL') || error.message?.includes('TLS')) {
        throw new MbcortexError('Erro de certificado SSL. A controladora usa certificado self-signed que pode ser rejeitado pelo Node.js.');
      } else if (error.name === 'TypeError' && error.message?.includes('fetch')) {
        throw new MbcortexError('Erro de conectividade: ' + error.message);
      } else if (error instanceof AuthError) {
        throw error; // Re-throw authentication errors
      } else {
        throw new MbcortexError(`Erro na autenticação: ${error.message}`);
      }
    }
  }

  /**
   * Executa requisição HTTP com tratamento de erros padronizado
   * @param {string} method - GET, POST, DELETE, etc
   * @param {string} endpoint - Caminho após /mbcortex/master/api/v1/
   * @param {object} options - {body, useAuth}
   * @returns {object} Response JSON ou lança erro
   */
  async request(method, endpoint, options = {}) {
    const { body, useAuth = true } = options;
    
    const headers = {
      'Content-Type': 'application/json',
    };
    
    // Adiciona Bearer token se autenticação requerida
    if (useAuth && this.sessionKey) {
      headers.Authorization = `Bearer ${this.sessionKey}`;
    }
    
    const url = `${this.baseUrl}/mbcortex/master/api/v1/${endpoint}`;
    
    console.log(`→ ${method} ${url}`);
    if (body) {
      console.log(`  Body: ${JSON.stringify(body)}`);
    }
    
    try {
      const response = await fetch(url, {
        method,
        headers,
        body: body ? JSON.stringify(body) : undefined,
      });
      
      const responseText = await response.text();
      let responseData = null;
      
      // Tenta parsear JSON se possível
      try {
        responseData = JSON.parse(responseText);
      } catch (e) {
        responseData = responseText;
      }
      
      console.log(`← ${response.status} ${response.statusText}`);
      console.log(`  Response: ${JSON.stringify(responseData, null, 2)}`);
      
      if (!response.ok) {
        // Determina tipo de exceção baseado no status code
        const ErrorClass = (() => {
          switch (response.status) {
            case 400: return ValidationError;
            case 401: return AuthError;
            case 404: return NotFoundError;
            case 409: return ConflictError;
            default: return response.status >= 500 ? ServerError : MbcortexError;
          }
        })();
        
        const errorMessage = responseData?.error || responseText || `HTTP ${response.status}`;
        throw new ErrorClass(
          errorMessage,
          response.status,
          responseData
        );
      }
      
      return responseData;
    } catch (error) {
      // Trata erros de conectividade
      if (error.code === 'ECONNREFUSED') {
        throw new MbcortexError('Conexão recusada. Verifique se o IP/porta estão corretos e se a controladora está ligada.');
      } else if (error.code === 'ENOTFOUND') {
        throw new MbcortexError('Host não encontrado. Verifique o IP/hostname da controladora.');
      } else if (error.code === 'ECONNRESET' || error.cause?.code === 'ECONNRESET') {
        throw new MbcortexError('Conexão resetada. Verifique se a porta HTTPS está correta.');
      } else if (error.message?.includes('certificate') || error.message?.includes('SSL') || error.message?.includes('TLS')) {
        throw new MbcortexError('Erro de certificado SSL. A controladora usa certificado self-signed.');
      } else if (error.name === 'TypeError' && error.message?.includes('fetch')) {
        throw new MbcortexError('Erro de conectividade: ' + error.message);
      } else if (error instanceof MbcortexError) {
        // Re-throw MbcortexError e subclasses
        throw error;
      } else {
        throw new MbcortexError(`Erro na requisição: ${error.message}`);
      }
    }
  }

  /**
   * Cria unidade central com configurações básicas
   * @param {number} id - 0 para ID automático, >0 para ID específico
   * @param {string} name - Nome da unidade
   * @returns {number} ID da unidade criada
   */
  async createUnit(id, name) {
    // Payload obrigatório para central-registry
    // slots1/slots2: define quantos slots de comunicação (0-15)
    // type: tipo da central (geralmente 0 para unidade padrão)
    // enabled: 1 = ativa, 0 = inativa
    const payload = {
      id: id,           // 0 = auto-id (faixa 4294000000+), >0 = ID específico
      name: name,
      enabled: 1,       // Ativa por padrão
      slots1: 8,        // Slots de comunicação série 1 (padrão 8)
      slots2: 0,        // Slots de comunicação série 2 (padrão 0)
      type: 0           // Tipo padrão da unidade
    };
    
    const result = await this.request('POST', 'central-registry', { body: payload });
    
    // A controladora retorna o ID definitivo da unidade
    // Para auto-id (id=0), será um número na faixa 4294000000+
    // Para ID fixo, será o mesmo número enviado
    return result.id || id;
  }

  /**
   * Cria unidade com ID automático (controladora gera)
   * @param {string} name Nome da unidade
   * @returns {number} ID gerado pela controladora (faixa 4294000000+)
   */
  async createUnitAuto(name) {
    // id=0 significa "gere automaticamente"
    // Controladora usa faixa WEB_PRINCIPAL_ID_BASE = 4294000000+
    return this.createUnit(0, name);
  }

  /**
   * Cria unidade com ID específico (para testes)
   * @param {number} unitId ID desejado (use faixa 2000000+ para teste)
   * @param {string} name Nome da unidade  
   * @returns {number} ID confirmado pela controladora
   */
  async createUnitFixed(unitId, name) {
    // IDs fixos: usar faixa 2E6+ (2000000+) para não conflitar
    if (unitId < 2000000) {
      console.warn('AVISO: ID < 2000000 pode conflitar com IDs existentes');
    }
    return this.createUnit(unitId, name);
  }

  /**
   * Remove unidade da controladora
   * @param {number} unitId ID da unidade a ser removida
   */
  async deleteUnit(unitId) {
    // DELETE via query parameter
    await this.request('DELETE', `central-registry?id=${unitId}`);
    console.log(`✓ Unidade ${unitId} removida`);
  }

  /**
   * Lista cadastros centrais com paginação
   * @param {number} offset - Posição inicial (padrão: 0)
   * @param {number} count - Quantidade por página (padrão: 10)
   * @returns {Object} {items: Array, total: number, offset: number, count: number}
   */
  async listCentralRegistries(offset = 0, count = 10) {
    const response = await this.request('GET', `central-registry?offset=${offset}&count=${count}`);
    
    // Normaliza resposta para formato consistente
    const items = response.items || response.data || [];
    const total = response.total || items.length;
    
    return {
      items,
      total,
      offset,
      count,
      hasNext: (offset + count) < total,
      hasPrevious: offset > 0
    };
  }

  /**
   * Busca cadastro central por ID
   * @param {number} unitId - ID do cadastro
   * @returns {Object} Dados do cadastro ou null se não encontrado
   */
  async getCentralRegistry(unitId) {
    try {
      const response = await this.request('GET', `central-registry/${unitId}`);
      return response;
    } catch (error) {
      if (error instanceof NotFoundError) {
        return null;
      }
      throw error;
    }
  }

  /**
   * Lista entidades por cadastro com paginação
   * @param {number} unitId - ID do cadastro
   * @param {number} offset - Posição inicial
   * @param {number} count - Quantidade por página
   * @returns {Object} Lista paginada de entidades
   */
  async listEntitiesByUnit(unitId, offset = 0, count = 10) {
    const response = await this.request('GET', `entities?cadastro_id=${unitId}&offset=${offset}&count=${count}`);
    
    const items = response.items || response.data || [];
    const total = response.total || items.length;
    
    return {
      items,
      total,
      offset,
      count,
      hasNext: (offset + count) < total,
      hasPrevious: offset > 0
    };
  }

  /**
   * Busca avançada de entidades com filtros
   * @param {Object} filters - {type, name, doc, cadastro_id}
   * @param {number} offset - Posição inicial
   * @param {number} count - Quantidade por página
   * @returns {Object} Resultados da busca
   */
  async searchEntities(filters = {}, offset = 0, count = 10) {
    const params = new URLSearchParams();
    
    // Adiciona filtros
    if (filters.type) params.append('type', filters.type);
    if (filters.name) params.append('name', filters.name);
    if (filters.doc) params.append('doc', filters.doc);
    if (filters.cadastro_id) params.append('cadastro_id', filters.cadastro_id);
    
    // Adiciona paginação
    params.append('offset', offset);
    params.append('count', count);
    
    const response = await this.request('GET', `entities?${params.toString()}`);
    
    const items = response.items || response.data || [];
    const total = response.total || items.length;
    
    return {
      items,
      total,
      offset,
      count,
      hasNext: (offset + count) < total,
      hasPrevious: offset > 0,
      filters
    };
  }

  /**
   * Cria veículo (entidade tipo 2) com configurações de LPR
   * @param {number} id - 0 para createid=true, >0 para ID específico
   * @param {number} unitId - ID da unidade à qual o veículo pertence
   * @param {string} name - Nome/descrição do veículo
   * @param {string} plate - Placa do veículo (será normalizada: maiúscula, sem espaços)
   * @param {number} lprAtivo - 1 = LPR ativo, 0 = inativo (padrão: 1)
   * @param {Object} options - Campos opcionais (brand, model, color, obs)
   * @returns {number} ID do veículo criado
   */
  async createVehicle(id, unitId, name, plate, lprAtivo = 1, options = {}) {
    // Normaliza placa: maiúscula, sem espaços nem hífens
    const normalizedPlate = plate.toUpperCase().replace(/[\s-]/g, '');
    
    const payload = {
      tipo: 2,              // Tipo 2 = veículo
      name: name,
      doc: normalizedPlate, // Campo "doc" armazena a placa
      cadastro_id: unitId,  // Vincula à unidade criada
      lpr_ativo: lprAtivo   // Habilita/desabilita LPR para este veículo
    };
    
    // Adiciona campos opcionais se fornecidos
    const optionalFields = ['brand', 'model', 'color', 'obs'];
    optionalFields.forEach(field => {
      if (options[field]) {
        payload[field] = options[field];
      }
    });
    
    if (id === 0) {
      // Usa createid=true para ID automático
      payload.createid = true;
      // Não envia o campo "id" quando createid=true
    } else {
      // ID específico fornecido pelo usuário
      payload.id = id;
      // Não usa createid quando ID é especificado
    }
    
    const result = await this.request('POST', 'entities', { body: payload });
    
    // Retorna o ID do veículo criado
    return result.id || id;
  }

  /**
   * Cria veículo com ID automático (controladora gera)
   * @param {number} unitId ID da unidade
   * @param {string} name Nome do veículo
   * @param {string} plate Placa do veículo
   * @param {number} lprAtivo 1 = LPR ativo, 0 = inativo (padrão: 1)
   * @param {Object} options Campos opcionais: {brand, model, color, obs}
   * @returns {number} ID gerado pela controladora
   */
  async createVehicleAuto(unitId, name, plate, lprAtivo = 1, options = {}) {
    // createid=true: controladora gera ID automaticamente
    return this.createVehicle(0, unitId, name, plate, lprAtivo, options);
  }

  /**
   * Cria veículo com ID específico (para testes)
   * @param {number} vehicleId ID desejado (use faixa 2000000+ para teste)
   * @param {number} unitId ID da unidade
   * @param {string} name Nome do veículo  
   * @param {string} plate Placa do veículo
   * @param {number} lprAtivo 1 = LPR ativo, 0 = inativo (padrão: 1)
   * @param {Object} options Campos opcionais: {brand, model, color, obs}
   * @returns {number} ID confirmado pela controladora
   */
  async createVehicleFixed(vehicleId, unitId, name, plate, lprAtivo = 1, options = {}) {
    // IDs fixos: usar faixa 2E6+ (2000000+) para não conflitar
    if (vehicleId < 2000000) {
      console.warn('AVISO: ID < 2000000 pode conflitar com IDs existentes');
    }
    return this.createVehicle(vehicleId, unitId, name, plate, lprAtivo, options);
  }

  /**
   * Remove veículo da controladora
   * @param {number} vehicleId ID do veículo a ser removido
   */
  async deleteVehicle(vehicleId) {
    // DELETE via query parameter
    await this.request('DELETE', `entities?id=${vehicleId}`);
    console.log(`✓ Veículo ${vehicleId} removido`);
  }

  /**
   * Cria pessoa (entidade tipo 1) com configurações básicas
   * @param {number} id - 0 para createid=true, >0 para ID específico
   * @param {number} unitId - ID da unidade à qual a pessoa pertence
   * @param {string} name - Nome da pessoa
   * @param {string} doc - CPF/Documento da pessoa (será normalizado: apenas números)
   * @param {Object} options - Campos opcionais (obs, email, telefone)
   * @returns {number} ID da pessoa criada
   */
  async createPerson(id, unitId, name, doc = "", options = {}) {
    // Normaliza documento: apenas números
    const normalizedDoc = doc.replace(/[^\d]/g, '');
    
    const payload = {
      tipo: 1,              // Tipo 1 = pessoa
      name: name,
      doc: normalizedDoc,   // Campo "doc" armazena CPF/documento
      cadastro_id: unitId,  // Vincula à unidade
      lpr_ativo: 0          // Pessoas não usam LPR
    };
    
    // Adiciona campos opcionais se fornecidos
    const optionalFields = ['obs', 'email', 'telefone'];
    optionalFields.forEach(field => {
      if (options[field]) {
        payload[field] = options[field];
      }
    });
    
    if (id === 0) {
      // Usa createid=true para ID automático
      payload.createid = true;
    } else {
      // ID específico fornecido pelo usuário
      payload.id = id;
    }
    
    const result = await this.request('POST', 'entities', { body: payload });
    
    // Retorna o ID da pessoa criada
    return result.id || id;
  }

  /**
   * Cria pessoa com ID automático (controladora gera)
   * @param {number} unitId ID da unidade
   * @param {string} name Nome da pessoa
   * @param {string} doc CPF/Documento da pessoa
   * @param {Object} options Campos opcionais: {obs, email, telefone}
   * @returns {number} ID gerado pela controladora
   */
  async createPersonAuto(unitId, name, doc = "", options = {}) {
    // createid=true: controladora gera ID automaticamente
    return this.createPerson(0, unitId, name, doc, options);
  }

  /**
   * Cria pessoa com ID específico (para testes)
   * @param {number} personId ID desejado (use faixa 2000000+ para teste)
   * @param {number} unitId ID da unidade
   * @param {string} name Nome da pessoa
   * @param {string} doc CPF/Documento da pessoa
   * @param {Object} options Campos opcionais: {obs, email, telefone}
   * @returns {number} ID confirmado pela controladora
   */
  async createPersonFixed(personId, unitId, name, doc = "", options = {}) {
    // IDs fixos: usar faixa 2E6+ (2000000+) para não conflitar
    if (personId < 2000000) {
      console.warn('AVISO: ID < 2000000 pode conflitar com IDs existentes');
    }
    return this.createPerson(personId, unitId, name, doc, options);
  }

  /**
   * Remove pessoa da controladora
   * @param {number} personId ID da pessoa a ser removida
   */
  async deletePerson(personId) {
    // DELETE via query parameter
    await this.request('DELETE', `entities?id=${personId}`);
    console.log(`✓ Pessoa ${personId} removida`);
  }
}

module.exports = MbcortexClient;