DROP VIEW IF EXISTS `vw_atendimentos_detalhes`;

CREATE VIEW `vw_atendimentos_detalhes` AS
SELECT
  a.id_atendimento,
  a.data_atendimento,
  a.status_atendimento,
  pc.id_pessoa AS id_cliente,
  pc.nome AS cliente,
  pc.cpf AS cpf_cliente,
  i.id_item,
  i.item AS produto,
  ai.quantidade_itens,
  ai.valor_unitario_entrada,
  (ai.quantidade_itens * ai.valor_unitario_entrada) AS valor_total_item,
  pf.id_pessoa AS id_funcionario,
  pf.nome AS funcionario
FROM atendimento a
INNER JOIN pessoa pc ON pc.id_pessoa = a.id_pessoa
INNER JOIN atendimento_item ai ON ai.id_atendimento = a.id_atendimento
INNER JOIN item i ON i.id_item = ai.id_item
INNER JOIN pessoa pf ON pf.id_pessoa = ai.id_funcionario
ORDER BY a.data_atendimento, a.id_atendimento, i.item;
