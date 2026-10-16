DROP TRIGGER IF EXISTS `tg_entrada_produto_ajustes`;

CREATE TRIGGER `tg_entrada_produto_ajustes`
AFTER INSERT ON `entrada_produto`
FOR EACH ROW
BEGIN
  DECLARE v_codigo_preco INT UNSIGNED DEFAULT NULL;

  IF NEW.valor_unitario_entrada IS NULL OR NEW.valor_unitario_entrada < 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe um valor unitario de entrada valido (>= 0).';
  END IF;

  IF NEW.quantidade_entrada IS NULL OR NEW.quantidade_entrada < 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe uma quantidade de entrada valida (>= 0).';
  END IF;

  UPDATE item
  SET preco = NEW.valor_unitario_entrada
  WHERE id_item = NEW.id_item;

  SELECT codigo_preco INTO v_codigo_preco
  FROM produto
  WHERE id_item = NEW.id_item;

  IF v_codigo_preco IN (1, 2, 3) THEN
    UPDATE produto
    SET preco_venda = fn_calcular_valor_venda(v_codigo_preco, NEW.valor_unitario_entrada)
    WHERE id_item = NEW.id_item;
  END IF;

  UPDATE estoque
  SET quantidade_estoque = quantidade_estoque + NEW.quantidade_entrada
  WHERE id_item = NEW.id_item;
END;

-- ########## TESTES ##########

-- Rode os testes da procedure sp_cadastrar_produto para criar o produto
-- e a trigger tg_adicionar_estoque_produto para criar a linha em estoque
-- com quantidade_estoque = 0.

INSERT INTO entrada_produto
  (id_item, data_entrada_produto, quantidade_entrada, unidade, valor_unitario_entrada)
VALUES
  (1, NOW(), 10.00, 'UN', 13.50);