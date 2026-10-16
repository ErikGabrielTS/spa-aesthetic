DROP TRIGGER IF EXISTS `tg_atendimento_item_debita_estoque`;

DELIMITER $$

CREATE TRIGGER `tg_atendimento_item_debita_estoque`
BEFORE INSERT ON `atendimento_item`
FOR EACH ROW
BEGIN
  DECLARE v_tipo_item     CHAR(1) DEFAULT NULL;
  DECLARE v_estoque_atual INT UNSIGNED DEFAULT 0;
  DECLARE v_msg           VARCHAR(128);

  SELECT tipo_item INTO v_tipo_item
  FROM item
  WHERE id_item = NEW.id_item;

  IF v_tipo_item = 'P' THEN
    IF NEW.quantidade_itens IS NULL OR NEW.quantidade_itens <= 0 THEN
      SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Quantidade invalida no atendimento_item (>= 1).';
    END IF;

    SELECT IFNULL(quantidade_estoque, 0) INTO v_estoque_atual
    FROM estoque
    WHERE id_item = NEW.id_item;

    IF v_estoque_atual < NEW.quantidade_itens THEN
      SET v_msg = CONCAT(
        'Estoque insuficiente para o item ', NEW.id_item,
        ': disponivel = ', v_estoque_atual,
        ', solicitado = ', NEW.quantidade_itens
      );
      SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = v_msg;
    END IF;

    UPDATE estoque
    SET quantidade_estoque = quantidade_estoque - NEW.quantidade_itens
    WHERE id_item = NEW.id_item;
  END IF;
END$$

DELIMITER ;

-- ########## TESTES ##########

CALL sp_cadastrar_atendimento(
  3, NOW(), 'A',
  '[{"id_item": 1, "id_funcionario": 3, "quantidade_itens": 6, "valor_unitario_entrada": 25.00}]'
);