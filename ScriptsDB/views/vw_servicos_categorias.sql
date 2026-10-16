DROP VIEW IF EXISTS `vw_servicos_categorias`;

CREATE VIEW `vw_servicos_categorias` AS
SELECT
  i.id_item,
  i.item,
  i.descricao_item,
  i.preco,
  i.ativo_produto_servico,
  c.id_categoria,
  c.descricao AS categoria
FROM item i
INNER JOIN categoria c ON c.id_categoria = i.id_categoria
WHERE i.tipo_item = 'S'
ORDER BY c.descricao, i.item;
