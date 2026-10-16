DROP PROCEDURE IF EXISTS `sp_cadastrar_produto`;

DELIMITER $$

CREATE PROCEDURE `sp_cadastrar_produto`(
  IN p_item                  VARCHAR(40),
  IN p_id_categoria          BIGINT UNSIGNED,
  IN p_descricao_item        VARCHAR(200),
  IN p_preco                 DECIMAL(10,2),
  IN p_ativo_produto_servico CHAR(1),
  IN p_codigo_preco          INT UNSIGNED,
  IN p_preco_venda           DECIMAL(10,2),
  IN p_unidade               VARCHAR(10)
)
BEGIN
  DECLARE v_id_item        BIGINT UNSIGNED DEFAULT NULL;
  DECLARE v_total_item     INT DEFAULT 0;
  DECLARE v_total_categoria INT DEFAULT 0;

  DECLARE EXIT HANDLER FOR SQLEXCEPTION
  BEGIN
    ROLLBACK;
    RESIGNAL;
  END;

  START TRANSACTION;

  IF p_item IS NULL
     OR TRIM(p_item) = ''
     OR CHAR_LENGTH(p_item) > 40 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe o nome do produto (1 a 40 caracteres).';
  END IF;

  IF p_id_categoria IS NULL THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe o id da categoria do produto.';
  END IF;

  IF CHAR_LENGTH(p_descricao_item) > 200 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'A descricao do produto deve ter no maximo 200 caracteres.';
  END IF;

  IF p_preco IS NULL OR p_preco < 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe um preco valido para o item (>= 0).';
  END IF;

  IF p_preco_venda IS NULL OR p_preco_venda < 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe um preco de venda valido (>= 0).';
  END IF;

  IF p_ativo_produto_servico IS NULL OR p_ativo_produto_servico NOT IN ('S', 'N') THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'ativo_produto_servico invalido: use S ou N.';
  END IF;

  IF p_unidade IS NULL
     OR TRIM(p_unidade) = ''
     OR CHAR_LENGTH(p_unidade) > 10 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe uma unidade valida (1 a 10 caracteres).';
  END IF;

  SELECT COUNT(*) INTO v_total_categoria
  FROM categoria
  WHERE id_categoria = p_id_categoria;

  IF v_total_categoria = 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Categoria inexistente: cadastre a categoria antes do produto.';
  END IF;

  SELECT COUNT(*) INTO v_total_item
  FROM item
  WHERE item = p_item;

  IF v_total_item > 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Ja existe um item cadastrado com este nome.';
  END IF;

  INSERT INTO item
    (id_categoria, item, descricao_item, preco, tipo_item, ativo_produto_servico)
  VALUES
    (p_id_categoria, p_item, NULLIF(TRIM(p_descricao_item), ''), p_preco, 'P', p_ativo_produto_servico);

  SET v_id_item = LAST_INSERT_ID();

  INSERT INTO produto
    (id_item, codigo_preco, preco_venda, unidade)
  VALUES
    (v_id_item, p_codigo_preco, p_preco_venda, p_unidade);

  COMMIT;
END$$

DELIMITER ;

-- ########## TESTES ##########

CALL sp_cadastrar_produto('Shampoo 400ml', 1, 'Shampoo antipereira', 12.90, 'S', 1, 24.90, 'UN');

CALL sp_cadastrar_produto('Condicionador 300ml', 1, NULL, 10.00, 'S', 2, 19.90, 'UN');