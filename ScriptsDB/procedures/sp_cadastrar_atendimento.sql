DROP PROCEDURE IF EXISTS `sp_cadastrar_atendimento`;

DELIMITER $$

CREATE PROCEDURE `sp_cadastrar_atendimento`(
  IN p_id_pessoa          BIGINT UNSIGNED,
  IN p_data_atendimento   DATETIME,
  IN p_status_atendimento CHAR(1),
  IN p_itens              JSON
)
BEGIN
  DECLARE v_id_atendimento BIGINT UNSIGNED DEFAULT NULL;
  DECLARE v_total_pessoa   INT DEFAULT 0;
  DECLARE v_total_itens    INT DEFAULT 0;

  DECLARE EXIT HANDLER FOR SQLEXCEPTION
  BEGIN
    ROLLBACK;
    RESIGNAL;
  END;

  START TRANSACTION;

  IF p_id_pessoa IS NULL THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe o id da pessoa.';
  END IF;

  SELECT COUNT(*) INTO v_total_pessoa
  FROM pessoa
  WHERE id_pessoa = p_id_pessoa;

  IF v_total_pessoa = 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Pessoa inexistente: cadastre a pessoa antes do atendimento.';
  END IF;

  IF p_status_atendimento IS NULL OR TRIM(p_status_atendimento) = '' THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe o status do atendimento.';
  END IF;

  IF p_itens IS NULL
     OR JSON_TYPE(p_itens) <> 'ARRAY'
     OR JSON_LENGTH(p_itens) = 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe ao menos um item: p_itens deve ser um JSON array nao vazio.';
  END IF;

  INSERT INTO atendimento
    (id_pessoa, data_atendimento, status_atendimento)
  VALUES
    (p_id_pessoa, IFNULL(p_data_atendimento, NOW()), p_status_atendimento);

  SET v_id_atendimento = LAST_INSERT_ID();

  INSERT INTO atendimento_item
    (id_atendimento, id_item, id_funcionario, quantidade_itens, valor_unitario_entrada)
  SELECT
    v_id_atendimento,
    i.id_item,
    i.id_funcionario,
    i.quantidade_itens,
    COALESCE(
      i.valor_unitario_entrada,
      (SELECT preco_venda FROM produto WHERE id_item = i.id_item),
      (SELECT preco FROM item WHERE id_item = i.id_item)
    )
  FROM JSON_TABLE(
    p_itens,
    '$[*]'
    COLUMNS(
      id_item                BIGINT UNSIGNED PATH '$.id_item',
      id_funcionario         BIGINT UNSIGNED PATH '$.id_funcionario',
      quantidade_itens       INT UNSIGNED    PATH '$.quantidade_itens',
      valor_unitario_entrada DECIMAL(10,2)   PATH '$.valor_unitario_entrada'
    )
  ) AS i;

  SET v_total_itens = ROW_COUNT();

  IF v_total_itens = 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Nenhum item foi associado ao atendimento; a operacao foi cancelada.';
  END IF;

  COMMIT;
END$$

DELIMITER ;

-- ########## TESTES ##########

-- Funcionario que atenderá o atendimento.

-- Item 1: valor_unitario_entrada em informado no JSON;
-- Item 2: valor omitido -> fallback para produto.preco_venda (32.40).
CALL sp_cadastrar_atendimento(
  2,
  NOW(),
  'A',
  '[
    {"id_item": 1, "id_funcionario": 3, "quantidade_itens": 2, "valor_unitario_entrada": 25.00},
    {"id_item": 2, "id_funcionario": 3, "quantidade_itens": 1}
  ]'
);