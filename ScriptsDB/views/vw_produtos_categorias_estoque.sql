DROP VIEW IF EXISTS `vw_produtos_categorias_estoque`;

CREATE VIEW `vw_produtos_categorias_estoque` AS
SELECT
  i.id_item,
  i.item,
  i.descricao_item,
  i.ativo_produto_servico,
  c.id_categoria,
  c.descricao AS categoria,
  p.codigo_preco,
  p.preco_venda,
  p.unidade,
  COALESCE(e.quantidade_estoque, 0) AS quantidade_estoque
FROM item i
INNER JOIN categoria c ON c.id_categoria = i.id_categoria
INNER JOIN produto p ON p.id_item = i.id_item
LEFT JOIN estoque e ON e.id_item = i.id_item
WHERE i.tipo_item = 'P'
ORDER BY c.descricao, i.item;
