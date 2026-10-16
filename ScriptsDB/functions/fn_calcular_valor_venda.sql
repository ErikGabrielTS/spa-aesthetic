DROP FUNCTION IF EXISTS `fn_calcular_valor_venda`;

CREATE FUNCTION `fn_calcular_valor_venda`(
  p_codigo_preco INT UNSIGNED,
  p_preco        DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
DETERMINISTIC
BEGIN
  DECLARE v_percentual DECIMAL(5,2);

  IF p_preco IS NULL OR p_preco < 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Informe um preco valido (>= 0).';
  END IF;

  IF p_codigo_preco IS NULL OR p_codigo_preco NOT IN (1, 2, 3) THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Codigo de preco invalido: use 1, 2 ou 3.';
  END IF;

  SET v_percentual = CASE p_codigo_preco
    WHEN 1 THEN 1.04
    WHEN 2 THEN 1.08
    WHEN 3 THEN 1.10
  END;

  RETURN ROUND(p_preco * v_percentual, 2);
END;

-- ########## TESTES ##########

SELECT fn_calcular_valor_venda(1, 100.00) AS valor_venda_codigo_1; -- espera 104.00 (acrescimo de 4%)
SELECT fn_calcular_valor_venda(2, 100.00) AS valor_venda_codigo_2; -- espera 108.00 (acrescimo de 8%)
SELECT fn_calcular_valor_venda(3, 100.00) AS valor_venda_codigo_3; -- espera 110.00 (acrescimo de 10%)
