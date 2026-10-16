DROP VIEW IF EXISTS `vw_clientes_enderecos`;

CREATE VIEW `vw_clientes_enderecos` AS
SELECT
  p.id_pessoa,
  p.nome,
  p.cpf,
  p.email,
  p.telefone,
  p.ativo_pessoa,
  e.id_endereco,
  e.cep,
  e.numero,
  e.complemento
FROM pessoa p
LEFT JOIN endereco e ON e.id_pessoa = p.id_pessoa
WHERE NOT EXISTS (
  SELECT 1 FROM funcionario f WHERE f.id_pessoa = p.id_pessoa
)
ORDER BY p.nome, e.id_endereco;
