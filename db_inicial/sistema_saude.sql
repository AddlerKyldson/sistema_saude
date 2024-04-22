-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 22/04/2024 às 16:14
-- Versão do servidor: 10.4.32-MariaDB
-- Versão do PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `sistema_saude`
--

-- --------------------------------------------------------

--
-- Estrutura para tabela `bairro`
--

CREATE TABLE `bairro` (
  `id` int(11) NOT NULL,
  `nome` varchar(250) NOT NULL,
  `id_cidade` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NULL DEFAULT NULL,
  `data_alteracao` timestamp NULL DEFAULT NULL,
  `slug` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `cidade`
--

CREATE TABLE `cidade` (
  `id` int(11) NOT NULL,
  `nome` varchar(250) NOT NULL,
  `id_estado` int(11) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NULL DEFAULT NULL,
  `data_alteracao` timestamp NULL DEFAULT NULL,
  `slug` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `estabelecimento`
--

CREATE TABLE `estabelecimento` (
  `id` int(11) NOT NULL,
  `razao_social` varchar(250) NOT NULL,
  `nome_fantasia` varchar(250) NOT NULL,
  `cnpj` varchar(30) NOT NULL,
  `cnae` varchar(250) NOT NULL,
  `data_inicio_funcionamento` varchar(20) NOT NULL,
  `grau_risco` int(11) NOT NULL,
  `inscricao_municipal` varchar(100) NOT NULL,
  `inscricao_estadual` varchar(100) NOT NULL,
  `logradouro` varchar(250) NOT NULL,
  `id_bairro` int(11) NOT NULL,
  `cep` varchar(10) NOT NULL,
  `complemento` varchar(250) NOT NULL,
  `telefone` varchar(250) NOT NULL,
  `email` varchar(250) NOT NULL,
  `protocolo_funcionamento` varchar(250) NOT NULL,
  `passivo_alvara_sanitario` int(11) NOT NULL,
  `n_alvara_sanitario` varchar(250) NOT NULL,
  `coleta_residuos` int(11) NOT NULL,
  `autuacao_visa` int(11) NOT NULL,
  `forma_abastecimento` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `data_alteracao` timestamp NULL DEFAULT NULL,
  `slug` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `estado`
--

CREATE TABLE `estado` (
  `id` int(11) NOT NULL,
  `nome` varchar(250) NOT NULL,
  `sigla` varchar(5) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NULL DEFAULT NULL,
  `data_alteracao` timestamp NULL DEFAULT NULL,
  `slug` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `inspecao`
--

CREATE TABLE `inspecao` (
  `id` int(11) NOT NULL,
  `descricao` varchar(250) NOT NULL,
  `n_termo_inspecao` varchar(250) NOT NULL,
  `data_inspecao` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `motivo_inspecao` int(11) NOT NULL,
  `id_estabelecimento` int(11) NOT NULL,
  `id_responsavel_tecnico` int(11) NOT NULL,
  `procedencia_materiais` int(11) NOT NULL,
  `verificacao_produtos` int(11) NOT NULL,
  `verificacao_uniformes` int(11) NOT NULL,
  `verificacao_transporte_carnes` int(11) NOT NULL,
  `verificacao_transportes` int(11) NOT NULL,
  `verificacao_recebimento_frios` int(11) NOT NULL,
  `verificacao_armazenamento` int(11) NOT NULL,
  `verificacao_embalagens_industrializados` int(11) NOT NULL,
  `verificacao_armazenamento_toxicos` int(11) NOT NULL,
  `verificacao_temperatura_pereciveis` int(11) NOT NULL,
  `verificacao_local_geladeira` int(11) NOT NULL,
  `verificacao_especura_gelo` int(11) NOT NULL,
  `verificacao_organizacao_geladeira` int(11) NOT NULL,
  `verificacao_regulacao_freezer` int(11) NOT NULL,
  `verificacao_estado_camara_fria` int(11) NOT NULL,
  `verificacao_porta_camara_fria` int(11) NOT NULL,
  `verificacao_termometro_camara_fria` int(11) NOT NULL,
  `verificacao_carnes_camara_fria` int(11) NOT NULL,
  `verificacao_hortifruti_carmara_fria` int(11) NOT NULL,
  `verificacao_estrado_camara_fria` int(11) NOT NULL,
  `verificacao_higienizacao_camara_fria` int(11) NOT NULL,
  `verificacao_qualidade_temperatura` int(11) NOT NULL,
  `verificacao_prazo_validade` int(11) NOT NULL,
  `verificacao_devolucao_produtos` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NULL DEFAULT NULL,
  `data_alteracao` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `medicamento`
--

CREATE TABLE `medicamento` (
  `id` int(11) NOT NULL,
  `nome` varchar(250) NOT NULL,
  `apelido` varchar(250) NOT NULL,
  `codigo_barras` varchar(250) NOT NULL,
  `estoque` int(11) NOT NULL,
  `status` int(1) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `data_alteracao` timestamp NULL DEFAULT NULL,
  `slug` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `movimentacao_medicamento`
--

CREATE TABLE `movimentacao_medicamento` (
  `id` int(11) NOT NULL,
  `descricao` varchar(250) NOT NULL,
  `data` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `tipo` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NULL DEFAULT NULL,
  `data_alteracao` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `movimentacao_medicamento_item`
--

CREATE TABLE `movimentacao_medicamento_item` (
  `id` int(11) NOT NULL,
  `id_medicamento` int(11) NOT NULL,
  `id_movimentacao` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `data_alteracao` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `regional`
--

CREATE TABLE `regional` (
  `id` int(11) NOT NULL,
  `nome` varchar(250) NOT NULL,
  `id_estado` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `data` timestamp NULL DEFAULT NULL,
  `slug` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `serie`
--

CREATE TABLE `serie` (
  `id` int(11) NOT NULL,
  `nome` varchar(250) NOT NULL,
  `status` int(11) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `data_alteracao` timestamp NULL DEFAULT NULL,
  `slug` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `tipo_estabelecimento`
--

CREATE TABLE `tipo_estabelecimento` (
  `id` int(11) NOT NULL,
  `nome` varchar(250) NOT NULL,
  `id_serie` int(11) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `data_alteracao` timestamp NULL DEFAULT NULL,
  `slug` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `unidade_saude`
--

CREATE TABLE `unidade_saude` (
  `id` int(11) NOT NULL,
  `nome` varchar(250) NOT NULL,
  `cnes` varchar(20) NOT NULL,
  `logradouro` varchar(250) NOT NULL,
  `id_bairro` int(11) NOT NULL,
  `cep` varchar(10) NOT NULL,
  `numero` varchar(20) NOT NULL,
  `complemento` varchar(250) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `data_alteracao` timestamp NULL DEFAULT NULL,
  `slug` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `nome` varchar(250) NOT NULL,
  `data_nascimento` varchar(20) NOT NULL,
  `email` varchar(250) NOT NULL,
  `senha` varchar(250) NOT NULL,
  `telefone` varchar(250) NOT NULL,
  `cpf` varchar(20) NOT NULL,
  `cns` varchar(20) NOT NULL,
  `logradouro` varchar(250) NOT NULL,
  `numero` varchar(20) NOT NULL,
  `complemento` varchar(250) NOT NULL,
  `bairro` int(11) NOT NULL,
  `cep` varchar(20) NOT NULL,
  `status` int(1) NOT NULL,
  `id_usuario_cadastro` int(11) NOT NULL,
  `id_usuario_alteracao` int(11) DEFAULT NULL,
  `data_cadastro` timestamp NULL DEFAULT NULL,
  `data_alteracao` timestamp NULL DEFAULT NULL,
  `slug` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Índices para tabelas despejadas
--

--
-- Índices de tabela `bairro`
--
ALTER TABLE `bairro`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `cidade`
--
ALTER TABLE `cidade`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `estabelecimento`
--
ALTER TABLE `estabelecimento`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `estado`
--
ALTER TABLE `estado`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `inspecao`
--
ALTER TABLE `inspecao`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `medicamento`
--
ALTER TABLE `medicamento`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `movimentacao_medicamento`
--
ALTER TABLE `movimentacao_medicamento`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `movimentacao_medicamento_item`
--
ALTER TABLE `movimentacao_medicamento_item`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `regional`
--
ALTER TABLE `regional`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `serie`
--
ALTER TABLE `serie`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `tipo_estabelecimento`
--
ALTER TABLE `tipo_estabelecimento`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `unidade_saude`
--
ALTER TABLE `unidade_saude`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT para tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `bairro`
--
ALTER TABLE `bairro`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `cidade`
--
ALTER TABLE `cidade`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `estabelecimento`
--
ALTER TABLE `estabelecimento`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `estado`
--
ALTER TABLE `estado`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `inspecao`
--
ALTER TABLE `inspecao`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `medicamento`
--
ALTER TABLE `medicamento`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `movimentacao_medicamento`
--
ALTER TABLE `movimentacao_medicamento`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `movimentacao_medicamento_item`
--
ALTER TABLE `movimentacao_medicamento_item`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `regional`
--
ALTER TABLE `regional`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `serie`
--
ALTER TABLE `serie`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `tipo_estabelecimento`
--
ALTER TABLE `tipo_estabelecimento`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `unidade_saude`
--
ALTER TABLE `unidade_saude`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
