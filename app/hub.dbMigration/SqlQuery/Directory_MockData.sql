
use TheHub
GO

DELETE FROM employees
dbcc checkident ('Employees', reseed, 0)
GO
delete from Departments
dbcc checkident ('Departments', reseed, 0)
GO
delete from JobTitles
dbcc checkident ('JobTitles', reseed, 0)
go
delete from Locations
dbcc checkident('Locations', reseed, 0)
GO

delete from FaxNumbers
dbcc checkident('Faxnumbers', reseed, 0)
go


INSERT INTO Departments(DepartmentName) 
VALUES('IT'),('Accouting'),('Billing'),('Exec');
SELECT* FROM Departments;

go

insert into JobTitles(JobTitleName)
values('SysAdmin'), ('Payroll'), ('CFO'), ('CEO');
select* from JobTitles;
go

insert into Locations(LocationName)
values('Secondary'),('Main'), ('Remote');
select * from Locations;
go

insert into FaxNumbers(FaxName, Number, LocationId, DepartmentId)
values('IT', '123-9856', 1, 1),
('payroll', '6653532', 2, null),
('billing', '123-2125', null, 3),
('exec', '1238654', 3, 4);
select * from FaxNumbers;
go

insert into Employees
(FirstName, LastName, UserName, Email, AccountCreated, HireDate, PhotoPath, EmployeeNumber, FullNumber, Extension, DepartmentId, JobTitleId, LocationId, PrimaryManagerId, Keyword)
values
('e', 'k', 'ek', 'ek@email.com', '2006-08-28', '2006-08-28', null, null, null, null, null, 4,1, null, 'ceo'),
('l', 's', 'ls', 'ls@email.com', '2010-02-17', '2010-02-17', null, null, '123-7648', 'Ext. 256',4, 3,3, 1, 'cfo'),
('d', 'c', 'dc', 'dc@email.com', '2014-07-14', '2014-07-14', null, null, null, null, 2, 2,3, 2, 'd c'),
('r', 'm', 'rma', 'rma@email.com', '2014-09-16', '2014-09-16', null, null, '123-7152', 'Ext. 8242', 3, 2,3, 3, 'r m'),
('d', 'a', 'da', 'da@email.com', '2014-02-10', '2014-02-10', null, null, '321-9632', 'Ext. 3451', 3, 2,null, 3, 'd a'),
('e', 'z', 'ez', 'ez@email.com', '2006-08-28', '2006-08-28', null, null, '123-7266', 'Ext. 857', 2, 2,2, 3, 'e z'),
('r', 'm', 'rmo', 'rmo@email.com', '2011-09-01', '2011-09-01', null, null, null, 'Ext. 6542', 2, 2,3, 3,'r m'),
('a', 'm', 'am', 'am@email.com', '2011-09-20', '2011-09-20', null, null, '123-7377', '1277', 1, 1,2, 2,'a m'),
('j', 'c', 'jc', 'jc@email.com', '2006-08-16', '2006-08-16', null, null, '321-8368', 'Ext. 3168', 1, 1,null, 2, 'j c'),
('a', 'm', 'amm', 'amm@email.com', '2016-05-23', '2016-05-23', null, null, '321-8361', 'Ext. 3611', 1, null,2, 2, 'a m'),
('a', 'l', 'al', 'al@email.com', '2019-06-15', null, null, null, '123-654-8532', '5321', 1, 1,2, 8, 'a l'),
('p', 'm', 'pm', 'pm@email.com', '2020-06-15', null, null, null, '321-859-1526', '5261', null, 1,2, 8, 'p m');
select* from Employees

go
select 
	emp.FirstName, emp.LastName, emp.UserName, emp.Email, 
	cast(emp.HireDate as date) HireDate, 
	emp.FullNumber, emp.Extension,
	dept.DepartmentName, job.JobTitleName, loc.LocationName, 
	sup.FirstName + ' ' + sup.LastName Manager
from Employees emp
	left join Departments dept on emp.DepartmentId = dept.DepartmentId
	left join JobTitles job on emp.JobTitleId = job.JobTitleId
	left join Locations loc on loc.LocationId = emp.LocationId
	left join Employees sup on emp.PrimaryManagerId = sup.EmployeeId

go


/*
begin tran
update e
set e.email = 'test@email.com'
--select * 
from Employees e
where e.employeeid = 1;
*/
--commit