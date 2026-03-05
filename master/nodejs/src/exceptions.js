/**
 * Exceções customizadas para API MobiCortex Master
 * ================================================
 * 
 * Define todas as exceções específicas da API MobiCortex para melhor
 * tratamento de erros e diagnóstico de problemas.
 * 
 * AUTOR: MobiCortex
 */

/**
 * Exceção base para erros da API MobiCortex.
 * 
 * Contém informações detalhadas sobre o erro para facilitar 
 * o diagnóstico e tratamento.
 */
class MbcortexError extends Error {
  /**
   * @param {string} message - Mensagem do erro
   * @param {number} statusCode - Código de status HTTP (se aplicável)
   * @param {string} method - Método HTTP usado na requisição
   * @param {string} url - URL que causou o erro
   * @param {any} responseData - Dados da resposta do servidor
   */
  constructor(message, statusCode = 0, method = '', url = '', responseData = null) {
    super(message);
    this.name = 'MbcortexError';
    this.statusCode = statusCode;
    this.method = method;
    this.url = url;
    this.responseData = responseData;
  }
}

/**
 * Erro de autenticação (login/senha inválidos ou token expirado).
 * 
 * Geralmente indica que é necessário:
 * - Verificar credenciais (usuário/senha)
 * - Fazer novo login (token expirado)
 */
class AuthError extends MbcortexError {
  constructor(message, statusCode = 401, method = '', url = '', responseData = null) {
    super(message, statusCode, method, url, responseData);
    this.name = 'AuthError';
  }
}

/**
 * Recurso não encontrado (HTTP 404).
 * 
 * Indica que o ID especificado não existe na controladora.
 */
class NotFoundError extends MbcortexError {
  constructor(message, statusCode = 404, method = '', url = '', responseData = null) {
    super(message, statusCode, method, url, responseData);
    this.name = 'NotFoundError';
  }
}

/**
 * Conflito de dados (HTTP 409).
 * 
 * Geralmente indica:
 * - ID já existe (em modo fixed)
 * - Placa duplicada
 * - Violação de restrição única
 */
class ConflictError extends MbcortexError {
  constructor(message, statusCode = 409, method = '', url = '', responseData = null) {
    super(message, statusCode, method, url, responseData);
    this.name = 'ConflictError';
  }
}

/**
 * Dados inválidos enviados para API (HTTP 400).
 * 
 * Indica problema nos dados enviados:
 * - Campos obrigatórios faltando
 * - Formato de placa inválido
 * - Valores fora do range permitido
 */
class ValidationError extends MbcortexError {
  constructor(message, statusCode = 400, method = '', url = '', responseData = null) {
    super(message, statusCode, method, url, responseData);
    this.name = 'ValidationError';
  }
}

/**
 * Erro interno do servidor (HTTP 5xx).
 * 
 * Indica problema na controladora ou infraestrutura.
 */
class ServerError extends MbcortexError {
  constructor(message, statusCode = 500, method = '', url = '', responseData = null) {
    super(message, statusCode, method, url, responseData);
    this.name = 'ServerError';
  }
}

module.exports = {
  MbcortexError,
  AuthError,
  NotFoundError,
  ConflictError,
  ValidationError,
  ServerError
};