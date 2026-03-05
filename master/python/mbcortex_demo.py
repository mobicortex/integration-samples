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
import re
from typing import Optional, Tuple, Any, Dict, List

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
    
    def create_vehicle_auto(self, unit_id: int, name: str, plate: str, lpr_ativo: int = 1, **kwargs) -> int:
        """Cria veiculo com ID automatico.
        
        Args:
            unit_id: ID do cadastro central
            name: Nome do proprietario/veiculo
            plate: Placa do veiculo (sera normalizada)
            lpr_ativo: Se deve ativar LPR (1=sim, 0=nao)
            **kwargs: Campos opcionais (brand, model, color, etc)
            
        Returns:
            ID do veiculo criado pela controladora
        """
        plate_norm = plate.upper().replace(" ", "").replace("-", "")
        
        # Payload basico obrigatorio
        payload = {
            "createid": True, 
            "tipo": 2,  # 2 = veiculo
            "name": name, 
            "doc": plate_norm,
            "cadastro_id": unit_id, 
            "lpr_ativo": lpr_ativo
        }
        
        # Adiciona campos opcionais se fornecidos
        for field in ['brand', 'model', 'color', 'obs']:
            if field in kwargs and kwargs[field]:
                payload[field] = kwargs[field]
        
        status, data = self._request("POST", "/mbcortex/master/api/v1/entities", json_data=payload)
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            return data.get("entity_id", 0)
        raise MbcortexError(f"Falha ao criar veiculo: {data}")
    
    def create_vehicle_fixed(self, vehicle_id: int, unit_id: int, name: str, 
                             plate: str, lpr_ativo: int = 1, **kwargs) -> int:
        """Cria veiculo com ID fixo.
        
        Args:
            vehicle_id: ID desejado para o veiculo
            unit_id: ID do cadastro central
            name: Nome do proprietario/veiculo
            plate: Placa do veiculo (sera normalizada)
            lpr_ativo: Se deve ativar LPR (1=sim, 0=nao)
            **kwargs: Campos opcionais (brand, model, color, etc)
            
        Returns:
            ID do veiculo criado (confirmado pela controladora)
        """
        plate_norm = plate.upper().replace(" ", "").replace("-", "")
        
        # Payload basico obrigatorio
        payload = {
            "id": vehicle_id,
            "tipo": 2,  # 2 = veiculo
            "name": name, 
            "doc": plate_norm,
            "cadastro_id": unit_id, 
            "lpr_ativo": lpr_ativo
        }
        
        # Adiciona campos opcionais se fornecidos
        for field in ['brand', 'model', 'color', 'obs']:
            if field in kwargs and kwargs[field]:
                payload[field] = kwargs[field]
        
        status, data = self._request("POST", "/mbcortex/master/api/v1/entities", json_data=payload)
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
    # METODOS DE CONSULTA E PAGINACAO  
    # ============================================================================
    
    def list_units_paginated(self, offset: int = 0, count: int = 20, name_filter: str = "") -> Dict[str, Any]:
        """Lista cadastros centrais com paginacao.
        
        Args:
            offset: Registro inicial (0-based)
            count: Quantidade de registros por pagina (max 100)
            name_filter: Filtro por nome (opcional)
            
        Returns:
            Dict com 'data' (lista de cadastros), 'total', 'offset', 'count'
        """
        params = {"offset": offset, "count": min(count, 100)}  # Limita a 100 por seguranca
        if name_filter.strip():
            params["name"] = name_filter.strip()
            
        status, data = self._request("GET", "/mbcortex/master/api/v1/central-registry", params=params)
        # DEBUG - descomente a linha abaixo para ver a resposta da API
        # print(f"  DEBUG list_units: status={status}, data={data}")
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            # A API pode retornar dados em 'data' ou em outras chaves como 'items'
            result_data = data.get("data", [])
            # Se não achou em 'data' ou está vazio, tenta outras chaves
            if not result_data:
                for key in ['items', 'records', 'results', 'list']:
                    if key in data and isinstance(data[key], list):
                        result_data = data[key]
                        break
            return {
                "data": result_data,
                "total": data.get("total", len(result_data)),
                "offset": data.get("offset", offset),
                "count": data.get("count", count)
            }
        raise MbcortexError(f"Falha ao listar cadastros: {data}")
    
    def get_unit_by_id(self, unit_id: int) -> Dict[str, Any]:
        """Busca cadastro central por ID.
        
        Args:
            unit_id: ID do cadastro central
            
        Returns:
            Dict com dados do cadastro ou None se nao encontrado
        """
        status, data = self._request("GET", "/mbcortex/master/api/v1/central-registry", params={"id": unit_id})
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            # API retorna dados diretamente (id, name, enabled, etc.)
            if "id" in data:
                return data
            return data.get("data") or data.get("item") or None
        elif status == 404:
            return None  # Nao encontrado
        raise MbcortexError(f"Falha ao buscar cadastro: {data}")
    
    def list_entities_by_unit(self, unit_id: int) -> List[Dict[str, Any]]:
        """Lista todas as entidades de um cadastro central.
        
        Args:
            unit_id: ID do cadastro central
            
        Returns:
            Lista de entidades (pode ser vazia)
        """
        status, data = self._request("GET", "/mbcortex/master/api/v1/entities", params={"cadastro_id": unit_id})
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            result_data = data.get("data", [])
            if not result_data:
                for key in ['items', 'records', 'results', 'list']:
                    if key in data and isinstance(data[key], list):
                        result_data = data[key]
                        break
            return result_data
        raise MbcortexError(f"Falha ao listar entidades: {data}")
    
    def search_entities_paginated(self, offset: int = 0, count: int = 20, 
                                  name_filter: str = "", doc_filter: str = "", 
                                  tipo_filter: int = None) -> Dict[str, Any]:
        """Busca entidades com filtros e paginacao.
        
        Args:
            offset: Registro inicial (0-based)
            count: Quantidade por pagina (max 100)
            name_filter: Filtro por nome (opcional)
            doc_filter: Filtro por documento/placa (opcional) 
            tipo_filter: Filtro por tipo (1=pessoa, 2=veiculo) (opcional)
            
        Returns:
            Dict com 'data' (lista de entidades), 'total', 'offset', 'count'
        """
        params = {"offset": offset, "count": min(count, 100)}
        
        if name_filter.strip():
            params["name"] = name_filter.strip()
        if doc_filter.strip():
            params["doc"] = doc_filter.strip()
        if tipo_filter is not None:
            params["tipo"] = tipo_filter
            
        status, data = self._request("GET", "/mbcortex/master/api/v1/entities", params=params)
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            result_data = data.get("data", [])
            if not result_data:
                for key in ['items', 'records', 'results', 'list']:
                    if key in data and isinstance(data[key], list):
                        result_data = data[key]
                        break
            return {
                "data": result_data,
                "total": data.get("total", len(result_data)),
                "offset": data.get("offset", offset),
                "count": data.get("count", count)
            }
        raise MbcortexError(f"Falha ao buscar entidades: {data}")
    
    def get_entity_by_id(self, entity_id: int) -> Dict[str, Any]:
        """Busca entidade por ID.
        
        Args:
            entity_id: ID da entidade
            
        Returns:
            Dict com dados da entidade ou None se nao encontrado
        """
        status, data = self._request("GET", "/mbcortex/master/api/v1/entities", params={"id": entity_id})
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            # API retorna dados diretamente ou em 'data'
            if "entity_id" in data or "id" in data:
                return data
            return data.get("data") or data.get("item") or None
        elif status == 404:
            return None  # Nao encontrado
        raise MbcortexError(f"Falha ao buscar entidade: {data}")
    
    def get_dashboard_stats(self) -> Dict[str, Any]:
        """Obtem estatisticas gerais do sistema.
        
        Returns:
            Dict com estatisticas do dashboard
        """
        status, data = self._request("GET", "/mbcortex/master/api/v1/dashboard")
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            return data.get("data", {})
        raise MbcortexError(f"Falha ao obter estatisticas: {data}")


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
            # Reconstroi base_url se nao existir
            ip = config.get("ip", "")
            port = config.get("port", 443)
            if config.get("base_url"):
                base_url = config["base_url"]
            elif ip:
                base_url = f"https://{ip}" if port == 443 else f"https://{ip}:{port}"
            else:
                base_url = ""
            
            # Salva apenas dados de conexao (sem dados do teste)
            dados_salvar = {
                "ip": ip,
                "port": port,
                "base_url": base_url,
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
        # Verifica se tem configuração salva
        config = self.load_config()
        status_conexao = "❌ Não configurado"
        if config:
            url = config.get('base_url', 'N/A')
            status_conexao = f"🔗 Configurado: {url}"
        
        self.header("MENU PRINCIPAL")
        print(f"\n  🔐 **CONFIGURAÇÃO**")
        print("  [C] Configurar Conexão (IP, porta, login, senha)")
        print("  [T] Testar Conectividade")
        
        print(f"\n  📋 **CADASTROS**")
        print("  [1] Listar Cadastros Centrais (com paginacao)")
        print("  [2] Novo Cadastro Central")
        print("  [3] Buscar Cadastro por ID")
        
        print(f"\n  👥 **ENTIDADES**")
        print("  [4] Nova Pessoa")
        print("  [5] Novo Veiculo")
        print("  [6] Listar Entidades por Cadastro")
        print("  [7] Busca Avancada de Entidades (filtros + paginacao)")
        
        print(f"\n  🧪 **TESTES RAPIDOS**")
        print("  [8] Teste Completo - Modo AUTO (IDs automaticos)")
        print("  [9] Teste Completo - Modo FIXED (IDs fixos)")
        
        print(f"\n  ℹ️  **INFORMACOES**")
        print("  [I] Sobre o Sistema")
        print("  [0] Sair")
        
        print(f"\n  Status: {status_conexao}")
        print("")
        
        while True:
            opcao = input("Escolha uma opcao: ").strip().upper()
            if opcao in ("0", "C", "T", "I") or opcao.isdigit() and 1 <= int(opcao) <= 9:
                return opcao
            self.error("Opcao invalida. Use C, T, I, 0-9.")
    
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
        config["vehicle_name"] = self.input_default("Nome do proprietario/veiculo", "Veiculo Teste")
        config["plate"] = self.input_default("Placa do veiculo", "ABC1234").upper()
        
        # Campos opcionais
        print(colorize("\n  ** Campos Opcionais (pressione ENTER para pular) **", Colors.BLUE))
        config["brand"] = input("  Marca: ").strip() or None
        config["model"] = input("  Modelo: ").strip() or None
        config["color"] = input("  Cor: ").strip() or None
        
        config["lpr_ativo"] = 1 if self.input_yes_no("\nAtivar LPR?", default=True) else 0
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
        config["vehicle_id"] = self.input_int("ID do Veiculo", default=2000001, min_val=1)
        
        self.header("Dados do Veiculo")
        config["vehicle_name"] = self.input_default("Nome do proprietario/veiculo", "Veiculo Teste")
        config["plate"] = self.input_default("Placa do veiculo", "ABC1234").upper()
        
        # Campos opcionais
        print(colorize("\n  ** Campos Opcionais (pressione ENTER para pular) **", Colors.BLUE))
        config["brand"] = input("  Marca: ").strip() or None
        config["model"] = input("  Modelo: ").strip() or None
        config["color"] = input("  Cor: ").strip() or None
        
        config["lpr_ativo"] = 1 if self.input_yes_no("\nAtivar LPR?", default=True) else 0
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
        
        unit_id = self.input_int("ID da Unidade (0=criar automatico com nome da placa)", default=0, min_val=0)
        vehicle_id = self.input_int("ID do Veiculo", default=0, min_val=0)
        
        # Define modo baseado nos IDs informados
        if unit_id == 0 and vehicle_id == 0:
            config["mode"] = "auto"
        else:
            config["mode"] = "fixed"
            if vehicle_id == 0:
                vehicle_id = 2000000
        
        config["unit_id"] = unit_id
        config["vehicle_id"] = vehicle_id
        # Flag para criar cadastro automatico com nome da placa
        config["auto_create_unit"] = (unit_id == 0)
        
        self.header("Dados do Veiculo")
        config["vehicle_name"] = self.input_default("Nome do proprietario/veiculo", "Veiculo Teste")
        config["plate"] = self.input_default("Placa do veiculo", "ABC1234").upper()
        
        # Campos opcionais
        print(colorize("\n  ** Campos Opcionais (pressione ENTER para pular) **", Colors.BLUE))
        config["brand"] = input("  Marca: ").strip() or None
        config["model"] = input("  Modelo: ").strip() or None  
        config["color"] = input("  Cor: ").strip() or None
        
        config["lpr_ativo"] = 1 if self.input_yes_no("\nAtivar LPR?", default=True) else 0
        config["cleanup_unit"] = self.input_yes_no("Apagar unidade no final?", default=False)
        
        return config
    
    def mostrar_resumo(self, config: Dict):
        """Mostra resumo da configuracao."""
        self.header("RESUMO DA CONFIGURACAO")
        print(f"\n  🌐 URL:      {config['base_url']}")
        print(f"  👤 Usuario:  {config['username']}")
        print(f"  🔑 Senha:    {config['password']} ")
        print(f"  ⏱️  Timeout:  {config['timeout']}s")
        print(f"  ⚙️  Modo:     {config['mode'].upper()}")
        
        if config["mode"] == "fixed":
            print(f"  🏢 Unidade:  {config['unit_id']}")
            print(f"  🚗 Veiculo:  {config['vehicle_id']}")
        
        print(f"\n  📋 **Dados do Veiculo**")
        print(f"  ├─ Nome:     {config.get('vehicle_name', 'Veiculo Teste')}")
        print(f"  ├─ Placa:    {config['plate']}")
        
        # Campos opcionais (só mostra se preenchidos)
        if config.get('brand'):
            print(f"  ├─ Marca:    {config['brand']}")
        if config.get('model'):
            print(f"  ├─ Modelo:   {config['model']}")
        if config.get('color'):
            print(f"  ├─ Cor:      {config['color']}")
        
        print(f"  ├─ LPR:      {'✅ Ativo' if config['lpr_ativo'] else '❌ Inativo'}")
        print(f"  └─ Cleanup:  {'✅ Sim' if config['cleanup_unit'] else '❌ Não'}")
    
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
                # Se auto_create_unit=True, cria cadastro com nome da placa
                unit_name = config.get("plate", "Demo Unidade") if config.get("auto_create_unit") else "Demo Unidade"
                
                if config["mode"] == "auto" or config.get("auto_create_unit"):
                    self.menu.step(2, f"Criar unidade (ID automatico) - {unit_name}")
                    unit_id = client.create_unit_auto(name=unit_name, enabled=1, slots1=2)
                    self.menu.success(f"Unidade criada: ID={unit_id}")
                else:
                    self.menu.step(2, f"Criar unidade (ID fixo: {config['unit_id']})")
                    unit_id = client.create_unit_fixed(
                        unit_id=config["unit_id"], name=unit_name, enabled=1, slots1=2
                    )
                    self.menu.success(f"Unidade criada: ID={unit_id}")
                
                # PASSO 3: Criar veiculo
                if config["mode"] == "auto":
                    self.menu.step(3, "Criar veiculo (ID automatico)")
                    
                    # Prepara campos opcionais
                    optional_fields = {}
                    if config.get("brand"):
                        optional_fields["brand"] = config["brand"]
                    if config.get("model"):
                        optional_fields["model"] = config["model"]
                    if config.get("color"):
                        optional_fields["color"] = config["color"]
                    
                    vehicle_id = client.create_vehicle_auto(
                        unit_id=unit_id, 
                        name=config.get("vehicle_name", "Demo Veiculo"),
                        plate=config["plate"], 
                        lpr_ativo=config["lpr_ativo"],
                        **optional_fields
                    )
                    self.menu.success(f"Veiculo criado: ID={vehicle_id}")
                else:
                    self.menu.step(3, f"Criar veiculo (ID fixo: {config['vehicle_id']})")
                    
                    # Prepara campos opcionais
                    optional_fields = {}
                    if config.get("brand"):
                        optional_fields["brand"] = config["brand"]
                    if config.get("model"):
                        optional_fields["model"] = config["model"]
                    if config.get("color"):
                        optional_fields["color"] = config["color"]
                    
                    vehicle_id = client.create_vehicle_fixed(
                        vehicle_id=config["vehicle_id"], 
                        unit_id=unit_id,
                        name=config.get("vehicle_name", "Demo Veiculo"), 
                        plate=config["plate"],
                        lpr_ativo=config["lpr_ativo"],
                        **optional_fields
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
def get_client_from_config(menu: Menu) -> Optional[MbcortexClient]:
    """Cria cliente a partir da configuração salva."""
    config = menu.load_config()
    if not config or not config.get("base_url"):
        menu.error("Nenhuma configuracao salva. Use [C] para configurar primeiro.")
        return None
    
    return MbcortexClient(
        base_url=config["base_url"],
        username=config.get("username", "master"),
        password=config["password"],
        timeout=config.get("timeout", 10)
    )


def listar_cadastros(menu: Menu, client: MbcortexClient):
    """Lista cadastros centrais com paginação."""
    menu.header("LISTAR CADASTROS CENTRAIS")
    
    offset = 0
    count = 20
    name_filter = ""
    
    while True:
        try:
            result = client.list_units_paginated(offset=offset, count=count, name_filter=name_filter)
            
            # DEBUG: mostra resposta bruta
            # print(f"  DEBUG: {result}")
            
            total = result.get("total", 0)
            data = result.get("data", [])
            
            # Se total é 0 mas tem data, usa len(data)
            if total == 0 and data:
                total = len(data)
            
            print(f"\n  Total: {total} | Mostrando: {offset+1}-{min(offset+count, total)} de {total}")
            if name_filter:
                print(f"  Filtro: '{name_filter}'")
            
            if not data:
                menu.info("Nenhum cadastro encontrado.")
            else:
                print(f"\n  {'ID':<10} {'Nome':<30} {'Status'}")
                print("  " + "-" * 55)
                for item in data:
                    status = "✓ Ativo" if item.get('enabled') else "✗ Inativo"
                    name = item.get('name', 'N/A')[:28]
                    print(f"  {item.get('id', 0):<10} {name:<30} {status}")
            
            print(f"\n  [N] Proxima pagina | [P] Pagina anterior | [F] Filtrar | [V] Voltar")
            acao = input("  Escolha: ").strip().upper()
            
            if acao == "N" and offset + count < total:
                offset += count
            elif acao == "P" and offset > 0:
                offset = max(0, offset - count)
            elif acao == "F":
                name_filter = input("  Filtrar por nome (vazio=todos): ").strip()
                offset = 0
            elif acao == "V":
                break
                
        except Exception as e:
            menu.error(f"Erro: {e}")
            break


def novo_cadastro_central(menu: Menu, client: MbcortexClient):
    """Cria novo cadastro central."""
    menu.header("NOVO CADASTRO CENTRAL")
    
    print("\n  (digite 0 para gerar ID automaticamente)\n")
    unit_id = menu.input_int("ID da Unidade", default=0, min_val=0)
    name = menu.input_required("Nome da Unidade")
    enabled = 1 if menu.input_yes_no("Ativar?", default=True) else 0
    slots1 = menu.input_int("Slots RFID", default=2, min_val=0)
    slots2 = menu.input_int("Slots Biometria", default=0, min_val=0)
    
    try:
        if unit_id == 0:
            new_id = client.create_unit_auto(name=name, enabled=enabled, slots1=slots1, slots2=slots2)
            menu.success(f"Unidade criada com ID automatico: {new_id}")
        else:
            new_id = client.create_unit_fixed(unit_id=unit_id, name=name, enabled=enabled, slots1=slots1, slots2=slots2)
            menu.success(f"Unidade criada com ID fixo: {new_id}")
    except ConflictError:
        menu.error(f"Ja existe uma unidade com ID {unit_id}")
    except Exception as e:
        menu.error(f"Erro ao criar unidade: {e}")


def buscar_cadastro_por_id(menu: Menu, client: MbcortexClient):
    """Busca cadastro central por ID."""
    menu.header("BUSCAR CADASTRO POR ID")
    
    unit_id = menu.input_int("ID da Unidade", min_val=1)
    
    try:
        result = client.get_unit_by_id(unit_id)
        if result:
            print(f"\n  {'='*50}")
            print(f"  ID:        {result.get('id')}")
            print(f"  Nome:      {result.get('name')}")
            print(f"  Status:    {'Ativo' if result.get('enabled') else 'Inativo'}")
            print(f"  Slots RFID: {result.get('slots1', 0)}")
            print(f"  Slots Bio:  {result.get('slots2', 0)}")
            print(f"  {'='*50}")
        else:
            menu.error(f"Unidade {unit_id} nao encontrada")
    except Exception as e:
        menu.error(f"Erro ao buscar: {e}")


def nova_pessoa(menu: Menu, client: MbcortexClient):
    """Cria nova pessoa/entidade."""
    menu.header("NOVA PESSOA")
    
    print("\n  (digite 0 para gerar ID automaticamente)\n")
    entity_id = menu.input_int("ID da Pessoa", default=0, min_val=0)
    print("\n  (digite 0 para criar cadastro automatico com nome da pessoa)")
    unit_id = menu.input_int("ID do Cadastro Central", default=0, min_val=0)
    name = menu.input_required("Nome")
    doc = menu.input_default("CPF/Documento", "")
    
    try:
        # Se unit_id for 0, cria cadastro automatico com nome da pessoa
        if unit_id == 0:
            menu.info(f"Criando cadastro central automatico com nome '{name}'...")
            unit_id = client.create_unit_auto(name=name, enabled=1, slots1=2, slots2=0)
            menu.success(f"Cadastro central criado com ID: {unit_id}")
        
        # Cria a pessoa (tipo 1 = pessoa)
        payload = {
            "tipo": 1,
            "name": name,
            "doc": doc,
            "cadastro_id": unit_id,
            "lpr_ativo": 0
        }
        
        if entity_id == 0:
            payload["createid"] = True
        else:
            payload["id"] = entity_id
        
        status, data = client._request("POST", "/mbcortex/master/api/v1/entities", json_data=payload)
        
        if status == 200 and isinstance(data, dict) and data.get("ret") == 0:
            new_id = data.get("entity_id", entity_id)
            menu.success(f"Pessoa criada com ID: {new_id}")
        else:
            raise MbcortexError(f"Falha ao criar pessoa: {data}")
            
    except ConflictError:
        menu.error("Ja existe uma pessoa com este ID ou documento")
    except Exception as e:
        menu.error(f"Erro ao criar pessoa: {e}")


def novo_veiculo(menu: Menu, client: MbcortexClient):
    """Cria novo veiculo."""
    menu.header("NOVO VEICULO")
    
    print("\n  (digite 0 para gerar ID automaticamente)\n")
    vehicle_id = menu.input_int("ID do Veiculo", default=0, min_val=0)
    print("\n  (digite 0 para criar cadastro automatico com nome da placa)")
    unit_id = menu.input_int("ID do Cadastro Central", default=0, min_val=0)
    name = menu.input_required("Nome do Proprietario")
    plate = menu.input_required("Placa").upper()
    lpr = 1 if menu.input_yes_no("Ativar LPR?", default=True) else 0
    
    brand = input("  Marca (opcional): ").strip() or None
    model = input("  Modelo (opcional): ").strip() or None
    color = input("  Cor (opcional): ").strip() or None
    
    try:
        optional = {}
        if brand: optional['brand'] = brand
        if model: optional['model'] = model
        if color: optional['color'] = color
        
        # Se unit_id for 0, cria cadastro automatico com nome da placa
        if unit_id == 0:
            menu.info(f"Criando cadastro central automatico com nome '{plate}'...")
            unit_id = client.create_unit_auto(name=plate, enabled=1, slots1=2, slots2=0)
            menu.success(f"Cadastro central criado com ID: {unit_id}")
        
        if vehicle_id == 0:
            new_id = client.create_vehicle_auto(unit_id=unit_id, name=name, plate=plate, 
                                                 lpr_ativo=lpr, **optional)
        else:
            new_id = client.create_vehicle_fixed(vehicle_id=vehicle_id, unit_id=unit_id, 
                                                  name=name, plate=plate, lpr_ativo=lpr, **optional)
        menu.success(f"Veiculo criado com ID: {new_id}")
    except ConflictError:
        menu.error("Ja existe um veiculo com este ID ou placa")
    except Exception as e:
        menu.error(f"Erro ao criar veiculo: {e}")


def listar_entidades_por_cadastro(menu: Menu, client: MbcortexClient):
    """Lista entidades de um cadastro central."""
    menu.header("LISTAR ENTIDADES POR CADASTRO")
    
    unit_id = menu.input_int("ID do Cadastro Central", min_val=1)
    
    try:
        entities = client.list_entities_by_unit(unit_id)
        
        if not entities:
            menu.info("Nenhuma entidade encontrada para este cadastro")
            return
        
        print(f"\n  {'ID':<10} {'Tipo':<10} {'Nome':<25} {'Documento/Placa'}")
        print("  " + "-" * 70)
        
        for e in entities:
            tipo = "Pessoa" if e.get('tipo') == 1 else "Veiculo" if e.get('tipo') == 2 else "?"
            name = e.get('name', 'N/A')[:23]
            doc = e.get('doc', 'N/A')[:20]
            print(f"  {e.get('id', 0):<10} {tipo:<10} {name:<25} {doc}")
        
        menu.info(f"Total: {len(entities)} entidade(s)")
        
    except Exception as e:
        menu.error(f"Erro ao listar: {e}")


def busca_avancada_entidades(menu: Menu, client: MbcortexClient):
    """Busca avancada de entidades com filtros."""
    menu.header("BUSCA AVANCADA DE ENTIDADES")
    
    offset = 0
    count = 20
    name_filter = ""
    doc_filter = ""
    
    while True:
        try:
            print(f"\n  Filtros atuais:")
            print(f"    Nome: '{name_filter or 'todos'}'")
            print(f"    Doc:  '{doc_filter or 'todos'}'")
            
            result = client.search_entities_paginated(offset=offset, count=count, 
                                                       name_filter=name_filter, doc_filter=doc_filter)
            total = result.get("total", 0)
            data = result.get("data", [])
            
            print(f"\n  Total: {total} | Mostrando: {offset+1}-{min(offset+count, total)} de {total}")
            
            if not data:
                menu.info("Nenhuma entidade encontrada")
            else:
                print(f"\n  {'ID':<10} {'Tipo':<10} {'Nome':<25} {'Documento/Placa'}")
                print("  " + "-" * 70)
                for e in data:
                    tipo = "Pessoa" if e.get('tipo') == 1 else "Veiculo" if e.get('tipo') == 2 else "?"
                    name = e.get('name', 'N/A')[:23]
                    doc = e.get('doc', 'N/A')[:20]
                    print(f"  {e.get('id', 0):<10} {tipo:<10} {name:<25} {doc}")
            
            print(f"\n  [N] Proxima | [P] Anterior | [F] Filtrar nome | [D] Filtrar doc | [V] Voltar")
            acao = input("  Escolha: ").strip().upper()
            
            if acao == "N" and offset + count < total:
                offset += count
            elif acao == "P" and offset > 0:
                offset = max(0, offset - count)
            elif acao == "F":
                name_filter = input("  Filtrar por nome: ").strip()
                offset = 0
            elif acao == "D":
                doc_filter = input("  Filtrar por documento/placa: ").strip()
                offset = 0
            elif acao == "V":
                break
                
        except Exception as e:
            menu.error(f"Erro: {e}")
            break


def teste_completo_auto(menu: Menu, runner: TestRunner):
    """Teste completo modo AUTO."""
    config = menu.configurar_teste_auto()
    menu.mostrar_resumo(config)
    print("")
    if menu.input_yes_no("Confirmar e executar?", default=True):
        runner.run(config)


def teste_completo_fixed(menu: Menu, runner: TestRunner):
    """Teste completo modo FIXED."""
    config = menu.configurar_teste_fixed()
    menu.mostrar_resumo(config)
    print("")
    if menu.input_yes_no("Confirmar e executar?", default=True):
        runner.run(config)


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
            
            # === CADASTROS ===
            elif opcao == "1":
                client = get_client_from_config(menu)
                if client:
                    try:
                        client.login()
                        listar_cadastros(menu, client)
                    except Exception as e:
                        menu.error(f"Erro: {e}")
                    input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "2":
                client = get_client_from_config(menu)
                if client:
                    try:
                        client.login()
                        novo_cadastro_central(menu, client)
                    except Exception as e:
                        menu.error(f"Erro: {e}")
                    input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "3":
                client = get_client_from_config(menu)
                if client:
                    try:
                        client.login()
                        buscar_cadastro_por_id(menu, client)
                    except Exception as e:
                        menu.error(f"Erro: {e}")
                    input("\nPressione ENTER para voltar ao menu...")
            
            # === ENTIDADES ===
            elif opcao == "4":
                client = get_client_from_config(menu)
                if client:
                    try:
                        client.login()
                        nova_pessoa(menu, client)
                    except Exception as e:
                        menu.error(f"Erro: {e}")
                    input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "5":
                client = get_client_from_config(menu)
                if client:
                    try:
                        client.login()
                        novo_veiculo(menu, client)
                    except Exception as e:
                        menu.error(f"Erro: {e}")
                    input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "6":
                client = get_client_from_config(menu)
                if client:
                    try:
                        client.login()
                        listar_entidades_por_cadastro(menu, client)
                    except Exception as e:
                        menu.error(f"Erro: {e}")
                    input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "7":
                client = get_client_from_config(menu)
                if client:
                    try:
                        client.login()
                        busca_avancada_entidades(menu, client)
                    except Exception as e:
                        menu.error(f"Erro: {e}")
                    input("\nPressione ENTER para voltar ao menu...")
            
            # === TESTES RAPIDOS ===
            elif opcao == "8":
                teste_completo_auto(menu, runner)
                input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "9":
                teste_completo_fixed(menu, runner)
                input("\nPressione ENTER para voltar ao menu...")
            
            # === CONFIGURACAO ===
            elif opcao == "C":
                config = menu.configurar_conexao()
                menu.save_config(config)
                menu.success("Configuracao salva com sucesso!")
                input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "T":
                menu.header("TESTAR CONECTIVIDADE")
                client = get_client_from_config(menu)
                if not client:
                    input("\nPressione ENTER para voltar ao menu...")
                    continue
                
                config = menu.load_config()
                print(f"\nTestando conexao com: {config['base_url']}")
                try:
                    session = client.login()
                    menu.success(f"Conexao OK! Login bem-sucedido.")
                    menu.info(f"Session: {session[:16]}...")
                except AuthError as e:
                    menu.error(f"Falha de autenticacao: {e}")
                except Exception as e:
                    menu.error(f"Erro de conexao: {e}")
                
                input("\nPressione ENTER para voltar ao menu...")
            
            elif opcao == "I":
                menu.sobre()
    
    except KeyboardInterrupt:
        print(colorize("\n\nOperacao cancelada pelo usuario.", Colors.WARNING))
        sys.exit(0)


if __name__ == "__main__":
    main()
