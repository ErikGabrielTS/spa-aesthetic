CREATE DATABASE `db_aula_backend` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

CREATE TABLE `atendimento` (
  `id_atendimento` bigint unsigned NOT NULL AUTO_INCREMENT,
  `id_pessoa` bigint unsigned NOT NULL,
  `data_atendimento` datetime NOT NULL,
  `status_atendimento` char(1) NOT NULL,
  PRIMARY KEY (`id_atendimento`),
  KEY `atendimento_pessoa_FK` (`id_pessoa`),
  CONSTRAINT `atendimento_pessoa_FK` FOREIGN KEY (`id_pessoa`) REFERENCES `pessoa` (`id_pessoa`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `atendimento_item` (
  `id_atendimento` bigint unsigned NOT NULL,
  `id_item` bigint unsigned NOT NULL,
  `id_funcionario` bigint unsigned NOT NULL,
  `quantidade_itens` int unsigned NOT NULL,
  `valor_unitario_entrada` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id_atendimento`,`id_item`),
  KEY `atendimento_item_item_FK` (`id_item`),
  KEY `atendimento_item_pessoa_FK` (`id_funcionario`),
  CONSTRAINT `atendimento_item_atendimento_FK` FOREIGN KEY (`id_atendimento`) REFERENCES `atendimento` (`id_atendimento`),
  CONSTRAINT `atendimento_item_item_FK` FOREIGN KEY (`id_item`) REFERENCES `item` (`id_item`),
  CONSTRAINT `atendimento_item_pessoa_FK` FOREIGN KEY (`id_funcionario`) REFERENCES `pessoa` (`id_pessoa`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `categoria` (
  `id_categoria` bigint unsigned NOT NULL AUTO_INCREMENT,
  `descricao` varchar(35) NOT NULL,
  PRIMARY KEY (`id_categoria`),
  UNIQUE KEY `descricao_unique` (`descricao`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `endereco` (
  `id_endereco` bigint unsigned NOT NULL AUTO_INCREMENT,
  `id_pessoa` bigint unsigned NOT NULL,
  `cep` char(8) NOT NULL,
  `numero` varchar(10) DEFAULT NULL,
  `complemento` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`id_endereco`),
  KEY `endereco_pessoa_FK` (`id_pessoa`),
  CONSTRAINT `endereco_pessoa_FK` FOREIGN KEY (`id_pessoa`) REFERENCES `pessoa` (`id_pessoa`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `entrada_produto` (
  `id_entrada_produto` bigint unsigned NOT NULL AUTO_INCREMENT,
  `id_item` bigint unsigned NOT NULL,
  `data_entrada_produto` datetime NOT NULL,
  `quantidade_entrada` decimal(10,2) NOT NULL,
  `unidade` varchar(10) NOT NULL,
  `valor_unitario_entrada` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id_entrada_produto`,`id_item`),
  KEY `entrada_produto_estoque_FK` (`id_item`),
  CONSTRAINT `entrada_produto_estoque_FK` FOREIGN KEY (`id_item`) REFERENCES `estoque` (`id_item`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `estoque` (
  `id_item` bigint unsigned NOT NULL,
  `quantidade_estoque` int unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id_item`),
  CONSTRAINT `estoque_produto_FK` FOREIGN KEY (`id_item`) REFERENCES `produto` (`id_item`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `funcionario` (
  `id_pessoa` bigint unsigned NOT NULL,
  `cargo` varchar(20) NOT NULL,
  `data_admissao` date NOT NULL,
  PRIMARY KEY (`id_pessoa`),
  CONSTRAINT `funcionario_pessoa_FK` FOREIGN KEY (`id_pessoa`) REFERENCES `pessoa` (`id_pessoa`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `item` (
  `id_item` bigint unsigned NOT NULL AUTO_INCREMENT,
  `id_categoria` bigint unsigned NOT NULL,
  `item` varchar(40) NOT NULL,
  `descricao_item` varchar(200) DEFAULT NULL,
  `preco` decimal(10,2) NOT NULL,
  `tipo_item` char(1) NOT NULL,
  `ativo_produto_servico` char(1) NOT NULL,
  PRIMARY KEY (`id_item`),
  UNIQUE KEY `item_unique` (`item`),
  KEY `item_categoria_FK` (`id_categoria`),
  CONSTRAINT `item_categoria_FK` FOREIGN KEY (`id_categoria`) REFERENCES `categoria` (`id_categoria`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `pessoa` (
  `id_pessoa` bigint unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) NOT NULL,
  `cpf` char(11) NOT NULL,
  `data_nascimento` date NOT NULL,
  `sexo` char(1) NOT NULL,
  `email` varchar(100) NOT NULL,
  `telefone` char(11) NOT NULL,
  `ativo_pessoa` char(1) NOT NULL,
  PRIMARY KEY (`id_pessoa`),
  UNIQUE KEY `pessoa_unique_cpf` (`cpf`),
  UNIQUE KEY `pessoa_unique_email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `produto` (
  `id_item` bigint unsigned NOT NULL,
  `codigo_preco` int unsigned DEFAULT NULL,
  `preco_venda` decimal(10,2) NOT NULL,
  `unidade` varchar(10) NOT NULL,
  PRIMARY KEY (`id_item`),
  CONSTRAINT `produto_item_FK` FOREIGN KEY (`id_item`) REFERENCES `item` (`id_item`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;