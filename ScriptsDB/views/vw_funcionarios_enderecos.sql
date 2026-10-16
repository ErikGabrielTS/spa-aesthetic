DROP VIEW IF EXISTS `vw_funcionarios_enderecos`;

CREATE VIEW `vw_funcionarios_enderecos` AS
SELECT
  p.id_pessoa,
  p.nome,
  p.cpf,
  p.email,
  p.telefone,
  p.ativo_pessoa,
  f.cargo,
  f.data_admissao,
  e.id_endereco,
  e.cep,
  e.numero,
  e.complemento
FROM pessoa p
INNER JOIN funcionario f ON f.id_pessoa = p.id_pessoa
LEFT JOIN endereco e ON e.id_pessoa = p.id_pessoa
ORDER BY p.nome, e.id_endereco;
