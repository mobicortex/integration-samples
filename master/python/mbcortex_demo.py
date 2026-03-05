#!/usr/bin/env python3
"""
MobiCortex Master - Demo Interativa
====================================

Script unico para demonstracao da API REST do MobiCortex Master.

Funcionalidades:
- Login na controladora
- Criar unidades (com ID auto ou fixo)
- Criar veiculos com LPR
- Apagar registros

USO:
    python mbcortex_demo.py

REQUISITOS:
    Python 3.8+ (apenas biblioteca padrao)

AUTOR:
    MobiCortex
"""

import json
import os
import ssl
import urllib.request
import urllib.error
import sys
from typing import Optional, Tuple, Any, Dict

# Arquivo para salvar ultimas configuracoes
CONFIG_FILE = os.path.join(os.path.dirname(os.path.abspath(__file__)), "mbcortex_config.json")

# Cria contexto SSL que ignora verificacao de certificado (para certificados auto-assinados)
SSL_CONTEXT = ssl.create_default_context()
SSL_CONTEXT.check_hostname = False
SSL_CONTEXT.verify_mode = ssl.CERT_NONE


# ============================================================================
# CORES PARA TERMINAL
# ============================================================================
class Colors:
    HEADER = '\033[95m'
    BLUE = '\033[94m'
    CYAN = '\033[96m'
    GREEN = '\033[92m'
    WARNING = '\033[93m'
    FAIL = '\033[91m'
    ENDC = '\033[0m'
    BOLD = '\033[1m'
    UNDERLINE = '\033[4m'


def colorize(text: str, color: str) -> str:
    """Aplica cor ao texto se terminal suportar."""
    if sys.platform == 'win32':
        return text
    return f"{color}{text}{Colors.ENDC}"


# ============================================================================
# EXCECOES
# ============================================================================
class MbcortexError(Exception):
    """Excecao base para erros da API."""
    def __init__(self, message: str, status_code: int = 0, method: str = "", url: str = "", response_data: Any = None):
        super().__init__(message)
        self.status_code = status_code
        self.method = method
        self.url = url
        self.response_data = response_data


class AuthError(MbcortexError):
    pass


class NotFoundError(MbcortexError):
    pass


class ConflictError(MbcortexError):
    pass


class ValidationError(MbcortexError):
    pass


class ServerError(MbcortexError):
    pass


# ============================================================================
# CLIENTE HTTP
# ============================================================================
class MbcortexClient:
    """Cliente HTTP para API MobiCortex Master."""
    
    def __init__(self, base_url: str, username: str = "master", password: str = "1234", timeout: float = 10.0):
        self.base_url = base_url.rstrip("/")
        self.username = username
        self.password = password
        self.timeout = timeout
        self.session_key: Optional[str] = None
    
    def _request(self, method: str, path: str, json_data: Optional[Dict] = None,
                 params: Optional[Dict] = None, skip_auth: bool = False) -> Tuple[int, Any]:
        """Faz requisicao HTTP."""
        url = f"{self.base_url}{path}"
        if params:
            query = "&".join(f"{k}={v}" for k, v in params.items())
            url = f"{url}?{query}"
        
        data = json.dumps(json_data).encode("utf-8") if json_data else None
        req = urllib.request.Request(url, data=data, method=method)
        
        if data:
            req.add_header("Content-Type", "application/json")
        
        if not skip_auth:
            if not self.session_key:
                raise AuthError("Nao autenticado. Execute login() primeiro.")
            req.add_header("Authorization", f"Bearer {self.session_key}")
        
        try:
            with urllib.request.urlopen(req, timeout=self.timeout, context=SSL_CONTEXT) as response:
                status = response.getcode()
                body = response.read().decode("utf-8")
                try:
                    return status, json.loads(body) if body else None
                except json.JSONDecodeError:
                    return status, body
        except urllib.error.HTTPError as e:
            status = e.code
            body = e.read().decode("utf-8") if e.read() else ""
            try:
                response_data = json.loads(body) if body else None
            except:
                response_data = body
            
            error_classes = {400: ValidationError, 401: AuthError, 404: NotFoundError, 409: ConflictError}
            error_class = error_classes.get(status, ServerError if status >= 500 else MbcortexError)
            message = response_data.get("error", body) if isinstance(response_data, dict) else body
            raise error_class(message or f"Erro HTTP {status}", status, method, url, response_data)
        except urllib.error.URLError as e:
            raise MbcortexError(f"Erro de conexao: {e.reason}")
    
    def login(self) -> str:
        """Autentica na controladora."""
        status, data = self._request("POST", "/mbcortex/master/api/v1/login",
                                     json_data={"pass": self.password}, skip_auth=True)
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            self.session_key = data.get("session_key")
            if self.session_key:
                return self.session_key
        raise AuthError("Falha no login: resposta invalida")
    
    def create_unit_auto(self, name: str, enabled: int = 1, slots1: int = 0, 
                         slots2: int = 0, type_: int = 0) -> int:
        """Cria unidade com ID automatico."""
        status, data = self._request("POST", "/mbcortex/master/api/v1/central-registry",
            json_data={"id": 0, "name": name, "enabled": enabled, "slots1": slots1, "slots2": slots2, "type": type_})
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            return data.get("id", 0)
        raise MbcortexError(f"Falha ao criar unidade: {data}")
    
    def create_unit_fixed(self, unit_id: int, name: str, enabled: int = 1,
                          slots1: int = 0, slots2: int = 0, type_: int = 0) -> int:
        """Cria unidade com ID fixo."""
        status, data = self._request("POST", "/mbcortex/master/api/v1/central-registry",
            json_data={"id": unit_id, "name": name, "enabled": enabled, "slots1": slots1, "slots2": slots2, "type": type_})
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            return data.get("id", unit_id)
        raise MbcortexError(f"Falha ao criar unidade: {data}")
    
    def delete_unit(self, unit_id: int) -> None:
        """Apaga unidade."""
        status, data = self._request("DELETE", "/mbcortex/master/api/v1/central-registry", params={"id": unit_id})
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            return
        raise MbcortexError(f"Falha ao apagar unidade: {data}")
    
    def create_vehicle_auto(self, unit_id: int, name: str, plate: str, lpr_ativo: int = 1) -> int:
        """Cria veiculo com ID automatico."""
        plate_norm = plate.upper().replace(" ", "").replace("-", "")
        status, data = self._request("POST", "/mbcortex/master/api/v1/entities",
            json_data={"createid": True, "tipo": 2, "name": name, "doc": plate_norm,
                       "cadastro_id": unit_id, "lpr_ativo": lpr_ativo})
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            return data.get("entity_id", 0)
        raise MbcortexError(f"Falha ao criar veiculo: {data}")
    
    def create_vehicle_fixed(self, vehicle_id: int, unit_id: int, name: str, 
                             plate: str, lpr_ativo: int = 1) -> int:
        """Cria veiculo com ID fixo."""
        plate_norm = plate.upper().replace(" ", "").replace("-", "")
        status, data = self._request("POST", "/mbcortex/master/api/v1/entities",
            json_data={"id": vehicle_id, "tipo": 2, "name": name, "doc": plate_norm,
                       "cadastro_id": unit_id, "lpr_ativo": lpr_ativo})
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            return data.get("entity_id", vehicle_id)
        raise MbcortexError(f"Falha ao criar veiculo: {data}")
    
    def delete_vehicle(self, vehicle_id: int) -> None:
        """Apaga veiculo."""
        status, data = self._request("DELETE", "/mbcortex/master/api/v1/entities", params={"id": vehicle_id})
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            return
        raise MbcortexError(f"Falha ao apagar veiculo: {data}")


# ============================================================================
# INTERFACE DO MENU
# ============================================================================
class Menu:
    """Menu interativo CLI."""
    
    def __init__(self):
        self.config = {}
    
    def load_config(self) -> Dict:
        """Carrega configuracoes salvas do arquivo."""
        try:
            if os.path.exists(CONFIG_FILE):
                with open(CONFIG_FILE, 'r') as f:
                    return json.load(f)
        except Exception:
            pass
        return {}
    
    def save_config(self, config: Dict):
        """Salva configuracoes no arquivo."""
        try:
            # Salva apenas dados de conexao (sem dados do teste)
            dados_salvar = {
                "ip": config.get("ip", ""),
                "port": config.get("port", 443),
                "username": config.get("username", "master"),
                "password": config.get("password", "1234"),
                "timeout": config.get("timeout", 10)
            }
            with open(CONFIG_FILE, 'w') as f:
                json.dump(dados_salvar, f, indent=2)
        except Exception as e:
            self.error(f"Nao foi possivel salvar configuracao: {e}")
    
    def clear(self):
        """Limpa a tela."""
        print("\n" * 2)
    
    def header(self, title: str):
        """Imprime cabecalho."""
        print(colorize("\n" + "=" * 60, Colors.CYAN))
        print(colorize(f"  {title}", Colors.CYAN + Colors.BOLD))
        print(colorize("=" * 60, Colors.CYAN))
    
    def success(self, msg: str):
        print(colorize(f"  ✓ {msg}", Colors.GREEN))
    
    def error(self, msg: str):
        print(colorize(f"  ✗ {msg}", Colors.FAIL))
    
    def info(self, msg: str):
        print(colorize(f"  ℹ {msg}", Colors.BLUE))
    
    def step(self, num: int, desc: str):
        print(colorize(f"\n{'─' * 60}", Colors.CYAN))
        print(colorize(f"► PASSO {num}: {desc}", Colors.CYAN + Colors.BOLD))
        print(colorize("─" * 60, Colors.CYAN))
    
    def input_default(self, prompt: str, default: str = "") -> str:
        """Input com valor padrao."""
        if default:
            val = input(f"{prompt} [{default}]: ").strip()
            return val if val else default
        return input(f"{prompt}: ").strip()
    
    def input_required(self, prompt: str) -> str:
        """Input obrigatorio."""
        while True:
            val = input(f"{prompt}: ").strip()
            if val:
                return val
            self.error("Este campo e obrigatorio")
    
    def input_int(self, prompt: str, default: Optional[int] = None, 
                  min_val: Optional[int] = None, max_val: Optional[int] = None) -> int:
        """Input inteiro com validacao."""
        while True:
            ds = f" [{default}]" if default is not None else ""
            val = input(f"{prompt}{ds}: ").strip()
            if not val and default is not None:
                return default
            try:
                num = int(val)
                if min_val is not None and num < min_val:
                    self.error(f"Valor minimo: {min_val}")
                    continue
                if max_val is not None and num > max_val:
                    self.error(f"Valor maximo: {max_val}")
                    continue
                return num
            except ValueError:
                self.error("Digite um numero valido")
    
    def input_yes_no(self, prompt: str, default: bool = False) -> bool:
        """Input sim/nao."""
        suffix = " (S/n)" if default else " (s/N)"
        while True:
            val = input(f"{prompt}{suffix}: ").strip().lower()
            if not val:
                return default
            if val in ("s", "sim", "yes", "y"):
                return True
            if val in ("n", "nao", "no"):
                return False
            self.error("Digite S ou N")
    
    def show_banner(self):
        """Mostra banner inicial."""
        print(colorize("""
╔══════════════════════════════════════════════════════════════╗
║                                                              ║
║           MOBICORTEX MASTER - DEMO INTERATIVA                ║
║                                                              ║
║   Teste de integracao com a API REST da controladora         ║
║                                                              ║
╚══════════════════════════════════════════════════════════════╝
""", Colors.CYAN + Colors.BOLD))
    
    def show_menu_principal(self) -> str:
        """Menu principal."""
        self.header("MENU PRINCIPAL")
        print("\n  [1] Teste Rapido - Modo AUTO (IDs automaticos)")
        print("  [2] Teste Rapido - Modo FIXED (IDs fixos)")
        print("  [3] Cadastrar um carro")
        print("  [4] Sobre")
        print("  [0] Sair")
        print("")
        
        while True:
            opcao = input("Escolha uma opcao: ").strip()
            if opcao in ("0", "1", "2", "3", "4"):
                return opcao
            self.error("Opcao invalida. Digite 0, 1, 2, 3 ou 4.")
    
    def configurar_conexao(self) -> Dict:
        """Configura parametros de conexao."""
        # Carrega configuracoes salvas
        saved = self.load_config()
        
        self.header("CONFIGURACAO DA CONEXAO")
        
        # Mostra valores salvos se existirem
        if saved:
            print(colorize("\n  [Valores anteriores carregados - pressione ENTER para manter]\n", Colors.BLUE))
        
        # IP/Hostname
        ip_default = saved.get("ip", "")
        if ip_default:
            ip = self.input_default("IP ou hostname da controladora", ip_default)
        else:
            ip = self.input_required("IP ou hostname da controladora")
        
        # Porta
        port = self.input_int("Porta HTTPS", default=saved.get("port", 443), min_val=1, max_val=65535)
        
        # URL base (HTTPS)
        if port == 443:
            base_url = f"https://{ip}"
        else:
            base_url = f"https://{ip}:{port}"
        
        # Usuario e Senha
        usuario = self.input_default("Usuario", saved.get("username", "master"))
        senha = self.input_default("Senha", saved.get("password", "1234"))
        
        # Timeout
        timeout = self.input_int("Timeout (segundos)", default=saved.get("timeout", 10), min_val=1, max_val=300)
        
        result = {
            "ip": ip,
            "port": port,
            "base_url": base_url,
            "username": usuario,
            "password": senha,
            "timeout": timeout
        }
        
        # Salva configuracoes para proxima vez
        self.save_config(result)
        
        return result
    
    def configurar_teste_auto(self) -> Dict:
        """Configura teste modo AUTO."""
        self.header("TESTE MODO AUTO")
        print("\nNeste modo, a controladora gera automaticamente os IDs.\n")
        
        config = self.configurar_conexao()
        
        self.header("Dados do Veiculo")
        config["plate"] = self.input_default("Placa do veiculo", "ABC1234").upper()
        config["lpr_ativo"] = 1 if self.input_yes_no("Ativar LPR?", default=True) else 0
        config["cleanup_unit"] = self.input_yes_no("Apagar unidade no final?", default=False)
        config["mode"] = "auto"
        
        return config
    
    def configurar_teste_fixed(self) -> Dict:
        """Configura teste modo FIXED."""
        self.header("TESTE MODO FIXED")
        print("\nNeste modo, voce informa os IDs desejados.")
        print(colorize("Dica: Use valores >= 2000000 (2 milhoes) para evitar conflitos.\n", Colors.WARNING))
        
        config = self.configurar_conexao()
        
        self.header("IDs dos Registros")
        config["unit_id"] = self.input_int("ID da Unidade", default=2000000, min_val=1)
        config["vehicle_id"] = self.input_int("ID do Veiculo", default=2000000, min_val=1)
        
        self.header("Dados do Veiculo")
        config["plate"] = self.input_default("Placa do veiculo", "ABC1234").upper()
        config["lpr_ativo"] = 1 if self.input_yes_no("Ativar LPR?", default=True) else 0
        config["cleanup_unit"] = self.input_yes_no("Apagar unidade no final?", default=False)
        config["mode"] = "fixed"
        
        return config
    
    def configurar_cadastro_carro(self) -> Dict:
        """Configura cadastro de carro com escolha de ID."""
        self.header("CADASTRAR UM CARRO")
        print("\nInforme os IDs desejados ou 0 para gerar automaticamente.\n")
        
        config = self.configurar_conexao()
        
        self.header("IDs dos Registros")
        print(colorize("  (digite 0 para gerar automaticamente)\n", Colors.BLUE))
        
        unit_id = self.input_int("ID da Unidade", default=0, min_val=0)
        vehicle_id = self.input_int("ID do Veiculo", default=0, min_val=0)
        
        # Define modo baseado nos IDs informados
        if unit_id == 0 and vehicle_id == 0:
            config["mode"] = "auto"
        else:
            config["mode"] = "fixed"
            if unit_id == 0:
                unit_id = 2000000
            if vehicle_id == 0:
                vehicle_id = 2000000
        
        config["unit_id"] = unit_id
        config["vehicle_id"] = vehicle_id
        
        self.header("Dados do Veiculo")
        config["plate"] = self.input_default("Placa do veiculo", "ABC1234").upper()
        config["lpr_ativo"] = 1 if self.input_yes_no("Ativar LPR?", default=True) else 0
        config["cleanup_unit"] = self.input_yes_no("Apagar unidade no final?", default=False)
        
        return config
    
    def mostrar_resumo(self, config: Dict):
        """Mostra resumo da configuracao."""
        self.header("RESUMO DA CONFIGURACAO")
        print(f"\n  URL:      {config['base_url']}")
        print(f"  Usuario:  {config['username']}")
        print(f"  Senha:    {config['password']}")
        print(f"  Timeout:  {config['timeout']}s")
        print(f"  Modo:     {config['mode'].upper()}")
        if config["mode"] == "fixed":
            print(f"  Unidade:  {config['unit_id']}")
            print(f"  Veiculo:  {config['vehicle_id']}")
        print(f"  Placa:    {config['plate']}")
        print(f"  LPR:      {'Sim' if config['lpr_ativo'] else 'Nao'}")
        print(f"  Cleanup:  {'Sim' if config['cleanup_unit'] else 'Nao'}")
    
    def sobre(self):
        """Mostra informacoes sobre o programa."""
        self.header("SOBRE")
        print("""
  MobiCortex Master - Demo Interativa
  
  Versao: 1.0.0
  
  Este script demonstra a integracao com a API REST da
  controladora MobiCortex Master.
  
  Funcionalidades:
  - Autenticacao (login)
  - Criacao de unidades (central-registry)
  - Criacao de veiculos com LPR (entities)
  - Remocao de registros
  
  Requisitos: Python 3.8+ (apenas biblioteca padrao)
  
  Para mais informacoes, consulte a documentacao.
""")
        input("\nPressione ENTER para voltar ao menu...")


# ============================================================================
# EXECUCAO DO TESTE
# ============================================================================
class TestRunner:
    """Executa o fluxo de teste."""
    
    def __init__(self, menu: Menu):
        self.menu = menu
    
    def run(self, config: Dict) -> bool:
        """Executa o teste completo."""
        client = MbcortexClient(
            base_url=config["base_url"],
            username=config.get("username", "master"),
            password=config["password"],
            timeout=config["timeout"]
        )
        
        try:
            # PASSO 1: Login
            self.menu.step(1, "Autenticacao (login)")
            print(f"Conectando a: {config['base_url']}")
            session = client.login()
            self.menu.success(f"Login OK - Session: {session[:16]}...")
            
            unit_id = None
            vehicle_id = None
            
            try:
                # PASSO 2: Criar unidade
                if config["mode"] == "auto":
                    self.menu.step(2, "Criar unidade (ID automatico)")
                    unit_id = client.create_unit_auto(name="Demo Unidade", enabled=1, slots1=2)
                    self.menu.success(f"Unidade criada: ID={unit_id}")
                else:
                    self.menu.step(2, f"Criar unidade (ID fixo: {config['unit_id']})")
                    unit_id = client.create_unit_fixed(
                        unit_id=config["unit_id"], name="Demo Unidade", enabled=1, slots1=2
                    )
                    self.menu.success(f"Unidade criada: ID={unit_id}")
                
                # PASSO 3: Criar veiculo
                if config["mode"] == "auto":
                    self.menu.step(3, "Criar veiculo (ID automatico)")
                    vehicle_id = client.create_vehicle_auto(
                        unit_id=unit_id, name="Demo Veiculo",
                        plate=config["plate"], lpr_ativo=config["lpr_ativo"]
                    )
                    self.menu.success(f"Veiculo criado: ID={vehicle_id}")
                else:
                    self.menu.step(3, f"Criar veiculo (ID fixo: {config['vehicle_id']})")
                    vehicle_id = client.create_vehicle_fixed(
                        vehicle_id=config["vehicle_id"], unit_id=unit_id,
                        name="Demo Veiculo", plate=config["plate"],
                        lpr_ativo=config["lpr_ativo"]
                    )
                    self.menu.success(f"Veiculo criado: ID={vehicle_id}")
                
                # PASSO 4: Apagar veiculo
                self.menu.step(4, "Apagar veiculo")
                client.delete_vehicle(vehicle_id)
                self.menu.success("Veiculo apagado")
                vehicle_id = None
                
                # PASSO 5: Apagar unidade (se cleanup)
                if config["cleanup_unit"]:
                    self.menu.step(5, "Apagar unidade (cleanup)")
                    client.delete_unit(unit_id)
                    self.menu.success("Unidade apagada")
                    unit_id = None
                
                self.menu.header("TESTE CONCLUIDO COM SUCESSO!")
                return True
                
            except Exception:
                # Cleanup em caso de erro
                if vehicle_id:
                    try:
                        print("\n[Limpando] Apagando veiculo...")
                        client.delete_vehicle(vehicle_id)
                    except Exception as e:
                        print(f"Nao foi possivel limpar veiculo: {e}")
                if config["cleanup_unit"] and unit_id:
                    try:
                        print("[Limpando] Apagando unidade...")
                        client.delete_unit(unit_id)
                    except Exception as e:
                        print(f"Nao foi possivel limpar unidade: {e}")
                raise
                
        except AuthError as e:
            self.menu.error(f"Falha de autenticacao: {e}")
            self.menu.info("Verifique a senha e se a controladora esta acessivel")
        except ValidationError as e:
            self.menu.error(f"Dados invalidos: {e}")
            if "placa" in str(e).lower():
                self.menu.info("A placa deve estar em formato valido (ex: ABC1234)")
        except ConflictError as e:
            self.menu.error(f"Conflito: {e}")
            self.menu.info("O ID ou placa ja existe. Tente outros valores.")
        except NotFoundError as e:
            self.menu.error(f"Nao encontrado: {e}")
        except MbcortexError as e:
            self.menu.error(f"Erro da API: {e}")
            self.menu.info(f"Status: {e.status_code}, Metodo: {e.method}")
        except Exception as e:
            self.menu.error(f"Erro inesperado: {e}")
            import traceback
            traceback.print_exc()
        
        return False


# ============================================================================
# FUNCAO PRINCIPAL
# ============================================================================
def main():
    """Funcao principal."""
    menu = Menu()
    runner = TestRunner(menu)
    
    try:
        menu.show_banner()
        
        while True:
            opcao = menu.show_menu_principal()
            
            if opcao == "0":
                print(colorize("\nAte logo!\n", Colors.GREEN))
                break
            
            elif opcao == "1":
                # Teste rapido AUTO
                config = menu.configurar_teste_auto()
                menu.mostrar_resumo(config)
                print("")
                if menu.input_yes_no("Confirmar e executar?", default=True):
                    runner.run(config)
                    input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "2":
                # Teste rapido FIXED
                config = menu.configurar_teste_fixed()
                menu.mostrar_resumo(config)
                print("")
                if menu.input_yes_no("Confirmar e executar?", default=True):
                    runner.run(config)
                    input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "3":
                # Cadastrar um carro
                config = menu.configurar_cadastro_carro()
                menu.mostrar_resumo(config)
                print("")
                if menu.input_yes_no("Confirmar e executar?", default=True):
                    runner.run(config)
                    input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "4":
                menu.sobre()
    
    except KeyboardInterrupt:
        print(colorize("\n\nOperacao cancelada pelo usuario.", Colors.WARNING))
        sys.exit(0)


if __name__ == "__main__":
    main()
