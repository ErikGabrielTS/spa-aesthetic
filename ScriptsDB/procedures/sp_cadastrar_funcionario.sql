DROP PROCEDURE IF EXISTS `sp_cadastrar_funcionario`;

DELIMITER $$

CREATE PROCEDURE `sp_cadastrar_funcionario`(
  IN p_nome            VARCHAR(100),
  IN p_cpf             CHAR(11),
  IN p_data_nascimento DATE,
  IN p_sexo            CHAR(1),
  IN p_email           VARCHAR(100),
  IN p_telefone        CHAR(11),
  IN p_ativo_pessoa    CHAR(1),
  IN p_cargo           VARCHAR(20),
  IN p_data_admissao   DATE,
  IN p_enderecos       JSON
)
BEGIN
  DECLARE v_id_pessoa       BIGINT UNSIGNED DEFAULT NULL;
  DECLARE v_total_enderecos INT DEFAULT 0;
  DECLARE v_total_cpf       INT DEFAULT 0;
  DECLARE v_total_email     INT DEFAULT 0;

  DECLARE EXIT HANDLER FOR SQLEXCEPTION
  BEGIN
    ROLLBACK;
    RESIGNAL;
  END;

  START TRANSACTION;

  IF p_cargo IS NULL
     OR TRIM(p_cargo) = ''
     OR CHAR_LENGTH(p_cargo) > 20 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe um cargo valido (1 a 20 caracteres).';
  END IF;

  IF p_sexo IS NULL OR p_sexo NOT IN ('M', 'F') THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Sexo invalido: use M ou F.';
  END IF;

  IF p_ativo_pessoa IS NULL OR p_ativo_pessoa NOT IN ('S', 'N') THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'ativo_pessoa invalido: use S ou N.';
  END IF;

  IF p_data_admissao IS NULL OR p_data_admissao > CURDATE() THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Data de admissao invalida: nao pode ser nula nem futura.';
  END IF;

  IF p_data_admissao < p_data_nascimento THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Data de admissao anterior a data de nascimento.';
  END IF;

  IF p_enderecos IS NULL
     OR JSON_TYPE(p_enderecos) <> 'ARRAY'
     OR JSON_LENGTH(p_enderecos) = 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe ao menos um endereco: p_enderecos deve ser um JSON array nao vazio.';
  END IF;

  SELECT COUNT(*) INTO v_total_cpf
  FROM pessoa
  WHERE cpf = p_cpf;

  IF v_total_cpf > 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Ja existe uma pessoa cadastrada com este CPF.';
  END IF;

  SELECT COUNT(*) INTO v_total_email
  FROM pessoa
  WHERE email = p_email;

  IF v_total_email > 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Ja existe uma pessoa cadastrada com este e-mail.';
  END IF;

  INSERT INTO pessoa
    (nome, cpf, data_nascimento, sexo, email, telefone, ativo_pessoa)
  VALUES
    (p_nome, p_cpf, p_data_nascimento, p_sexo, p_email, p_telefone, p_ativo_pessoa);

  SET v_id_pessoa = LAST_INSERT_ID();

  INSERT INTO funcionario
    (id_pessoa, cargo, data_admissao)
  VALUES
    (v_id_pessoa, p_cargo, p_data_admissao);

  INSERT INTO endereco
    (id_pessoa, cep, numero, complemento)
  SELECT
    v_id_pessoa,
    e.cep,
    e.numero,
    e.complemento
  FROM JSON_TABLE(
    p_enderecos,
    '$[*]'
    COLUMNS(
      cep         CHAR(8)      PATH '$.cep',
      numero      VARCHAR(10)  PATH '$.numero',
      complemento VARCHAR(150) PATH '$.complemento'
    )
  ) AS e;

  SET v_total_enderecos = ROW_COUNT();

  IF v_total_enderecos = 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Nenhum endereco foi associado ao funcionario; a operacao foi cancelada.';
  END IF;

  COMMIT;
END$$

DELIMITER ;

-- ########## TESTES ##########

CALL sp_cadastrar_funcionario(
  'Joao Lima', '98765432100', '1995-02-10', 'M',
  'joao.lima@email.com', '11976543210', 'S',
  'Vendedor',
  '2026-03-01',
  '[{"cep":"01310100","numero":"250","complemento":"Bloco B"},
    {"cep":"04567000","numero":"90"}]'
);