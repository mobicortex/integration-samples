#!/usr/bin/env node

/**
 * Exemplo Básico - Uso como Biblioteca
 * ====================================
 * 
 * Demonstra como usar o MobiCortex Master como biblioteca
 * reutilizável no seu próprio código.
 * 
 * Este exemplo mostra:
 * ✅ Import da biblioteca
 * ✅ Autenticação 
 * ✅ Operações básicas (criar, listar, deletar)
 * ✅ Tratamento de erros específicos
 * ✅ Uso de campos opcionais em veículos
 * 
 * USO:
 *   node examples/basic_usage.js
 * 
 * AUTOR:
 *   MobiCortex
 */

// 📚 Import da biblioteca (como se fosse npm install)
const { 
  MbcortexClient,
  AuthError,
  ValidationError,
  ConflictError,
  Vehicle
} = require('../src');

/**
 * Exemplo prático de uso da biblioteca
 */
async function exemploBasico() {
  console.log('🚀 MobiCortex Master - Exemplo de Uso como Biblioteca');
  console.log('====================================================');
  console.log('');
  
  // 1️⃣ Configuração da conexão
  const baseUrl = 'https://192.168.0.21:4449';  // ⚠️ ALTERE para seu IP
  const password = 'admin';                      // ⚠️ ALTERE para sua senha
  
  console.log(`📡 Conectando a: ${baseUrl}`);
  console.log('⚠️  AVISO: Configure o IP da sua controladora para testar');
  console.log('');
  
  // 2️⃣ Cria instância do cliente
  const client = new MbcortexClient(baseUrl, password);
  
  try {
    // 3️⃣ Autentica na controladora
    console.log('🔐 Fazendo login...');
    const sessionKey = await client.login();
    console.log(`✅ Autenticado! Session: ${sessionKey.substring(0, 16)}...`);
    console.log('');
    
    // 4️⃣ Cria uma unidade (empresa/cadastro central)
    console.log('🏢 Criando cadastro central...');
    const unitId = await client.createUnitAuto('Minha Empresa LTDA');
    console.log(`✅ Unidade criada: ID=${unitId}`);
    console.log('');
    
    try {
      // 5️⃣ Cria veículo com campos opcionais
      console.log('🚗 Criando veículo com informações completas...');
      
      const vehicleOptions = {
        brand: 'Toyota',      // Marca (opcional)
        model: 'Corolla',     // Modelo (opcional)
        color: 'Prata',       // Cor (opcional)
        obs: 'Veículo da diretoria'  // Observações (opcional)
      };
      
      const vehicleId = await client.createVehicleAuto(
        unitId,                    // ID da unidade
        'João Silva',              // Nome do proprietário
        'ABC-1234',                // Placa (será normalizada para ABC1234)
        1,                         // LPR ativo (1 = sim, 0 = não)
        vehicleOptions             // Campos opcionais
      );
      
      console.log(`✅ Veículo criado: ID=${vehicleId}`);
      console.log('📋 Dados do veículo:');
      console.log(`   ├─ Proprietário: João Silva`);
      console.log(`   ├─ Placa: ABC1234`);
      console.log(`   ├─ Marca: ${vehicleOptions.brand}`);
      console.log(`   ├─ Modelo: ${vehicleOptions.model}`);
      console.log(`   ├─ Cor: ${vehicleOptions.color}`);
      console.log(`   ├─ LPR: ATIVO`);
      console.log(`   └─ Obs: ${vehicleOptions.obs}`);
      console.log('');
      
      // 6️⃣ Remove o veículo (cleanup)
      console.log('🗑️  Removendo veículo...');
      await client.deleteVehicle(vehicleId);
      console.log('✅ Veículo removido');
      
    } catch (vehicleError) {
      console.log('❌ Erro ao gerenciar veículo:', vehicleError.message);
    }
    
    // 7️⃣ Remove a unidade (cleanup)
    console.log('🗑️  Removendo unidade...');
    await client.deleteUnit(unitId);
    console.log('✅ Unidade removida');
    console.log('');
    
    // 🎉 Sucesso!
    console.log('🎉 Exemplo concluído com sucesso!');
    console.log('');
    console.log('💡 Próximos passos:');
    console.log('   1. Integre este código no seu projeto');
    console.log('   2. Adapte para suas necessidades específicas');
    console.log('   3. Veja examples/cli.js para funcionalidades avançadas');
    console.log('');
    
  } catch (error) {
    console.log('❌ Erro durante execução:');
    console.log('');
    
    // 🔍 Tratamento específico por tipo de erro  
    if (error.code === 'ECONNREFUSED' || error.code === 'ENOTFOUND') {
      console.log('🌐 ERRO DE CONEXÃO:');
      console.log('   ├─ Este exemplo precisa de uma controladora MobiCortex');
      console.log('   ├─ Configure o IP correto no início do arquivo');
      console.log('   └─ Para testar sem controladora, veja as classes de modelo abaixo');
      
    } else if (error instanceof AuthError) {
      console.log('🔐 ERRO DE AUTENTICAÇÃO:');
      console.log('   └─ Verifique a senha da controladora');
      
    } else {
      console.log('⚠️  ERRO:');
      console.log(`   └─ ${error.message}`);
    }
    
    console.log('');
  }
}

/**
 * Exemplo de uso das classes de modelo (funciona sem controladora)
 */
async function exemploComModelos() {
  console.log('📚 Demonstração das Classes de Modelo');
  console.log('─'.repeat(40));
  
  // Demonstra uso das classes Vehicle e CentralRegistry
  const { CentralRegistry, Vehicle } = require('../src');
  
  // Cria objetos usando as classes
  const unidade = new CentralRegistry(0, 'Empresa XYZ', 1, 10, 5, 0);
  console.log('🏢 Cadastro Central:', JSON.stringify(unidade.toApiObject(), null, 2));
  
  const veiculo = new Vehicle(0, 'Maria Santos', 'DEF-5678', 0, 1, {
    brand: 'Honda',
    model: 'Civic',
    color: 'Azul'
  });
  
  console.log('🚗 Veículo:', JSON.stringify(veiculo.toApiObject(), null, 2));
  console.log('✅ Placa válida:', veiculo.isValidPlate());
  
  // Demonstra normalização de placas
  console.log('');
  console.log('🔧 Normalização de placas:');
  const placas = ['abc-1234', 'DEF 5678', 'GHI9012'];
  
  placas.forEach(placa => {
    const normalizada = Vehicle.normalizePlate(placa);
    console.log(`   ${placa} → ${normalizada}`);
  });
  
  console.log('');
}

// 🚀 Executa os exemplos
if (require.main === module) {
  exemploBasico()
    .then(() => exemploComModelos())
    .catch(console.error);
}