"""
Exceções customizadas para a API MobiCortex Master
==================================================

Define todas as exceções específicas da API MobiCortex para melhor
tratamento de erros e diagnóstico de problemas.

AUTOR:
    MobiCortex
"""

from typing import Any


class MbcortexError(Exception):
    """
    Exceção base para erros da API MobiCortex.
    
    Contém informações detalhadas sobre o erro para facilitar 
    o diagnóstico e tratamento.
    
    Attributes:
        status_code: Código de status HTTP (se aplicável)
        method: Método HTTP usado na requisição
        url: URL que causou o erro
        response_data: Dados da resposta do servidor
    """
    
    def __init__(self, message: str, status_code: int = 0, method: str = "", 
                 url: str = "", response_data: Any = None):
        super().__init__(message)
        self.status_code = status_code
        self.method = method
        self.url = url
        self.response_data = response_data


class AuthError(MbcortexError):
    """
    Erro de autenticação (login/senha inválidos ou token expirado).
    
    Geralmente indica que é necessário:
    - Verificar credenciais (usuário/senha)
    - Fazer novo login (token expirado)
    """
    pass


class NotFoundError(MbcortexError):
    """
    Recurso não encontrado (HTTP 404).
    
    Indica que o ID especificado não existe na controladora.
    """
    pass


class ConflictError(MbcortexError):
    """
    Conflito de dados (HTTP 409).
    
    Geralmente indica:
    - ID já existe (em modo fixed)
    - Placa duplicada
    - Violação de restrição única
    """
    pass


class ValidationError(MbcortexError):
    """
    Dados inválidos enviados para API (HTTP 400).
    
    Indica problema nos dados enviados:
    - Campos obrigatórios faltando
    - Formato de placa inválido
    - Valores fora do range permitido
    """
    pass


class ServerError(MbcortexError):
    """
    Erro interno do servidor (HTTP 5xx).
    
    Indica problema na controladora ou infraestrutura.
    """
    pass