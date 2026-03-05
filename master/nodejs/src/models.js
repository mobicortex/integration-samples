/**
 * Modelos de dados para API MobiCortex Master
 * ============================================
 * 
 * Define estruturas de dados e classes auxiliares para facilitar
 * o trabalho com a API MobiCortex Master.
 * 
 * AUTOR: MobiCortex
 */

/**
 * Representa um cadastro central (unidade/empresa).
 * 
 * Um cadastro central é uma entidade organizacional que pode
 * conter múltiplas entidades (pessoas e veículos).
 */
class CentralRegistry {
  /**
   * @param {number} id - ID da unidade (0 = auto-gerado)
   * @param {string} name - Nome da unidade
   * @param {number} enabled - 1 = ativo, 0 = inativo
   * @param {number} slots1 - Slots de parqueamento tipo 1
   * @param {number} slots2 - Slots de parqueamento tipo 2
   * @param {number} type - Tipo de cadastro (0 = padrão)
   */
  constructor(id = 0, name = '', enabled = 1, slots1 = 0, slots2 = 0, type = 0) {
    this.id = id;
    this.name = name;
    this.enabled = enabled;
    this.slots1 = slots1;
    this.slots2 = slots2;
    this.type = type;
  }
  
  /**
   * Converte para formato da API.
   * @returns {Object} Objeto para enviar na API
   */
  toApiObject() {
    return {
      id: this.id,
      descricao: this.name,
      ativo: this.enabled,
      slots_tier_1: this.slots1,
      slots_tier_2: this.slots2,
      tipo: this.type
    };
  }
  
  /**
   * Cria instância a partir de resposta da API.
   * @param {Object} apiData - Dados da API
   * @returns {CentralRegistry} Nova instância
   */
  static fromApiData(apiData) {
    return new CentralRegistry(
      apiData.id || 0,
      apiData.descricao || '',
      apiData.ativo || 1,
      apiData.slots_tier_1 || 0,
      apiData.slots_tier_2 || 0,
      apiData.tipo || 0
    );
  }
  
  /**
   * Valida dados do cadastro.
   * @returns {boolean} True se válido
   */
  isValid() {
    return this.name && this.name.trim().length > 0;
  }
}

/**
 * Classe base para entidades (pessoas e veículos).
 * 
 * Representa uma entidade no sistema, que pode ser uma pessoa
 * ou veículo vinculado a um cadastro central.
 */
class Entity {
  /**
   * @param {number} id - ID da entidade (0 = auto-gerado)
   * @param {string} name - Nome da entidade
   * @param {string} doc - Documento (CPF ou placa)
   * @param {number} cadastroId - ID do cadastro central
   * @param {number} type - Tipo (1 = pessoa, 2 = veículo)
   */
  constructor(id = 0, name = '', doc = '', cadastroId = 0, type = 1) {
    this.id = id;
    this.name = name;
    this.doc = doc;
    this.cadastroId = cadastroId;
    this.type = type;
  }
  
  /**
   * Converte para formato da API.
   * @returns {Object} Objeto para enviar na API
   */
  toApiObject() {
    return {
      id: this.id,
      nome: this.name,
      doc: this.doc,
      cadastro_id: this.cadastroId,
      tipo: this.type
    };
  }
  
  /**
   * Cria instância a partir de resposta da API.
   * @param {Object} apiData - Dados da API
   * @returns {Entity} Nova instância
   */
  static fromApiData(apiData) {
    return new Entity(
      apiData.id || 0,
      apiData.nome || '',
      apiData.doc || '',
      apiData.cadastro_id || 0,
      apiData.tipo || 1
    );
  }
  
  /**
   * Valida dados básicos da entidade.
   * @returns {boolean} True se válido
   */
  isValid() {
    return this.name && this.name.trim().length > 0 && 
           this.doc && this.doc.trim().length > 0 &&
           this.cadastroId > 0;
  }
}

/**
 * Representa uma pessoa no sistema.
 * 
 * Herda de Entity e adiciona funcionalidades específicas para pessoas.
 */
class Person extends Entity {
  /**
   * @param {number} id - ID da pessoa (0 = auto-gerado)
   * @param {string} name - Nome da pessoa
   * @param {string} cpf - CPF da pessoa
   * @param {number} cadastroId - ID do cadastro central
   * @param {Object} options - Campos opcionais (obs, email, telefone)
   */
  constructor(id = 0, name = '', cpf = '', cadastroId = 0, options = {}) {
    // Pessoa sempre tem tipo = 1
    super(id, name, Person.normalizeCpf(cpf), cadastroId, 1);
    
    // Campos opcionais específicos da pessoa
    this.obs = options.obs || '';
    this.email = options.email || '';
    this.telefone = options.telefone || '';
  }
  
  /**
   * Getter para CPF (alias para doc).
   * @returns {string} CPF da pessoa
   */
  get cpf() {
    return this.doc;
  }
  
  /**
   * Setter para CPF (com normalização automática).
   * @param {string} value - CPF
   */
  set cpf(value) {
    this.doc = Person.normalizeCpf(value);
  }
  
  /**
   * Normaliza CPF removendo formatação.
   * @param {string} cpf - CPF para normalizar
   * @returns {string} CPF apenas com números
   */
  static normalizeCpf(cpf) {
    return cpf ? cpf.replace(/[^\d]/g, '') : '';
  }
  
  /**
   * Valida CPF básico (apenas comprimento).
   * @returns {boolean} True se válido
   */
  isValidCpf() {
    const cpf = this.cpf;
    return cpf && cpf.length === 11;
  }
  
  /**
   * Converte para formato da API, incluindo campos opcionais.
   * @returns {Object} Objeto para enviar na API
   */
  toApiObject() {
    const obj = super.toApiObject();
    
    // Adiciona campos opcionais se preenchidos
    if (this.obs) obj.obs = this.obs;
    if (this.email) obj.email = this.email;
    if (this.telefone) obj.telefone = this.telefone;
    
    return obj;
  }
}

/**
 * Representa um veículo no sistema.
 * 
 * Herda de Entity e adiciona funcionalidades específicas para veículos.
 */
class Vehicle extends Entity {
  /**
   * @param {number} id - ID do veículo (0 = auto-gerado)
   * @param {string} name - Nome do proprietário/veículo
   * @param {string} plate - Placa do veículo
   * @param {number} cadastroId - ID do cadastro central
   * @param {number} lprAtivo - 1 = LPR ativo, 0 = inativo
   * @param {Object} options - Campos opcionais (brand, model, color, obs)
   */
  constructor(id = 0, name = '', plate = '', cadastroId = 0, lprAtivo = 1, options = {}) {
    // Veículo sempre tem tipo = 2
    super(id, name, Vehicle.normalizePlate(plate), cadastroId, 2);
    
    // LPR (License Plate Recognition)
    this.lprAtivo = lprAtivo;
    
    // Campos opcionais específicos do veículo
    this.brand = options.brand || '';    // Marca
    this.model = options.model || '';    // Modelo
    this.color = options.color || '';    // Cor
    this.obs = options.obs || '';        // Observações
  }
  
  /**
   * Getter para placa (alias para doc).
   * @returns {string} Placa do veículo
   */
  get plate() {
    return this.doc;
  }
  
  /**
   * Setter para placa (com normalização automática).
   * @param {string} value - Placa
   */
  set plate(value) {
    this.doc = Vehicle.normalizePlate(value);
  }
  
  /**
   * Normaliza placa removendo caracteres especiais.
   * @param {string} plate - Placa para normalizar
   * @returns {string} Placa normalizada
   */
  static normalizePlate(plate) {
    return plate ? plate.replace(/[^A-Za-z0-9]/g, '').toUpperCase() : '';
  }
  
  /**
   * Valida formato básico da placa (padrão brasileiro).
   * @returns {boolean} True se válido
   */
  isValidPlate() {
    // Formato ABC1234 ou ABC1D34 (Mercosul)
    const pattern = /^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$/;
    return pattern.test(this.plate);
  }
  
  /**
   * Converte para formato da API, incluindo LPR e campos opcionais.
   * @returns {Object} Objeto para enviar na API
   */
  toApiObject() {
    const obj = super.toApiObject();
    
    // Adiciona LPR
    obj.lpr_ativo = this.lprAtivo;
    
    // Adiciona campos opcionais se preenchidos
    const optionalFields = ['brand', 'model', 'color', 'obs'];
    optionalFields.forEach(field => {
      if (this[field]) {
        obj[field] = this[field];
      }
    });
    
    return obj;
  }
}

/**
 * Representa uma resposta paginada da API.
 * 
 * Facilita o trabalho com listagens que suportam paginação.
 */
class PaginatedResponse {
  /**
   * @param {Array} data - Lista de itens da página atual
   * @param {number} total - Total de itens disponíveis
   * @param {number} offset - Posição inicial desta página
   * @param {number} count - Quantidade de itens por página
   */
  constructor(data, total, offset, count) {
    this.data = data;
    this.total = total;
    this.offset = offset;
    this.count = count;
  }
  
  /**
   * Verifica se há próxima página.
   * @returns {boolean} True se há próxima página
   */
  get hasNext() {
    return (this.offset + this.count) < this.total;
  }
  
  /**
   * Verifica se há página anterior.
   * @returns {boolean} True se há página anterior
   */
  get hasPrevious() {
    return this.offset > 0;
  }
  
  /**
   * Retorna offset para próxima página.
   * @returns {number|null} Offset ou null se não há próxima página
   */
  get nextOffset() {
    return this.hasNext ? this.offset + this.count : null;
  }
  
  /**
   * Retorna offset para página anterior.
   * @returns {number|null} Offset ou null se não há página anterior
   */
  get previousOffset() {
    return this.hasPrevious ? Math.max(0, this.offset - this.count) : null;
  }
  
  /**
   * Número da página atual (1-based).
   * @returns {number} Página atual
   */
  get currentPage() {
    return Math.floor(this.offset / this.count) + 1;
  }
  
  /**
   * Total de páginas disponíveis.
   * @returns {number} Total de páginas
   */
  get totalPages() {
    return this.total > 0 ? Math.ceil(this.total / this.count) : 0;
  }
}

module.exports = {
  CentralRegistry,
  Entity,
  Person,
  Vehicle,
  PaginatedResponse
};