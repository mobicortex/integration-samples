#!/usr/bin/env node

/**
 * CLI Interativo - MobiCortex Master API
 * ======================================
 * 
 * Interface de linha de comando com menu interativo para demonstrar
 * todas as funcionalidades da API MobiCortex Master.
 * 
 * Funcionalidades:
 * 📋 Configuração de conexão (IP, porta, login, senha)
 * 🧪 Testes rápidos (AUTO e FIXED IDs)  
 * 🏢 Gestão de cadastros centrais (unidades/empresas)
 * 🚗 Gestão de entidades (pessoas e veículos)
 * 🔍 Consultas e listagens com paginação
 * 📊 Estatísticas do sistema
 * 
 * USO:
 *   node examples/cli.js
 *   npm run cli
 * 
 * REQUISITOS:
 *   Node.js 18+ (apenas fetch nativo)
 * 
 * NOTA SSL:
 *   Controladores industriais usam certificados self-signed.
 *   Para testes, desabilite verificação SSL: NODE_TLS_REJECT_UNAUTHORIZED=0
 * 
 * AUTOR:
 *   MobiCortex
 */

// Para controladores com certificados self-signed (comum em equipamentos industriais)
// Esta configuração é necessária apenas para testes em ambiente desenvolvimento
if (process.env.NODE_TLS_REJECT_UNAUTHORIZED === undefined) {
  console.log('⚠️  Aviso: Desabilitando verificação SSL para certificados self-signed');
  console.log('   Controladores industriais frequentemente usam certificados próprios');
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
}

const { MbcortexClient } = require('../src');
const { AuthError, ValidationError, ConflictError, NotFoundError, MbcortexError } = require('../src');
const fs = require('fs');
const path = require('path');
const readline = require('readline');

// Arquivo para configurações
const CONFIG_FILE = path.join(__dirname, 'cli-config.json');

/**
 * Interface de menu interativo
 */
class InteractiveCLI {
  constructor() {
    this.rl = readline.createInterface({
      input: process.stdin,
      output: process.stdout
    });
    this.config = this.loadConfig();
  }
  
  /**
   * Carrega configurações salvas
   */
  loadConfig() {
    try {
      if (fs.existsSync(CONFIG_FILE)) {
        return JSON.parse(fs.readFileSync(CONFIG_FILE, 'utf8'));
      }
    } catch (error) {
      console.log('⚠️  Erro ao carregar config, usando padrões');
    }
    
    // Configuração padrão
    return {
      baseUrl: '',
      username: 'master',
      password: 'admin',
      timeout: 10000
    };
  }
  
  /**
   * Salva configurações
   */
  saveConfig() {
    try {
      fs.writeFileSync(CONFIG_FILE, JSON.stringify(this.config, null, 2));
      console.log('✅ Configuração salva');
    } catch (error) {
      console.log('⚠️  Erro ao salvar configuração:', error.message);
    }
  }
  
  /**
   * Faz pergunta ao usuário
   */
  async question(prompt) {
    return new Promise(resolve => {
      this.rl.question(prompt, resolve);
    });
  }
  
  /**
   * Limpa tela e mostra header
   */
  showHeader(title) {
    console.clear();
    console.log('╔══════════════════════════════════════════════════════════════╗');
    console.log('║                                                              ║'); 
    console.log('║           MOBICORTEX MASTER - CLI INTERATIVO                 ║');
    console.log('║                                                              ║');
    console.log('║   Demonstração completa da API REST da controladora          ║');
    console.log('║                                                              ║');
    console.log('╚══════════════════════════════════════════════════════════════╝');
    console.log('');
    if (title) {
      console.log(`🎯 ${title}`);
      console.log('─'.repeat(60));
      console.log('');
    }
  }

  /**
   * Menu principal
   */
  async showMainMenu() {
    this.showHeader();
    
    // Status da conexão
    const status = this.config.baseUrl ? 
      `🔗 Configurado: ${this.config.baseUrl}` : 
      '❌ Não configurado';
    
    console.log('  🔐 **CONFIGURAÇÃO**');
    console.log('  [C] Configurar Conexão (IP, porta, login, senha)');
    console.log('  [T] Testar Conectividade');
    console.log('');
    
    console.log('  🧪 **TESTES RÁPIDOS**');
    console.log('  [1] Teste Completo - Modo AUTO (IDs automáticos)');
    console.log('  [2] Teste Completo - Modo FIXED (IDs fixos)');
    console.log('');
    
    console.log('  🏢 **CADASTROS CENTRAIS**');
    console.log('  [3] Listar Cadastros com Paginação');
    console.log('  [4] Novo Cadastro Central');
    console.log('  [5] Buscar Cadastro por ID');
    console.log('');
    
    console.log('  👥 **ENTIDADES**');
    console.log('  [6] Nova Pessoa (0 no cadastro = cria automático com nome)');
    console.log('  [7] Novo Veículo (0 no cadastro = cria automático com placa)');
    console.log('  [8] Listar Entidades por Cadastro');
    console.log('  [9] Busca Avançada de Entidades');
    console.log('');
    
    console.log('  ℹ️  **INFORMAÇÕES**');
    console.log('  [I] Sobre o Sistema');
    console.log('  [0] Sair');
    console.log('');
    console.log(`  Status: ${status}`);
    console.log('');
    
    const choice = await this.question('Escolha uma opção: ');
    return choice.toUpperCase();
  }
  
  /**
   * Configura parâmetros de conexão
   */
  async configureConnection() {
    this.showHeader('CONFIGURAÇÃO DA CONEXÃO');
    
    if (this.config.baseUrl) {
      console.log('  [Configurações atuais - pressione ENTER para manter]');
      console.log('');
    }
    
    // IP/Hostname
    const currentHost = this.config.baseUrl ? new URL(this.config.baseUrl).hostname : '';
    const host = (await this.question(`IP/Hostname [${currentHost}]: `)).trim() || currentHost;
    
    // Porta
    const currentPort = this.config.baseUrl ? new URL(this.config.baseUrl).port || '443' : '443';
    const port = (await this.question(`Porta HTTPS [${currentPort}]: `)).trim() || currentPort;
    
    // Monta URL
    this.config.baseUrl = port === '443' ? `https://${host}` : `https://${host}:${port}`;
    
    // Usuário
    const username = (await this.question(`Usuário [${this.config.username}]: `)).trim() || this.config.username;
    this.config.username = username;
    
    // Senha
    const password = (await this.question(`Senha [${this.config.password}]: `)).trim() || this.config.password;
    this.config.password = password;
    
    // Timeout
    const timeout = (await this.question(`Timeout em ms [${this.config.timeout}]: `)).trim() || this.config.timeout;
    this.config.timeout = parseInt(timeout) || 10000;
    
    this.saveConfig();
    
    console.log('');
    console.log('✅ Configuração atualizada!');
    console.log(`   URL: ${this.config.baseUrl}`);
    console.log(`   Usuário: ${this.config.username}`);
    
    await this.question('\nPressione ENTER para continuar...');
  }
  
  /**
   * Testa conectividade
   */
  async testConnection() {
    this.showHeader('TESTE DE CONECTIVIDADE');
    
    if (!this.config.baseUrl) {
      console.log('❌ Configure a conexão primeiro (opção C)');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    console.log(`🔗 Testando: ${this.config.baseUrl}`);
    console.log('');
    
    try {
      const client = new MbcortexClient(this.config.baseUrl, this.config.password);
      
      console.log('📝 Tentando fazer login...');
      const sessionKey = await client.login();
      
      console.log('✅ Login realizado com sucesso!');
      console.log(`   Session Key: ${sessionKey.substring(0, 16)}...`);
      console.log('   Conexão funcionando corretamente.');
      
    } catch (error) {
      console.log('❌ Falha na conectividade:');
      
      if (error instanceof AuthError) {
        console.log('   🔐 Erro de autenticação - verifique a senha');
      } else if (error.code === 'ECONNREFUSED') {
        console.log('   🌐 Conexão recusada - verifique IP e porta');
      } else if (error.code === 'ENOTFOUND') {
        console.log('   🌐 Host não encontrado - verifique o IP/hostname');
      } else {
        console.log(`   ⚠️  ${error.message}`);
      }
    }
    
    await this.question('\nPressione ENTER para continuar...');
  }
  
  /**
   * Executa teste rápido com campos opcionais
   */
  async runQuickTest(mode = 'auto') {
    this.showHeader(`TESTE RÁPIDO - MODO ${mode.toUpperCase()}`);
    
    if (!this.config.baseUrl) {
      console.log('❌ Configure a conexão primeiro (opção C)');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    console.log(`Configuração: ${this.config.baseUrl}`);
    console.log('');
    
    // Coleta dados do teste
    let unitId, vehicleId;
    let unitName = 'Demo Unidade';
    
    if (mode === 'fixed') {
      console.log('\n  (digite 0 para criar cadastro automático com nome da placa)');
      unitId = parseInt((await this.question('ID da unidade [0]: ')).trim()) || 0;
      vehicleId = parseInt((await this.question('ID do veículo [2000002]: ')).trim()) || 2000002;
    }
    
    const vehicleName = (await this.question('Nome do veículo [Demo Veículo]: ')).trim() || 'Demo Veículo';
    const plate = (await this.question('Placa [ABC1234]: ')).trim() || 'ABC1234';
    
    // Se unitId for 0, usar placa como nome da unidade
    if (unitId === 0) {
      unitName = plate;
    }
    
    // Campos opcionais
    console.log('');
    console.log('📋 Campos opcionais do veículo:');
    const brand = (await this.question('Marca []: ')).trim();
    const model = (await this.question('Modelo []: ')).trim();
    const color = (await this.question('Cor []: ')).trim();
    
    const cleanup = (await this.question('\nRemover unidade no final? (s/N): ')).trim().toLowerCase();
    const shouldCleanup = cleanup === 's' || cleanup === 'sim';
    
    console.log('\n🚀 Iniciando teste...');
    console.log('─'.repeat(40));
    
    try {
      const client = new MbcortexClient(this.config.baseUrl, this.config.password);
      
      // Login
      console.log('\n📝 1. Fazendo login...');
      await client.login();
      console.log('   ✅ Login realizado');
      
      let createdUnitId, createdVehicleId;
      
      try {
        // Criar unidade
        console.log('\n🏢 2. Criando unidade...');
        if (mode === 'auto' || unitId === 0) {
          createdUnitId = await client.createUnitAuto(unitName);
        } else {
          createdUnitId = await client.createUnitFixed(unitId, unitName);
        }
        console.log(`   ✅ Unidade criada: ID=${createdUnitId}`);
        
        // Criar veículo com campos opcionais
        console.log('\n🚗 3. Criando veículo...');
        const options = {};
        if (brand) options.brand = brand;
        if (model) options.model = model;
        if (color) options.color = color;
        
        if (mode === 'auto') {
          createdVehicleId = await client.createVehicleAuto(createdUnitId, vehicleName, plate, 1, options);
        } else {
          createdVehicleId = await client.createVehicleFixed(vehicleId, createdUnitId, vehicleName, plate, 1, options);
        }
        
        console.log(`   ✅ Veículo criado: ID=${createdVehicleId}`);
        console.log(`   📋 Placa: ${plate} | LPR: ATIVO`);
        if (brand) console.log(`   📋 Marca: ${brand}`);
        if (model) console.log(`   📋 Modelo: ${model}`);
        if (color) console.log(`   📋 Cor: ${color}`);
        
        // Remover veículo
        console.log('\n🗑️  4. Removendo veículo...');
        await client.deleteVehicle(createdVehicleId);
        console.log('   ✅ Veículo removido');
        
        // Remover unidade (opcional)
        if (shouldCleanup) {
          console.log('\n🗑️  5. Removendo unidade...');
          await client.deleteUnit(createdUnitId);
          console.log('   ✅ Unidade removida');
        } else {
          console.log(`\n📌 Unidade mantida: ID=${createdUnitId}`);
        }
        
        console.log('\n🎉 TESTE CONCLUÍDO COM SUCESSO!');
        
      } catch (error) {
        // Cleanup em caso de erro
        console.log('\n🧹 Fazendo limpeza após erro...');
        
        try {
          if (createdVehicleId) {
            await client.deleteVehicle(createdVehicleId);
            console.log('   ✅ Veículo removido');
          }
          if (shouldCleanup && createdUnitId) {
            await client.deleteUnit(createdUnitId);
            console.log('   ✅ Unidade removida');
          }
        } catch (cleanupError) {
          console.log(`   ⚠️  Erro na limpeza: ${cleanupError.message}`);
        }
        
        throw error;
      }
      
    } catch (error) {
      console.log('\n❌ Erro durante teste:');
      
      if (error instanceof AuthError) {
        console.log('   🔐 Falha de autenticação - verifique senha');
      } else if (error instanceof ValidationError) {
        console.log('   📝 Dados inválidos - verifique os valores informados');
      } else if (error instanceof ConflictError) {
        console.log('   ⚔️  Conflito - ID ou placa já existe');
      } else {
        console.log(`   ⚠️  ${error.message}`);
      }
    }
    
    await this.question('\nPressione ENTER para continuar...');
  }
  
  /**
   * Cria nova pessoa
   */
  async createPerson() {
    this.showHeader('NOVA PESSOA');
    
    if (!this.config.baseUrl) {
      console.log('❌ Configure a conexão primeiro (opção C)');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    console.log('  (digite 0 para gerar ID automaticamente)');
    const personId = parseInt((await this.question('ID da Pessoa [0]: ')).trim()) || 0;
    
    console.log('\n  (digite 0 para criar cadastro automático com nome da pessoa)');
    let unitId = parseInt((await this.question('ID do Cadastro Central [0]: ')).trim()) || 0;
    
    const name = (await this.question('Nome: ')).trim();
    if (!name) {
      console.log('❌ Nome é obrigatório');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    const doc = (await this.question('CPF/Documento (opcional): ')).trim();
    
    try {
      const client = new MbcortexClient(this.config.baseUrl, this.config.password);
      
      // Login
      console.log('\n📝 Fazendo login...');
      await client.login();
      console.log('   ✅ Login realizado');
      
      // Se unitId for 0, cria cadastro automático com nome da pessoa
      if (unitId === 0) {
        console.log(`\n🏢 Criando cadastro central automático com nome '${name}'...`);
        unitId = await client.createUnitAuto(name);
        console.log(`   ✅ Cadastro criado: ID=${unitId}`);
      }
      
      // Criar pessoa (tipo 1)
      console.log('\n👤 Criando pessoa...');
      
      // Campos opcionais
      const options = {};
      
      let createdPersonId;
      if (personId === 0) {
        createdPersonId = await client.createPersonAuto(unitId, name, doc, options);
      } else {
        createdPersonId = await client.createPersonFixed(personId, unitId, name, doc, options);
      }
      
      console.log(`   ✅ Pessoa criada: ID=${createdPersonId}`);
      console.log(`   👤 Nome: ${name}`);
      console.log(`   📄 Documento: ${doc || 'Não informado'}`);
      
    } catch (error) {
      console.log('\n❌ Erro ao criar pessoa:');
      if (error instanceof ConflictError) {
        console.log('   ⚔️  Conflito - ID ou documento já existe');
      } else {
        console.log(`   ⚠️  ${error.message}`);
      }
    }
    
    await this.question('\nPressione ENTER para continuar...');
  }
  
  /**
   * Cria novo veículo
   */
  async createVehicle() {
    this.showHeader('NOVO VEÍCULO');
    
    if (!this.config.baseUrl) {
      console.log('❌ Configure a conexão primeiro (opção C)');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    console.log('  (digite 0 para gerar ID automaticamente)');
    const vehicleId = parseInt((await this.question('ID do Veículo [0]: ')).trim()) || 0;
    
    console.log('\n  (digite 0 para criar cadastro automático com nome da placa)');
    let unitId = parseInt((await this.question('ID do Cadastro Central [0]: ')).trim()) || 0;
    
    const ownerName = (await this.question('Nome do Proprietário: ')).trim();
    if (!ownerName) {
      console.log('❌ Nome do proprietário é obrigatório');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    const plate = (await this.question('Placa: ')).trim().toUpperCase();
    if (!plate) {
      console.log('❌ Placa é obrigatória');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    const lprInput = (await this.question('Ativar LPR? (S/n): ')).trim().toLowerCase();
    const lprAtivo = lprInput !== 'n' && lprInput !== 'nao' ? 1 : 0;
    
    // Campos opcionais
    console.log('\n📋 Campos opcionais:');
    const brand = (await this.question('Marca: ')).trim() || undefined;
    const model = (await this.question('Modelo: ')).trim() || undefined;
    const color = (await this.question('Cor: ')).trim() || undefined;
    
    try {
      const client = new MbcortexClient(this.config.baseUrl, this.config.password);
      
      // Login
      console.log('\n📝 Fazendo login...');
      await client.login();
      console.log('   ✅ Login realizado');
      
      // Se unitId for 0, cria cadastro automático com nome da placa
      if (unitId === 0) {
        console.log(`\n🏢 Criando cadastro central automático com nome '${plate}'...`);
        unitId = await client.createUnitAuto(plate);
        console.log(`   ✅ Cadastro criado: ID=${unitId}`);
      }
      
      // Criar veículo
      console.log('\n🚗 Criando veículo...');
      const options = {};
      if (brand) options.brand = brand;
      if (model) options.model = model;
      if (color) options.color = color;
      
      let createdVehicleId;
      if (vehicleId === 0) {
        createdVehicleId = await client.createVehicleAuto(unitId, ownerName, plate, lprAtivo, options);
      } else {
        createdVehicleId = await client.createVehicleFixed(vehicleId, unitId, ownerName, plate, lprAtivo, options);
      }
      
      console.log(`   ✅ Veículo criado: ID=${createdVehicleId}`);
      console.log(`   📋 Placa: ${plate} | LPR: ${lprAtivo ? 'ATIVO' : 'INATIVO'}`);
      
    } catch (error) {
      console.log('\n❌ Erro ao criar veículo:');
      if (error instanceof ConflictError) {
        console.log('   ⚔️  Conflito - ID ou placa já existe');
      } else {
        console.log(`   ⚠️  ${error.message}`);
      }
    }
    
    await this.question('\nPressione ENTER para continuar...');
  }
  
  /**
   * Lista entidades por cadastro
   */
  async listEntitiesByUnit() {
    this.showHeader('LISTAR ENTIDADES POR CADASTRO');
    
    if (!this.config.baseUrl) {
      console.log('❌ Configure a conexão primeiro (opção C)');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    const unitId = parseInt((await this.question('ID do Cadastro Central: ')).trim());
    if (!unitId) {
      console.log('❌ ID do cadastro é obrigatório');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    try {
      const client = new MbcortexClient(this.config.baseUrl, this.config.password);
      
      console.log('\n📝 Fazendo login...');
      await client.login();
      console.log('   ✅ Login realizado');
      
      console.log(`\n🔍 Buscando entidades do cadastro ${unitId}...`);
      
      // Usa o endpoint de entities com filtro por cadastro_id
      const result = await client.request('GET', `entities?cadastro_id=${unitId}`);
      
      if (result.ret === 0 && result.items) {
        console.log(`\n📋 ${result.items.length} entidade(s) encontrada(s):\n`);
        console.log('ID         | Tipo    | Nome');
        console.log('───────────┼─────────┼──────────────────────────────');
        
        for (const item of result.items) {
          const tipo = item.tipo === 1 ? 'Pessoa' : item.tipo === 2 ? 'Veículo' : '?';
          console.log(`${item.entity_id?.toString().padEnd(10)} | ${tipo.padEnd(7)} | ${item.name}`);
        }
      } else {
        console.log('ℹ️  Nenhuma entidade encontrada');
      }
      
    } catch (error) {
      console.log('\n❌ Erro ao listar:');
      console.log(`   ⚠️  ${error.message}`);
    }
    
    await this.question('\nPressione ENTER para continuar...');
  }
  
  /**
   * Busca avançada de entidades
   */
  async searchEntities() {
    this.showHeader('BUSCA AVANÇADA DE ENTIDADES');
    
    if (!this.config.baseUrl) {
      console.log('❌ Configure a conexão primeiro (opção C)');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    const nameFilter = (await this.question('Filtrar por nome (vazio=todos): ')).trim();
    const docFilter = (await this.question('Filtrar por documento/placa (vazio=todos): ')).trim();
    
    try {
      const client = new MbcortexClient(this.config.baseUrl, this.config.password);
      
      console.log('\n📝 Fazendo login...');
      await client.login();
      console.log('   ✅ Login realizado');
      
      console.log('\n🔍 Buscando...');
      
      // Monta query params
      let url = 'entities?offset=0&count=20';
      if (nameFilter) url += `&name=${encodeURIComponent(nameFilter)}`;
      if (docFilter) url += `&doc=${encodeURIComponent(docFilter)}`;
      
      const result = await client.request('GET', url);
      
      if (result.ret === 0 && result.items) {
        console.log(`\n📋 ${result.total} resultado(s) total:\n`);
        console.log('ID         | Tipo    | Nome                         | Documento/Placa');
        console.log('───────────┼─────────┼──────────────────────────────┼────────────────');
        
        for (const item of result.items) {
          const tipo = item.tipo === 1 ? 'Pessoa' : item.tipo === 2 ? 'Veículo' : '?';
          const nome = item.name?.substring(0, 28).padEnd(28) || '';
          const doc = item.doc || '';
          console.log(`${item.entity_id?.toString().padEnd(10)} | ${tipo.padEnd(7)} | ${nome} | ${doc}`);
        }
        
        if (result.total > result.items.length) {
          console.log(`\n... e mais ${result.total - result.items.length} resultado(s)`);
        }
      } else {
        console.log('ℹ️  Nenhuma entidade encontrada');
      }
      
    } catch (error) {
      console.log('\n❌ Erro na busca:');
      console.log(`   ⚠️  ${error.message}`);
    }
    
    await this.question('\nPressione ENTER para continuar...');
  }
  
  /**
   * Lista cadastros centrais com paginação
   */
  async listCentralRegistries() {
    this.showHeader('LISTAR CADASTROS CENTRAIS');
    
    if (!this.config.baseUrl) {
      console.log('❌ Configure a conexão primeiro (opção C)');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    try {
      const client = new MbcortexClient(this.config.baseUrl, this.config.password);
      
      console.log('📝 Fazendo login...');
      await client.login();
      
      let offset = 0;
      const count = 5; // Mostra 5 por página para melhor visualização
      
      while (true) {
        console.log(`\n📋 Listando cadastros (página ${Math.floor(offset/count) + 1})...`);
        
        const result = await client.listCentralRegistries(offset, count);
        
        if (result.items && result.items.length > 0) {
          console.log('\n📊 Cadastros encontrados:');
          console.log('─'.repeat(70));
          
          result.items.forEach((item, index) => {
            console.log(`${offset + index + 1}. ID: ${item.id || 'N/A'}`);
            console.log(`   Nome: ${item.name || item.descricao || 'Sem nome'}`);
            console.log(`   Status: ${item.enabled || item.ativo ? '🟢 Ativo' : '🔴 Inativo'}`);
            console.log(`   Slots: T1=${item.slots1 || item.slots_tier_1 || 0}, T2=${item.slots2 || item.slots_tier_2 || 0}`);
            console.log(`   Tipo: ${item.type || item.tipo || 0}`);
            console.log('');
          });
          
          console.log('─'.repeat(70));
          console.log(`📈 Total: ${result.total} cadastro(s) | Exibindo ${offset + 1}-${Math.min(offset + count, result.total)}`);
          
          // Menu de navegação
          console.log('\n🔄 Navegação:');
          if (result.hasPrevious) console.log('  [P] Página anterior');
          if (result.hasNext) console.log('  [N] Próxima página');
          console.log('  [0] Voltar ao menu principal');
          
          const nav = (await this.question('\nEscolha: ')).trim().toUpperCase();
          
          if (nav === '0') {
            break;
          } else if (nav === 'P' && result.hasPrevious) {
            offset = Math.max(0, offset - count);
          } else if (nav === 'N' && result.hasNext) {
            offset += count;
          } else if (nav === 'P' || nav === 'N') {
            console.log('\n⚠️  Navegação não disponível nesta direção');
            await this.question('Pressione ENTER para continuar...');
          }
          
        } else {
          console.log('\nℹ️  Nenhum cadastro encontrado');
          break;
        }
      }
      
    } catch (error) {
      console.log('\n❌ Erro ao listar cadastros:');
      
      if (error instanceof AuthError) {
        console.log('   🔐 Falha de autenticação - verifique senha');
      } else {
        console.log(`   ⚠️  ${error.message}`);
      }
    }
    
    await this.question('\nPressione ENTER para continuar...');
  }
  
  /**
   * Cria novo cadastro central
   */
  async createCentralRegistry() {
    this.showHeader('NOVO CADASTRO CENTRAL');
    
    if (!this.config.baseUrl) {
      console.log('❌ Configure a conexão primeiro (opção C)');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    console.log('📋 Informe os dados da nova unidade:');
    console.log('');
    
    const name = (await this.question('Nome da unidade: ')).trim();
    if (!name) {
      console.log('❌ Nome é obrigatório');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    const idInput = (await this.question('ID (0 = automático, >2000000 = fixo): ')).trim();
    const id = parseInt(idInput) || 0;
    
    if (id > 0 && id < 2000000) {
      console.log('⚠️  Aviso: IDs < 2000000 podem conflitar com IDs existentes');
      const confirm = (await this.question('Continuar mesmo assim? (s/N): ')).trim().toLowerCase();
      if (confirm !== 's' && confirm !== 'sim') {
        console.log('❌ Operação cancelada');
        await this.question('\nPressione ENTER para continuar...');
        return;
      }
    }
    
    try {
      const client = new MbcortexClient(this.config.baseUrl, this.config.password);
      
      console.log('\n📝 Fazendo login...');
      await client.login();
      
      console.log('\n🏢 Criando cadastro central...');
      let createdId;
      
      if (id === 0) {
        createdId = await client.createUnitAuto(name);
        console.log(`✅ Cadastro criado com ID automático: ${createdId}`);
      } else {
        createdId = await client.createUnitFixed(id, name);
        console.log(`✅ Cadastro criado com ID fixo: ${createdId}`);
      }
      
      console.log(`   📋 Nome: ${name}`);
      console.log( `   🔧 Configurações padrão aplicadas`);
      
    } catch (error) {
      console.log('\n❌ Erro ao criar cadastro:');
      
      if (error instanceof AuthError) {
        console.log('   🔐 Falha de autenticação - verifique senha');
      } else if (error instanceof ValidationError) {
        console.log('   📝 Dados inválidos - verifique os valores informados');
      } else if (error instanceof ConflictError) {
        console.log('   ⚔️  Conflito - ID já existe ou dados duplicados');
      } else {
        console.log(`   ⚠️  ${error.message}`);
      }
    }
    
    await this.question('\nPressione ENTER para continuar...');
  }
  
  /**
   * Busca cadastro por ID
   */
  async getCentralRegistryById() {
    this.showHeader('BUSCAR CADASTRO POR ID');
    
    if (!this.config.baseUrl) {
      console.log('❌ Configure a conexão primeiro (opção C)');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    const idInput = (await this.question('ID do cadastro: ')).trim();
    const id = parseInt(idInput);
    
    if (!id || id <= 0) {
      console.log('❌ ID deve ser um número válido maior que 0');
      await this.question('\nPressione ENTER para continuar...');
      return;
    }
    
    try {
      const client = new MbcortexClient(this.config.baseUrl, this.config.password);
      
      console.log('\n📝 Fazendo login...');
      await client.login();
      
      console.log(`\n🔍 Buscando cadastro ID ${id}...`);
      const registry = await client.getCentralRegistry(id);
      
      if (registry) {
        console.log('\n✅ Cadastro encontrado:');
        console.log('─'.repeat(50));
        console.log(`📋 ID: ${registry.id || 'N/A'}`);
        console.log(`🏢 Nome: ${registry.name || registry.descricao || 'Sem nome'}`);
        console.log(`🔄 Status: ${registry.enabled || registry.ativo ? '🟢 Ativo' : '🔴 Inativo'}`);
        console.log(`🔌 Slots T1: ${registry.slots1 || registry.slots_tier_1 || 0}`);
        console.log(`🔌 Slots T2: ${registry.slots2 || registry.slots_tier_2 || 0}`);
        console.log(`📂 Tipo: ${registry.type || registry.tipo || 0}`);
        
        if (registry.created_at) {
          console.log(`📅 Criado: ${new Date(registry.created_at).toLocaleString('pt-BR')}`);
        }
        
        console.log('─'.repeat(50));
        
        // Opção para listar entidades deste cadastro
        const listEntities = (await this.question('\nListar entidades deste cadastro? (s/N): ')).trim().toLowerCase();
        if (listEntities === 's' || listEntities === 'sim') {
          console.log('\n📋 Listando entidades...');
          
          const entities = await client.listEntitiesByUnit(id, 0, 10);
          
          if (entities.items && entities.items.length > 0) {
            console.log(`\n👥 ${entities.total} entidade(s) encontrada(s):`);
            
            entities.items.forEach((entity, index) => {
              const type = entity.type || entity.tipo;
              const icon = type === 1 ? '👤' : type === 2 ? '🚗' : '❓';
              const typeText = type === 1 ? 'Pessoa' : type === 2 ? 'Veículo' : `Tipo ${type}`;
              
              console.log(`  ${index + 1}. ${icon} ${entity.name || entity.nome || 'Sem nome'} (${typeText})`);
              console.log(`      Doc: ${entity.doc || 'N/A'}`);
            });
          } else {
            console.log('\nℹ️  Nenhuma entidade cadastrada nesta unidade');
          }
        }
        
      } else {
        console.log('\n❌ Cadastro não encontrado');
        console.log(`   ID ${id} não existe ou foi removido`);
      }
      
    } catch (error) {
      console.log('\n❌ Erro ao buscar cadastro:');
      
      if (error instanceof AuthError) {
        console.log('   🔐 Falha de autenticação - verifique senha');
      } else if (error instanceof NotFoundError) {
        console.log(`   📭 Cadastro ID ${id} não encontrado`);
      } else {
        console.log(`   ⚠️  ${error.message}`);
      }
    }
    
    await this.question('\nPressione ENTER para continuar...');
  }
  
  /**
   * Mostra informações sobre o sistema
   */
  async showAbout() {
    this.showHeader('SOBRE O SISTEMA');
    
    console.log('  MobiCortex Master - CLI Interativo');
    console.log('  Versão: 1.0.0');
    console.log('');
    console.log('  Este CLI demonstra todas as funcionalidades da');
    console.log('  API REST da controladora MobiCortex Master.');
    console.log('');
    console.log('  🚀 Funcionalidades:');
    console.log('  ├─ Autenticação Bearer token');
    console.log('  ├─ Gestão de cadastros centrais');
    console.log('  ├─ Gestão de entidades (pessoas/veículos)');
    console.log('  ├─ Campos opcionais para veículos');
    console.log('  ├─ Suporte a IDs automáticos e fixos');
    console.log('  ├─ Paginação e filtros');
    console.log('  └─ Tratamento robusto de erros');
    console.log('');
    console.log('  📋 Requisitos:');
    console.log('  ├─ Node.js 18+ (fetch nativo)');
    console.log('  ├─ Acesso à controladora na rede');
    console.log('  └─ Credenciais válidas');
    console.log('');
    console.log('  📚 Uso como biblioteca:');
    console.log('  const { MbcortexClient } = require(\'./src\');');
    console.log('');
    
    await this.question('Pressione ENTER para continuar...');
  }
  
  /**
   * Loop principal do CLI
   */
  async run() {
    try {
      while (true) {
        const choice = await this.showMainMenu();
        
        switch (choice) {
          case '0':
            console.log('\n👋 Até logo!');
            this.rl.close();
            process.exit(0);
            break;
            
          case 'C':
            await this.configureConnection();
            break;
            
          case 'T':
            await this.testConnection();
            break;
            
          case '1':
            await this.runQuickTest('auto');
            break;
            
          case '2':
            await this.runQuickTest('fixed');
            break;
            
          case '3':
            await this.listCentralRegistries();
            break;
            
          case '4':
            await this.createCentralRegistry();
            break;
            
          case '5':
            await this.getCentralRegistryById();
            break;
            
          case '6':
            await this.createPerson();
            break;
            
          case '7':
            await this.createVehicle();
            break;
            
          case '8':
            await this.listEntitiesByUnit();
            break;
            
          case '9':
            await this.searchEntities();
            break;
            
          case 'I':
            await this.showAbout();
            break;
            
          default:
            console.log('\n❌ Opção não implementada ainda ou inválida');
            await this.question('Pressione ENTER para continuar...');
        }
      }
    } catch (error) {
      console.error('\n💥 Erro fatal:', error.message);
      this.rl.close();
      process.exit(1);
    }
  }
}

// Executa apenas se chamado diretamente
if (require.main === module) {
  const cli = new InteractiveCLI();
  cli.run();
}