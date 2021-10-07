-- 
-- Set character set the client will use to send SQL statements to the server
--
SET NAMES 'utf8';

--
-- Set default database
--
USE `Web06.TEST.MF914.TBNGOC`;

DELIMITER $$

--
-- Create function `Func_RetreiveGenderName`
--
CREATE DEFINER = 'root'@'localhost'
FUNCTION Func_RetreiveGenderName (Gender int)
RETURNS varchar(255) CHARSET utf8mb4
BEGIN
  IF Gender = 0 THEN
    RETURN 'Nam';
  ELSEIF Gender = 1 THEN
    RETURN 'Nữ';
  ELSE
    RETURN 'Khác';
  END IF;

END
$$

--
-- Create function `Func_GetSubfixIsNumber`
--
CREATE DEFINER = 'root'@'localhost'
FUNCTION Func_GetSubfixIsNumber (InputString varchar(255))
RETURNS int(11)
BEGIN
  DECLARE ctrNumber varchar(50);
  DECLARE finNumber varchar(50) DEFAULT '';
  DECLARE sChar varchar(1);
  DECLARE inti integer DEFAULT 1;
  SET inti = LENGTH(InputString);

  IF LENGTH(InputString) > 0 THEN
    WHILE (inti > 0) DO
      SET sChar = SUBSTRING(InputString, inti, 1);
      SET ctrNumber = FIND_IN_SET(sChar, '0,1,2,3,4,5,6,7,8,9');
      IF ctrNumber > 0 THEN
        SET finNumber = CONCAT(finNumber, sChar);
      ELSE
        RETURN CAST(REVERSE(finNumber) AS UNSIGNED);
      END IF;
      SET inti = inti - 1;
    END WHILE;
    RETURN CAST(REVERSE(finNumber) AS UNSIGNED);
  ELSE
    RETURN 0;
  END IF;
END
$$

--
-- Create function `Func_ExtractNumber`
--
CREATE DEFINER = 'root'@'localhost'
FUNCTION Func_ExtractNumber (InputString varchar(255))
RETURNS int(11)
BEGIN
  DECLARE ctrNumber varchar(50);
  DECLARE finNumber varchar(50) DEFAULT '';
  DECLARE sChar varchar(1);
  DECLARE inti integer DEFAULT 1;

  IF LENGTH(InputString) > 0 THEN
    WHILE (inti <= LENGTH(InputString)) DO
      SET sChar = SUBSTRING(InputString, inti, 1);
      SET ctrNumber = FIND_IN_SET(sChar, '0,1,2,3,4,5,6,7,8,9');
      IF ctrNumber > 0 THEN
        SET finNumber = CONCAT(finNumber, sChar);
      END IF;
      SET inti = inti + 1;
    END WHILE;
    RETURN CAST(finNumber AS UNSIGNED);
  ELSE
    RETURN 0;
  END IF;
END
$$

--
-- Create function `Func_CheckEqual`
--
CREATE DEFINER = 'root'@'localhost'
FUNCTION Func_CheckEqual (str1 varchar(255), str2 varchar(255))
RETURNS tinyint(1)
BEGIN
  -- Kiểm tra xem chuôi tìm kiếm có chứa unicode không
  IF (REPLACE(CONVERT(str1 USING ascii), '?', '') = str1) THEN
    -- TH không chứa unicode
    -- dùng like bình thường không phân biệt unicode
    IF (str2 LIKE CONCAT('%', str1, '%')) THEN
      RETURN TRUE;
    ELSE
      RETURN FALSE;
    END IF;
  ELSE
    -- TH có chưa unicode
    -- bắt buộc phải giống cả unicode
    IF LOWER(str2) LIKE CONCAT('%', BINARY LOWER(str1), '%') THEN
      RETURN TRUE;
    ELSE
      RETURN FALSE;
    END IF;
  END IF;
  RETURN 1;
END
$$

--
-- Create function `Func_CheckDuplicatePeriodSalary`
--
CREATE DEFINER = 'root'@'localhost'
FUNCTION Func_CheckDuplicatePeriodSalary (Month varchar(20), Year varchar(5), PeriodSalaryId char(36))
RETURNS tinyint(1)
BEGIN
  DECLARE NumberOfRow int;
  SELECT
    COUNT(*) INTO NumberOfRow
  FROM PeriodSalary p
  WHERE p.Month = MONTH
  AND p.Year = Year
  AND p.PeriodSalaryId <> PeriodSalaryId
  AND p.Status = 1;
  IF NumberOfRow > 0 THEN
    RETURN TRUE;
  ELSE
    RETURN FALSE;
  END IF;
END
$$

DELIMITER ;