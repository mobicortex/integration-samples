"""
Modelos de dados para API MobiCortex Master
============================================

Define estruturas de dados e classes auxiliares para facilitar
o trabalho com a API MobiCortex Master.

AUTOR:
    MobiCortex
"""

from typing import Optional, Dict, Any
from datetime import datetime


class CentralRegistry:
    """
    Representa um cadastro central (unidade/empresa).
    
    Um cadastro central é uma entidade organizacional que pode
    conter múltiplas entidades (pessoas e veículos).
    """
    
    def __init__(self, id_: int = 0, name: str = "", enabled: int = 1, 
                 slots1: int = 0, slots2: int = 0, type_: int = 0):
        self.id = id_
        self.name = name
        self.enabled = enabled  # 1 = ativo, 0 = inativo
        self.slots1 = slots1   # Slots de parqueamento tipo 1
        self.slots2 = slots2   # Slots de parqueamento tipo 2  
        self.type = type_      # Tipo de cadastro (0 = padrão)
    
    def to_dict(self) -> Dict[str, Any]:
        """Converte para dict compatível com API."""
        return {
            "id": self.id,
            "name": self.name,
            "enabled": self.enabled,
            "slots1": self.slots1,
            "slots2": self.slots2,
            "type": self.type
        }
    
    @classmethod
    def from_dict(cls, data: Dict[str, Any]) -> 'CentralRegistry':
        """Cria instância a partir de dict da API."""
        return cls(
            id_=data.get("id", 0),
            name=data.get("name", ""),
            enabled=data.get("enabled", 1),
            slots1=data.get("slots1", 0),
            slots2=data.get("slots2", 0),
            type_=data.get("type", 0)
        )


class Entity:
    """
    Representa uma entidade (pessoa ou veículo).
    
    Base class para Person e Vehicle.
    """
    
    def __init__(self, id_: int = 0, name: str = "", doc: str = "", 
                 cadastro_id: int = 0, tipo: int = 1):
        self.id = id_
        self.name = name
        self.doc = doc  # Documento (CPF para pessoa, placa para veículo)
        self.cadastro_id = cadastro_id  # ID do cadastro central
        self.tipo = tipo  # 1 = pessoa, 2 = veículo
    
    def to_dict(self) -> Dict[str, Any]:
        """Converte para dict compatível com API."""
        return {
            "id": self.id,
            "name": self.name,
            "doc": self.doc,
            "cadastro_id": self.cadastro_id,
            "tipo": self.tipo
        }


class Person(Entity):
    """
    Representa uma pessoa no sistema.
    
    Herda de Entity e adiciona funcionalidades específicas para pessoas.
    """
    
    def __init__(self, id_: int = 0, name: str = "", cpf: str = "", 
                 cadastro_id: int = 0, **kwargs):
        # Pessoa sempre tem tipo = 1
        super().__init__(id_, name, cpf, cadastro_id, tipo=1)
        
        # Campos adicionais opcionais
        self.obs = kwargs.get("obs", "")
        self.email = kwargs.get("email", "")
        self.telefone = kwargs.get("telefone", "")
    
    @property
    def cpf(self) -> str:
        """Alias para doc (mais semântico para pessoa)."""
        return self.doc
    
    @cpf.setter  
    def cpf(self, value: str):
        """Setter para CPF."""
        self.doc = value
    
    def to_dict(self) -> Dict[str, Any]:
        """Converte para dict compatível com API."""
        base = super().to_dict()
        
        # Adiciona campos opcionais se preenchidos
        if self.obs:
            base["obs"] = self.obs
        if self.email:
            base["email"] = self.email
        if self.telefone:
            base["telefone"] = self.telefone
            
        return base


class Vehicle(Entity):
    """
    Representa um veículo no sistema.
    
    Herda de Entity e adiciona funcionalidades específicas para veículos.
    """
    
    def __init__(self, id_: int = 0, name: str = "", plate: str = "", 
                 cadastro_id: int = 0, lpr_ativo: int = 1, **kwargs):
        # Veículo sempre tem tipo = 2
        super().__init__(id_, name, self._normalize_plate(plate), cadastro_id, tipo=2)
        
        # LPR (License Plate Recognition)
        self.lpr_ativo = lpr_ativo  # 1 = ativo, 0 = inativo
        
        # Campos opcionais específicos do veículo
        self.brand = kwargs.get("brand", "")    # Marca
        self.model = kwargs.get("model", "")    # Modelo
        self.color = kwargs.get("color", "")    # Cor
        self.obs = kwargs.get("obs", "")        # Observações
    
    @property
    def plate(self) -> str:
        """Alias para doc (mais semântico para veículo)."""
        return self.doc
    
    @plate.setter
    def plate(self, value: str):
        """Setter para placa (com normalização automática)."""
        self.doc = self._normalize_plate(value)
    
    @staticmethod
    def _normalize_plate(plate: str) -> str:
        """
        Normaliza placa removendo espaços e hífens, convertendo para maiúsculo.
        
        Args:
            plate: Placa original (ex: "ABC-1234", "abc 1234")
            
        Returns:
            Placa normalizada (ex: "ABC1234")
        """
        return plate.upper().replace(" ", "").replace("-", "")
    
    def to_dict(self) -> Dict[str, Any]:
        """Converte para dict compatível com API."""
        base = super().to_dict()
        
        # Adiciona LPR
        base["lpr_ativo"] = self.lpr_ativo
        
        # Adiciona campos opcionais se preenchidos
        optional_fields = ["brand", "model", "color", "obs"]
        for field in optional_fields:
            value = getattr(self, field, "")
            if value:
                base[field] = value
        
        return base
    
    def validate_plate(self) -> bool:
        """
        Valida formato básico da placa.
        
        Returns:
            True se placa tem formato válido (3 letras + 4 números)
        """
        import re
        # Formato brasileiro padrão: ABC1234
        pattern = r'^[A-Z]{3}\d{4}$'
        return bool(re.match(pattern, self.plate))


class PaginatedResponse:
    """
    Representa uma resposta paginada da API.
    
    Facilita o trabalho com listagens que suportam paginação.
    """
    
    def __init__(self, data: list, total: int, offset: int, count: int):
        self.data = data      # Lista de itens da página atual
        self.total = total    # Total de itens disponíveis
        self.offset = offset  # Posição inicial desta página 
        self.count = count    # Quantidade de itens por página
    
    @property
    def has_next(self) -> bool:
        """Verifica se há próxima página."""
        return (self.offset + self.count) < self.total
    
    @property
    def has_previous(self) -> bool:
        """Verifica se há página anterior."""
        return self.offset > 0
    
    @property
    def next_offset(self) -> Optional[int]:
        """Retorna offset para próxima página."""
        if self.has_next:
            return self.offset + self.count
        return None
    
    @property
    def previous_offset(self) -> Optional[int]:
        """Retorna offset para página anterior.""" 
        if self.has_previous:
            return max(0, self.offset - self.count)
        return None
    
    @property
    def current_page(self) -> int:
        """Número da página atual (1-based)."""
        return (self.offset // self.count) + 1
    
    @property
    def total_pages(self) -> int:
        """Total de páginas disponíveis."""
        return ((self.total - 1) // self.count) + 1 if self.total > 0 else 0