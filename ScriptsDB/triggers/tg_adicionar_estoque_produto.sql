DROP TRIGGER IF EXISTS `tg_adicionar_estoque_produto`;

CREATE TRIGGER `tg_adicionar_estoque_produto`
AFTER INSERT ON `produto`
FOR EACH ROW
BEGIN
  INSERT INTO estoque (id_item, quantidade_estoque)
  VALUES (NEW.id_item, 0);
END;

-- ########## TESTES ##########

-- A procedure sp_cadastrar_produto insere apenas em item/produto;
-- a trigger gera a linha em estoque com quantidade_estoque = 0.
CALL sp_cadastrar_produto('Sabonete 90g', 1, 'Sabonete em barra', 3.50, 'S', 1, 5.90, 'UN');
