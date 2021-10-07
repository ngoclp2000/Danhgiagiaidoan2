-- 
-- Set character set the client will use to send SQL statements to the server
--
SET NAMES 'utf8';

--
-- Set default database
--
USE `Web06.TEST.MF914.TBNGOC`;

--
-- Create view `view_periodsalaryemployee`
--
CREATE
DEFINER = 'root'@'localhost'
VIEW view_periodsalaryemployee
AS
SELECT
  `pse`.`PeriodSalaryEmployeeId` AS `PeriodSalaryEmployeeId`,
  `pse`.`Salary` AS `Salary`,
  `pse`.`SocialInsuranceCost` AS `SocialInsuranceCost`,
  `pse`.`Subsidy` AS `Subsidy`,
  `pse`.`CoefficientsSalary` AS `CoefficientsSalary`,
  `pse`.`EmployeeId` AS `EmployeeId`,
  `pse`.`PeriodSalaryId` AS `PeriodSalaryId`,
  `pse`.`CreatedDate` AS `CreatedDate`,
  `pse`.`CreatedBy` AS `CreatedBy`,
  `pse`.`ModifiedDate` AS `ModifiedDate`,
  `pse`.`ModifiedBy` AS `ModifiedBy`
FROM `web06.test.mf914.tbngoc`.`periodsalaryemployee` `pse`;

--
-- Create view `view_periodsalary`
--
CREATE
DEFINER = 'root'@'localhost'
VIEW view_periodsalary
AS
SELECT
  `p`.`PeriodSalaryId` AS `PeriodSalaryId`,
  `p`.`PeriodName` AS `PeriodName`,
  `p`.`Month` AS `Month`,
  `p`.`Year` AS `Year`,
  `p`.`Note` AS `Note`,
  `p`.`CreatedDate` AS `CreatedDate`,
  `p`.`CreatedBy` AS `CreatedBy`,
  `p`.`ModifiedDate` AS `ModifiedDate`,
  `p`.`ModifiedBy` AS `ModifiedBy`,
  COUNT(`ps`.`PeriodSalaryId`) AS `NumberOfEmployee`,
  SUM(`ps`.`Salary`) AS `TotalSalary`
FROM (`web06.test.mf914.tbngoc`.`periodsalary` `p`
  JOIN `web06.test.mf914.tbngoc`.`periodsalaryemployee` `ps`
    ON (`p`.`PeriodSalaryId` = `ps`.`PeriodSalaryId`))
WHERE `p`.`Status` = 1
GROUP BY `p`.`PeriodSalaryId`;

--
-- Create view `view_employee`
--
CREATE
DEFINER = 'root'@'localhost'
VIEW view_employee
AS
SELECT
  `e`.`EmployeeId` AS `EmployeeId`,
  `e`.`EmployeeCode` AS `EmployeeCode`,
  `e`.`EmployeeName` AS `EmployeeName`,
  `e`.`Gender` AS `Gender`,
  `e`.`DateOfBirth` AS `DateOfBirth`,
  `e`.`PhoneNumber` AS `PhoneNumber`,
  `e`.`TelephoneNumber` AS `TelephoneNumber`,
  `e`.`EmployeePosition` AS `EmployeePosition`,
  `e`.`BankAccountNumber` AS `BankAccountNumber`,
  `e`.`BankName` AS `BankName`,
  `e`.`BankBranchName` AS `BankBranchName`,
  `e`.`Email` AS `Email`,
  `e`.`Address` AS `Address`,
  `e`.`IdentityNumber` AS `IdentityNumber`,
  `e`.`IdentityDate` AS `IdentityDate`,
  `e`.`IdentityPlace` AS `IdentityPlace`,
  `d`.`DepartmentId` AS `DepartmentId`,
  `d`.`DepartmentCode` AS `DepartmentCode`,
  `d`.`DepartmentName` AS `DepartmentName`,
  `e`.`CreatedDate` AS `CreatedDate`,
  `e`.`CreatedBy` AS `CreatedBy`,
  `e`.`ModifiedDate` AS `ModifiedDate`,
  `e`.`ModifiedBy` AS `ModifiedBy`
FROM (`web06.test.mf914.tbngoc`.`employee` `e`
  LEFT JOIN `web06.test.mf914.tbngoc`.`department` `d`
    ON (`e`.`DepartmentId` = `d`.`DepartmentId`))
WHERE `e`.`Status` = 1;