#!/usr/bin/env node

/**
 * DEPRECATED: Este arquivo será removido em versões futuras.
 * Use examples/cli.js em vez disso.
 * 
 * Redirecionamento temporário para compatibilidade.
 */

console.log('⚠️  main.js está deprecated. Use examples/cli.js');
console.log('   Redirecionando automaticamente...');
console.log('');

// Redireciona para o novo CLI
require('./examples/cli');

/**
 * Faz parse manual dos argumentos da linha de comando
 * Não usa bibliotecas externas para manter simplicidade
 * @returns {object} Objeto com as opções parseadas
 */
function parseArgs() {
  const args = process.argv.slice(2); // Remove 'node' e nome do script
  const options = {
    baseUrl: null,
    password: 'admin',        // Valor padrão
    mode: 'auto',            // 'auto' ou 'fixed'
    unitId: 2000001,         // ID fixo para teste (faixa 2E6+)
    vehicleId: 2000002,      // ID fixo para teste (faixa 2E6+)
    plate: 'ABC1234',        // Placa padrão
    cleanupUnit: false       // Se deve apagar unidade no final
  };

  // Parse manual: --flag valor ou --flag (boolean)
  for (let i = 0; i < args.length; i++) {
    const arg = args[i];
    
    if (arg === '--base-url' && i + 1 < args.length) {
      options.baseUrl = args[++i];
    } else if (arg === '--password' && i + 1 < args.length) {
      options.password = args[++i];
    } else if (arg === '--mode' && i + 1 < args.length) {
      options.mode = args[++i];
    } else if (arg === '--unit-id' && i + 1 < args.length) {
      options.unitId = parseInt(args[++i]);
    } else if (arg === '--vehicle-id' && i + 1 < args.length) {
      options.vehicleId = parseInt(args[++i]);
    } else if (arg === '--plate' && i + 1 < args.length) {
      options.plate = args[++i];
    } else if (arg === '--cleanup-unit') {
      options.cleanupUnit = true;
    } else if (arg === '--help') {
      showHelp();
      process.exit(0);
    }
  }

  return options;
}

/**
 * Exibe ajuda de uso do CLI
 */
function showHelp() {
  console.log(`
Uso: node main.js [opções]

Opções:
  --base-url <url>      URL base da controladora (obrigatório)
                        Exemplo: http://192.168.0.10
  
  --password <senha>    Senha da controladora (padrão: admin)
  
  --mode <modo>         Modo de criação de IDs:
                        auto  = IDs gerados automaticamente
                        fixed = IDs específicos fornecidos
  
  --unit-id <id>        ID específico para unidade (modo fixed)
                        Padrão: 2000001
  
  --vehicle-id <id>     ID específico para veículo (modo fixed)  
                        Padrão: 2000002
  
  --plate <placa>       Placa do veículo (padrão: ABC1234)
  
  --cleanup-unit        Remove unidade no final (opcional)
  
  --help               Mostra esta ajuda

Exemplos:
  node main.js --base-url http://192.168.0.10 --mode auto
  node main.js --base-url http://192.168.0.10 --mode fixed --unit-id 2000050 --vehicle-id 2000051
  node main.js --base-url http://192.168.0.10 --mode auto --cleanup-unit
`);
}

/**
 * Função principal que executa o fluxo completo
 */
async function main() {
  const options = parseArgs();
  
  // Validação: base-url é obrigatório
  if (!options.baseUrl) {
    console.error('❌ --base-url é obrigatório');
    console.error('Use --help para ver as opções disponíveis');
    process.exit(1);
  }
  
  // Validação: mode deve ser 'auto' ou 'fixed'
  if (options.mode !== 'auto' && options.mode !== 'fixed') {
    console.error('❌ --mode deve ser "auto" ou "fixed"');
    process.exit(1);
  }
  
  console.log('🚀 MobiCortex Master - Exemplo Node.js');
  console.log('=====================================');
  console.log(`URL: ${options.baseUrl}`);
  console.log(`Modo: ${options.mode}`);
  console.log(`Placa: ${options.plate}`);
  console.log('');

  const client = new MbcortexClient(options.baseUrl, options.password);
  let unitId = null;
  let vehicleId = null;

  try {
    // ==========================================
    // PASSO 1: LOGIN
    // ==========================================
    console.log('📝 Passo 1: Fazendo login...');
    await client.login();
    console.log('✅ Login realizado com sucesso');
    console.log('');

    // ==========================================
    // PASSO 2: CRIAR UNIDADE
    // ==========================================
    console.log('🏢 Passo 2: Criando unidade...');
    
    if (options.mode === 'auto') {
      // Modo automático: controladora gera ID (faixa 4294000000+)
      console.log('→ Usando ID automático (controladora decide)');
      unitId = await client.createUnitAuto('Unidade Teste Auto');
    } else {
      // Modo fixo: usar ID específico (faixa 2000000+ para teste)
      console.log(`→ Usando ID fixo: ${options.unitId}`);
      unitId = await client.createUnitFixed(options.unitId, 'Unidade Teste Fixed');
    }
    
    console.log(`✅ Unidade criada com ID: ${unitId}`);
    console.log('');

    // ==========================================
    // PASSO 3: CRIAR VEÍCULO
    // ==========================================
    console.log('🚗 Passo 3: Criando veículo...');
    
    if (options.mode === 'auto') {
      // Modo automático: createid=true
      console.log('→ Usando ID automático (createid=true)');
      vehicleId = await client.createVehicleAuto(unitId, 'Veículo Teste Auto', options.plate);
    } else {
      // Modo fixo: ID específico
      console.log(`→ Usando ID fixo: ${options.vehicleId}`);
      vehicleId = await client.createVehicleFixed(options.vehicleId, unitId, 'Veículo Teste Fixed', options.plate);
    }
    
    console.log(`✅ Veículo criado com ID: ${vehicleId}`);
    console.log(`   Placa: ${options.plate} | LPR: ATIVO`);
    console.log('');

    // ==========================================
    // PASSO 4: APAGAR VEÍCULO
    // ==========================================
    console.log('🗑️  Passo 4: Removendo veículo...');
    await client.deleteVehicle(vehicleId);
    console.log('');

    // ==========================================
    // PASSO 5: APAGAR UNIDADE (OPCIONAL)
    // ==========================================
    if (options.cleanupUnit) {
      console.log('🗑️  Passo 5: Removendo unidade...');
      await client.deleteUnit(unitId);
    } else {
      console.log('📌 Unidade mantida (use --cleanup-unit para remover)');
      console.log(`   ID da unidade: ${unitId}`);
    }
    
    console.log('');
    console.log('🎉 Fluxo concluído com sucesso!');

  } catch (error) {
    console.error('');
    console.error('❌ Erro durante execução:');
    console.error(error.message);
    console.error('');
    
    // Cleanup em caso de erro: tenta remover recursos criados
    console.log('🧹 Tentando limpeza após erro...');
    
    try {
      if (vehicleId) {
        await client.deleteVehicle(vehicleId);
      }
      if (unitId && options.cleanupUnit) {
        await client.deleteUnit(unitId);
      }
    } catch (cleanupError) {
      console.error('⚠️  Erro durante limpeza:', cleanupError.message);
    }
    
    process.exit(1);
  }
}

// Executa apenas se chamado diretamente (não via require)
if (require.main === module) {
  main().catch((error) => {
    console.error('❌ Erro fatal:', error.message);
    process.exit(1);
  });
}