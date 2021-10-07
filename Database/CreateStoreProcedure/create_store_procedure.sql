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
-- Create procedure `Proc_UpdatePeriodSalaryEmployee`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_UpdatePeriodSalaryEmployee (IN CoefficientsSalary decimal(10, 3), IN CreatedBy varchar(255), IN CreatedDate datetime, IN EmployeeCode varchar(255), IN EmployeeId char(36), IN EmployeeName varchar(255), IN EntityState varchar(255), IN ModifiedBy varchar(255), IN ModifiedDate datetime, IN PeriodSalaryEmployeeId char(36), IN PeriodSalaryId char(36), IN Salary int, IN SocialInsuranceCost int, IN Subsidy int)
BEGIN
  UPDATE PeriodSalaryEmployee ps
  SET ps.Salary = Salary,
      ps.SocialInsuranceCost = SocialInsuranceCost,
      ps.Subsidy = Subsidy,
      ps.CoefficientsSalary = CoefficientsSalary,
      ps.ModifiedDate = CURRENT_TIMESTAMP()
  WHERE ps.PeriodSalaryEmployeeId = PeriodSalaryEmployeeId;
  UPDATE PeriodSalary ps
  SET ps.ModifiedDate = CURRENT_TIMESTAMP()
  WHERE ps.PeriodSalaryId = PeriodSalaryId;
END
$$

--
-- Create procedure `Proc_UpdatePeriodSalary`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_UpdatePeriodSalary (IN CreatedBy varchar(255), IN CreatedDate datetime, IN EmployeeDeleteList longtext, IN EmployeeNewList longtext, IN EmployeeUpdateList longtext, IN EntityState int, IN ModifiedBy varchar(255), IN ModifiedDate datetime, IN Month varchar(20), IN Note varchar(255), IN NumberOfEmployee int, IN PeriodName varchar(255), IN PeriodSalaryId char(36), IN TotalSalary int, IN Year varchar(5))
BEGIN
  DECLARE i int UNSIGNED
  DEFAULT 0;
  --   DECLARE v_count INT UNSIGNED
  --         DEFAULT JSON_LENGTH(EmployeeList);
  DECLARE v_current_item longtext
  DEFAULT NULL;
  DECLARE v_count int DEFAULT 0;

  SET v_count = JSON_LENGTH(EmployeeNewList);


  WHILE i < v_count DO
    -- get the current item and build an SQL statement
    -- to pass it to a callback procedure
    SET v_current_item :=
    JSON_EXTRACT(EmployeeNewList, CONCAT('$[', i, ']'));
    SELECT
      REPLACE(v_current_item, '\\', '') INTO v_current_item;
    SELECT
      REPLACE(v_current_item, '"{', '{') INTO v_current_item;
    SELECT
      REPLACE(v_current_item, '}"', '}') INTO v_current_item;

    SET @CoefficientsSalary = JSON_EXTRACT(v_current_item, '$.CoefficientsSalary');
    SET @CreatedBy = JSON_EXTRACT(v_current_item, '$.CreatedBy');
    SET @CreatedDate = JSON_EXTRACT(v_current_item, '$.CreatedDate');
    SET @EmployeeCode = JSON_EXTRACT(v_current_item, '$.EmployeeCode');
    SET @EmployeeId = JSON_EXTRACT(v_current_item, '$.EmployeeId');
    SET @EmployeeName = JSON_EXTRACT(v_current_item, '$.EmployeeName');
    SET @EntityState = JSON_EXTRACT(v_current_item, '$.EntityState');
    SET @ModifiedBy = JSON_EXTRACT(v_current_item, '$.ModifiedBy');
    SET @ModifiedDate = JSON_EXTRACT(v_current_item, '$.ModifiedDate');
    SET @PeriodSalaryEmployeeId = JSON_EXTRACT(v_current_item, '$.PeriodSalaryEmployeeId');
    SET @Salary = JSON_EXTRACT(v_current_item, '$.Salary');
    SET @SocialInsuranceCost = JSON_EXTRACT(v_current_item, '$.SocialInsuranceCost');
    SET @Subsidy = JSON_EXTRACT(v_current_item, '$.Subsidy');

    SELECT
      REPLACE(@EmployeeId, '"', '') INTO @EmployeeId;

    INSERT INTO PeriodSalaryEmployee (PeriodSalaryEmployeeId, Salary, SocialInsuranceCost, Subsidy, CoefficientsSalary, EmployeeId, PeriodSalaryId, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)
      VALUES (UUID(), @Salary, @SocialInsuranceCost, @Subsidy, @CoefficientsSalary, @EmployeeId, PeriodSalaryId, CURRENT_TIMESTAMP(), @CreatedBy, CURRENT_TIMESTAMP(), @ModifiedBy);

    SET i = i + 1;
  END WHILE;
  SET i = 0;
  SET v_count = JSON_LENGTH(EmployeeUpdateList);
  WHILE i < v_count DO
    -- get the current item and build an SQL statement
    -- to pass it to a callback procedure
    SET v_current_item :=
    JSON_EXTRACT(EmployeeUpdateList, CONCAT('$[', i, ']'));
    --         SELECT REPLACE(v_current_item,'\\','') into v_current_item;
    --         SELECT REPLACE(v_current_item,'"{','{') into v_current_item;
    --         SELECT REPLACE(v_current_item,'}"','}') INTO v_current_item;

    SET @CoefficientsSalary = JSON_EXTRACT(v_current_item, '$.CoefficientsSalary');
    SET @PeriodSalaryEmployeeId = JSON_EXTRACT(v_current_item, '$.PeriodSalaryEmployeeId');
    SET @Salary = JSON_EXTRACT(v_current_item, '$.Salary');
    SET @SocialInsuranceCost = JSON_EXTRACT(v_current_item, '$.SocialInsuranceCost');
    SET @Subsidy = JSON_EXTRACT(v_current_item, '$.Subsidy');

    SET @PeriodSalaryEmployeeId = REPLACE(@PeriodSalaryEmployeeId, '"', '');
    UPDATE PeriodSalaryEmployee pse
    SET Salary = @Salary,
        SocialInsuranceCost = @SocialInsuranceCost,
        Subsidy = @Subsidy,
        CoefficientsSalary = @CoefficientsSalary,
        ModifiedDate = NOW()
    WHERE pse.PeriodSalaryEmployeeId = @PeriodSalaryEmployeeId;

    SET i = i + 1;
  END WHILE;
  SET i = 0;
  SET v_count = JSON_LENGTH(EmployeeDeleteList);

  WHILE i < v_count DO
    -- get the current item and build an SQL statement
    -- to pass it to a callback procedure
    SET v_current_item :=
    JSON_EXTRACT(EmployeeDeleteList, CONCAT('$[', i, ']'));
    --         SELECT REPLACE(v_current_item,'\\','') into v_current_item;
    --         SELECT REPLACE(v_current_item,'"{','{') into v_current_item;
    --         SELECT REPLACE(v_current_item,'}"','}') INTO v_current_item;


    SET @PeriodSalaryEmployeeId = JSON_EXTRACT(v_current_item, '$.PeriodSalaryEmployeeId');
    SET @PeriodSalaryEmployeeId = REPLACE(@PeriodSalaryEmployeeId, '"', '');
    DELETE
      FROM PeriodSalaryEmployee
    WHERE PeriodSalaryEmployee.PeriodSalaryEmployeeId = @PeriodSalaryEmployeeId;

    SET i = i + 1;
  END WHILE;
  UPDATE PeriodSalary p
  SET PeriodName = PeriodName,
      Month = Month,
      Year = Year,
      Note = Note,
      p.ModifiedDate = CURRENT_TIMESTAMP()
  WHERE p.PeriodSalaryId = PeriodSalaryId;
END
$$

--
-- Create procedure `Proc_UpdateEmployee`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_UpdateEmployee (IN Address varchar(255), IN BankAccountNumber varchar(20), IN BankBranchName varchar(255), IN BankName varchar(255), IN CreatedBy varchar(255), IN CreatedDate datetime, IN DateOfBirth datetime, IN DepartmentId char(36), IN DepartmentName varchar(255), IN Email varchar(100), IN EmployeeCode varchar(20), IN EmployeeId char(36), IN EmployeeName varchar(100), IN EmployeePosition varchar(255), IN EntitySate varchar(255), IN Gender int, IN GenderName varchar(255), IN IdentityDate datetime, IN IdentityNumber varchar(25), IN IdentityPlace varchar(255), IN ModifiedBy varchar(255), IN ModifiedDate datetime, IN PhoneNumber varchar(50), IN TelephoneNumber varchar(20))
BEGIN
  UPDATE Employee e
  SET e.EmployeeCode = EmployeeCode,
      e.EmployeeName = EmployeeName,
      e.Gender = Gender,
      e.DateOfBirth = DateOfBirth,
      e.PhoneNumber = PhoneNumber,
      e.TelephoneNumber = TelephoneNumber,
      e.EmployeePosition = EmployeePosition,
      e.BankAccountNumber = BankAccountNumber,
      e.BankName = BankName,
      e.BankBranchName = BankBranchName,
      e.Email = Email,
      e.Address = Address,
      e.IdentityNumber = IdentityNumber,
      e.IdentityDate = IdentityDate,
      e.IdentityPlace = IdentityPlace,
      e.DepartmentId = DepartmentId,
      e.ModifiedDate = CURRENT_TIMESTAMP()
  WHERE e.EmployeeId = EmployeeId;
END
$$

--
-- Create procedure `Proc_InsertPeriodSalaryEmployee`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_InsertPeriodSalaryEmployee (IN CoefficientsSalary decimal(10, 3), IN CreatedBy varchar(255), IN CreatedDate datetime, IN EmployeeCode varchar(255), IN EmployeeId char(36), IN EmployeeName varchar(255), IN EntityState varchar(255), IN ModifiedBy varchar(255), IN ModifiedDate datetime, IN PeriodSalaryEmployeeId char(36), IN PeriodSalaryId char(36), IN Salary int, IN SocialInsuranceCost int, IN Subsidy int)
BEGIN
  INSERT INTO PeriodSalaryEmployee (PeriodSalaryEmployeeId, Salary, SocialInsuranceCost, CoefficientsSalary, Subsidy, EmployeeId, PeriodSalaryId, CreatedDate)
    VALUES (UUID(), Salary, SocialInsuranceCost, CoefficientsSalary, Subsidy, EmployeeId, PeriodSalaryId, CURRENT_TIMESTAMP());
END
$$

--
-- Create procedure `Proc_InsertPeriodSalary`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_InsertPeriodSalary (IN CreatedBy varchar(255), IN CreatedDate datetime, IN EmployeeDeleteList longtext, IN EmployeeNewList longtext, IN EmployeeUpdateList longtext, IN EntityState int, IN ModifiedBy varchar(255), IN ModifiedDate datetime, IN Month varchar(20), IN Note varchar(255), IN NumberOfEmployee int, IN PeriodName varchar(255), IN PeriodSalaryId char(36), IN TotalSalary int, IN Year varchar(5))
BEGIN
  DECLARE i int UNSIGNED
  DEFAULT 0;
  --   DECLARE v_count INT UNSIGNED
  --         DEFAULT JSON_LENGTH(EmployeeList);
  DECLARE v_current_item longtext
  DEFAULT NULL;
  DECLARE v_count int DEFAULT 0;


  SET @PeriodSalaryId = UUID();
  SET v_count = JSON_LENGTH(EmployeeNewList);
  INSERT INTO PeriodSalary (PeriodSalaryId, PeriodName, Month, Year, Note, Status, CreatedDate, ModifiedDate)
    VALUES (@PeriodSalaryId, PeriodName, Month, Year, Note, 1, CURRENT_TIMESTAMP(), CURRENT_TIMESTAMP());

  WHILE i < v_count DO
    -- get the current item and build an SQL statement
    -- to pass it to a callback procedure
    SET v_current_item :=
    JSON_EXTRACT(EmployeeNewList, CONCAT('$[', i, ']'));
    SELECT
      REPLACE(v_current_item, '\\', '') INTO v_current_item;
    SELECT
      REPLACE(v_current_item, '"{', '{') INTO v_current_item;
    SELECT
      REPLACE(v_current_item, '}"', '}') INTO v_current_item;

    SET @CoefficientsSalary = JSON_EXTRACT(v_current_item, '$.CoefficientsSalary');
    SET @CreatedBy = JSON_EXTRACT(v_current_item, '$.CreatedBy');
    SET @CreatedDate = JSON_EXTRACT(v_current_item, '$.CreatedDate');
    SET @EmployeeCode = JSON_EXTRACT(v_current_item, '$.EmployeeCode');
    SET @EmployeeId = JSON_EXTRACT(v_current_item, '$.EmployeeId');
    SET @EmployeeName = JSON_EXTRACT(v_current_item, '$.EmployeeName');
    SET @EntityState = JSON_EXTRACT(v_current_item, '$.EntityState');
    SET @ModifiedBy = JSON_EXTRACT(v_current_item, '$.ModifiedBy');
    SET @ModifiedDate = JSON_EXTRACT(v_current_item, '$.ModifiedDate');
    SET @PeriodSalaryEmployeeId = JSON_EXTRACT(v_current_item, '$.PeriodSalaryEmployeeId');
    SET @Salary = JSON_EXTRACT(v_current_item, '$.Salary');
    SET @SocialInsuranceCost = JSON_EXTRACT(v_current_item, '$.SocialInsuranceCost');
    SET @Subsidy = JSON_EXTRACT(v_current_item, '$.Subsidy');
    SELECT
      REPLACE(@EmployeeId, '"', '') INTO @EmployeeId;

    INSERT INTO PeriodSalaryEmployee (PeriodSalaryEmployeeId, Salary, SocialInsuranceCost, Subsidy, CoefficientsSalary, EmployeeId, PeriodSalaryId, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)
      VALUES (UUID(), @Salary, @SocialInsuranceCost, @Subsidy, @CoefficientsSalary, @EmployeeId, @PeriodSalaryId, CURRENT_TIMESTAMP(), @CreatedBy, CURRENT_TIMESTAMP(), @ModifiedBy);

    SET i = i + 1;
  END WHILE;


END
$$

--
-- Create procedure `Proc_InsertEmployee`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_InsertEmployee (IN Address varchar(255), IN BankAccountNumber varchar(20), IN BankBranchName varchar(255), IN BankName varchar(255), IN CreatedBy varchar(255), IN CreatedDate datetime, IN DateOfBirth datetime, IN DepartmentId char(36), IN DepartmentName varchar(255), IN Email varchar(100), IN EmployeeCode varchar(20), IN EmployeeId char(36), IN EmployeeName varchar(100), IN EmployeePosition varchar(255), IN EntitySate varchar(255), IN Gender int, IN GenderName varchar(255), IN IdentityDate datetime, IN IdentityNumber varchar(25), IN IdentityPlace varchar(255), IN ModifiedBy varchar(255), IN ModifiedDate datetime, IN PhoneNumber varchar(50), IN TelephoneNumber varchar(20))
BEGIN
  INSERT INTO Employee (EmployeeId,
  EmployeeCode,
  EmployeeName,
  Gender,
  DateOfBirth,
  PhoneNumber,
  Email,
  Address,
  IdentityNumber,
  IdentityDate,
  IdentityPlace,
  DepartmentId,
  CreatedDate,
  TelephoneNumber,
  EmployeePosition,
  BankAccountNumber,
  BankName,
  BankBranchName,
  Status)
    VALUES (UUID(), EmployeeCode, EmployeeName, Gender, DateOfBirth, PhoneNumber, Email, Address, IdentityNumber, IdentityDate, IdentityPlace, DepartmentId, CURRENT_TIMESTAMP(), TelephoneNumber, EmployeePosition, BankAccountNumber, BankName, BankBranchName, 1);
END
$$

--
-- Create procedure `Proc_GetPeriodSalaryFilterPaging`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetPeriodSalaryFilterPaging (IN PeriodSalaryFilter varchar(255), IN PageIndex int, IN PageSize int, OUT TotalRecord int, OUT TotalPage int)
BEGIN
  DECLARE PageOffset int;
  SET @PeriodSalaryFilter = (SELECT
      PeriodSalaryFilter);
  SET PageOffset = PageIndex * PageSize;
  -- Kiểm tra xem kích thước trang đưa lên có > 0 hay không?
  IF PageSize > 0 THEN
    -- Tạo 1 bảng tạm thời để chưa thông tin được lọc
    CREATE TEMPORARY TABLE FilteredTable
    SELECT
      vps.PeriodSalaryId AS PeriodSalaryId,
      vps.`PeriodName` AS `PeriodName`,
      vps.`Month` AS `Month`,
      vps.`Year` AS `Year`,
      vps.Note,
      vps.`NumberOfEmployee`,
      vps.`TotalSalary`,
      vps.CreatedDate,
      vps.CreatedBy,
      vps.ModifiedDate,
      vps.ModifiedBy
    FROM View_PeriodSalary vps
    WHERE (@PeriodSalaryFilter <=> NULL
    OR Func_CheckEqual(@PeriodSalaryFilter, vps.PeriodName));

    -- Đưa ra kết quả
    SELECT
      *
    FROM FilteredTable ft
    ORDER BY ft.ModifiedDate DESC LIMIT PageSize OFFSET PageOffSet;
    SET TotalRecord = (SELECT
        COUNT(*)
      FROM FilteredTable ft);
    DROP TEMPORARY TABLE FilteredTable;

    SET @NumberRecordLastPage = TotalRecord % PageSize;
    IF @NumberRecordLastPage > 0 THEN
      SET TotalPage = FLOOR(TotalRecord / PageSize) + 1;
    ELSE
      SET TotalPage = TotalRecord / PageSize;
    END IF;
  END IF;
END
$$

--
-- Create procedure `Proc_GetPeriodSalaryEmployeeByPeriodSalaryId`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetPeriodSalaryEmployeeByPeriodSalaryId (IN PeriodSalaryId char(36))
BEGIN
  SELECT
    ps.EmployeeId,
    p.PeriodSalaryId,
    ps.PeriodSalaryEmployeeId,
    e.EmployeeCode,
    e.EmployeeName,
    ps.CoefficientsSalary,
    ps.Subsidy,
    ps.SocialInsuranceCost,
    ps.Salary,
    ps.CreatedDate,
    ps.CreatedBy,
    ps.ModifiedDate,
    ps.ModifiedBy
  FROM PeriodSalary p
    JOIN PeriodSalaryEmployee ps
      ON p.PeriodSalaryId = ps.PeriodSalaryId
    JOIN Employee e
      ON ps.EmployeeId = e.EmployeeId
  WHERE ps.PeriodSalaryId = PeriodSalaryId;
END
$$

--
-- Create procedure `Proc_GetPeriodSalaryEmployee`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetPeriodSalaryEmployee ()
BEGIN
  SELECT
    *
  FROM View_PeriodSalaryEmployee;
END
$$

--
-- Create procedure `Proc_GetPeriodSalaryById`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetPeriodSalaryById (IN PeriodSalaryId char(36))
BEGIN
  SELECT
    *
  FROM View_PeriodSalary vps
  WHERE vps.PeriodSalaryId = PeriodSalaryId;
END
$$

--
-- Create procedure `Proc_GetPeriodSalary`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetPeriodSalary ()
BEGIN
  SELECT
    *
  FROM View_PeriodSalary vps;
END
$$

--
-- Create procedure `Proc_GetNewEmployeeCode`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetNewEmployeeCode ()
BEGIN
  DECLARE maxCodeNumber int DEFAULT 0;
  DECLARE maxCode varchar(20) DEFAULT '';
  DECLARE n int DEFAULT 0;
  DECLARE i int DEFAULT 0;
  DECLARE code varchar(20) DEFAULT '';
  DECLARE codeNumber int DEFAULT 0;
  SELECT
    COUNT(*)
  FROM Employee e INTO n;
  WHILE i < n DO
    SELECT
      e.EmployeeCode
    FROM Employee e LIMIT i, 1 INTO code;
    SET codeNumber = Func_GetSubfixIsNumber(code);
    IF codeNumber > maxCodeNumber THEN
      SET maxCodeNumber = codeNumber;
      SET maxCode = code;
    END IF;
    SET i = i + 1;
  END WHILE;

  SET @SUB_NEW_CODE = maxCodeNumber + 1;
  SET @LASTEST_CODE_LEN = LENGTH(maxCode);
  SET @SUB_NEW_CODE_LEN = LENGTH(@SUB_NEW_CODE);
  SET @PREFIX_CODE = LEFT(maxCode, @LASTEST_CODE_LEN - @SUB_NEW_CODE_LEN);
  SET @NEW_CODE = CONCAT(@PREFIX_CODE, @SUB_NEW_CODE);
  SELECT
    @NEW_CODE Ma_NV;
END
$$

--
-- Create procedure `Proc_GetEmployeeFilterPaging`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetEmployeeFilterPaging (IN EmployeeFilter varchar(255), IN PageIndex int, IN PageSize int, OUT TotalRecord int, OUT TotalPage int)
BEGIN
  DECLARE PageOffset int;
  SET @EmployeeFilter = (SELECT
      EmployeeFilter);
  SET PageOffset = PageIndex * PageSize;

  IF PageSize > 0 THEN

    CREATE TEMPORARY TABLE FilteredTable
    SELECT
      e.EmployeeId,
      e.EmployeeCode,
      e.EmployeeName,
      e.Gender,
      e.DateOfBirth,
      e.PhoneNumber,
      e.Email,
      e.Address,
      e.EmployeePosition,
      e.BankAccountNumber,
      e.BankName,
      e.BankBranchName,
      e.TelephoneNumber,
      e.IdentityNumber,
      e.IdentityDate,
      e.IdentityPlace,
      e.DepartmentId,
      e.DepartmentCode,
      e.DepartmentName,
      e.CreatedDate,
      e.CreatedBy,
      e.ModifiedDate,
      e.ModifiedBy
    FROM View_Employee e
    WHERE (@EmployeeFilter <=> NULL
    OR Func_CheckEqual(@EmployeeFilter, e.EmployeeCode)
    OR Func_CheckEqual(@EmployeeFilter, e.EmployeeName)
    OR Func_CheckEqual(@EmployeeFilter, e.PhoneNumber))
    ORDER BY e.CreatedDate DESC
    ;

    SELECT
      *
    FROM FilteredTable ft LIMIT PageSize OFFSET PageOffSet;

    SET TotalRecord = (SELECT
        COUNT(*)
      FROM FilteredTable ft);
    DROP TEMPORARY TABLE FilteredTable;
    SET @NumberRecordLastPage = TotalRecord % PageSize;

    IF @NumberRecordLastPage > 0 THEN
      SET TotalPage = FLOOR(TotalRecord / PageSize) + 1;
    ELSE
      SET TotalPage = TotalRecord / PageSize;
    END IF;
  END IF;
END
$$

--
-- Create procedure `Proc_GetEmployeeById`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetEmployeeById (IN EmployeeId char(36))
BEGIN
  SELECT
    *
  FROM View_Employee e
  WHERE e.EmployeeId = EmployeeId;
END
$$

--
-- Create procedure `Proc_GetEmployeeByCode`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetEmployeeByCode (IN EmployeeCode varchar(20))
BEGIN
  SELECT
    *
  FROM Employee e
  WHERE e.EmployeeCode = EmployeeCode
  AND e.Status = 1;
END
$$

--
-- Create procedure `Proc_GetEmployee`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetEmployee ()
BEGIN
  SELECT
    *
  FROM View_Employee
  ORDER BY CreatedDate DESC;
END
$$

--
-- Create procedure `Proc_GetDepartment`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_GetDepartment ()
BEGIN
  SELECT
    *
  FROM Department d;
END
$$

--
-- Create procedure `Proc_DeletePeriodSalaryEmployeeById`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_DeletePeriodSalaryEmployeeById (IN PeriodSalaryEmployeeID char(36))
BEGIN
  SELECT
    pse.PeriodSalaryId INTO @PeriodSalaryId
  FROM PeriodSalaryEmployee pse
  WHERE pse.PeriodSalaryEmployeeId = PeriodSalaryEmployeeID;
  UPDATE PeriodSalary ps
  SET ps.ModifiedDate = CURRENT_TIMESTAMP()
  WHERE ps.PeriodSalaryId = @PeriodSalaryId;
  DELETE
    FROM PeriodSalaryEmployee
  WHERE PeriodSalaryEmployee.PeriodSalaryEmployeeID = PeriodSalaryEmployeeID;

END
$$

--
-- Create procedure `Proc_DeletePeriodSalaryById`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_DeletePeriodSalaryById (IN PeriodSalaryId varchar(255))
BEGIN
  -- Chuyển trạng thái của kỳ về trạng thái đóng.
  UPDATE PeriodSalary p
  SET p.Status = 0
  WHERE p.PeriodSalaryId = PeriodSalaryId;
END
$$

--
-- Create procedure `Proc_DeleteEmployeeById`
--
CREATE DEFINER = 'root'@'localhost'
PROCEDURE Proc_DeleteEmployeeById (IN EmployeeId varchar(36))
BEGIN
  UPDATE Employee e
  SET e.Status = 0
  WHERE e.EmployeeId = EmployeeId;
END
$$

DELIMITER ;