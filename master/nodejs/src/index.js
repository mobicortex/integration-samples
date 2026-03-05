/**
 * MobiCortex Master - Biblioteca de Integração (Node.js)
 * ======================================================
 * 
 * Exports principais para uso como biblioteca reutilizável.
 * 
 * AUTOR: MobiCortex
 */

const MbcortexClient = require('./mbcortexClient');
const { 
  MbcortexError,
  AuthError,
  ValidationError,
  NotFoundError,
  ConflictError,
  ServerError
} = require('./exceptions');
const {
  CentralRegistry,
  Entity,
  Person,
  Vehicle,
  PaginatedResponse
} = require('./models');

module.exports = {
  // Cliente principal
  MbcortexClient,
  
  // Exceções
  MbcortexError,
  AuthError,
  ValidationError,
  NotFoundError,
  ConflictError,
  ServerError,
  
  // Modelos
  CentralRegistry,
  Entity,
  Person,
  Vehicle,
  PaginatedResponse,
  
  // Informações da biblioteca
  version: '1.0.0',
  author: 'MobiCortex'
};