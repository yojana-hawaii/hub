using hub.dbMigration.dbContext;
using hub.domain.model.directory;
using System;
using System.Linq;

namespace hub.InMemoryTests.Seed
{
    internal class HubDbInitializer
    {
        internal static void Initialize(HubDbContext _context)
        {
            if (!_context.Departments.Any())
                SeedDepartment(_context);

            if (!_context.JobTitles.Any())
                SeedJobTitle(_context);

            if (!_context.Locations.Any())
                SeedLocation(_context);
            if (!_context.Employees.Any())
                SeedEmployees(_context);
        }

        private static void SeedEmployees(HubDbContext context)
        {
            var employees = new[]
           {
                new Employee {
                        EmployeeId = 1, FirstName = "e", LastName = "k", Email = "ek@email.com",
                        Username = "ek",
                        DepartmentId = 3, JobTitleId = 4,
                        PrimaryManagerId = null,
                        LocationId = 4,
                        Keyword = "e k, ek@email.com",
                },
                new Employee {
                        EmployeeId = 2, FirstName = "l", LastName = "s", Email = "ls@email.com",
                        Username = "ls",
                        DepartmentId = 4, JobTitleId = 3,
                        PrimaryManagerId = 1,
                        LocationId = 4,
                        Keyword = "l s, ls@email.com",
                        FullNumber = "963-8520",
                },
                new Employee {
                        EmployeeId = 3, FirstName = "a", LastName = "m", Email = "am@email.com",
                        Username = "am",
                        DepartmentId = 1, JobTitleId = 1,
                        PrimaryManagerId = 2,
                        LocationId = 1,
                        Keyword = "a m",
                        Extension = "987",
                },
                new Employee {
                        EmployeeId = 4, FirstName = "d", LastName = "c", Email = "dc@email.com",
                        Username = "dc",
                        DepartmentId = 2, JobTitleId = 3,
                        PrimaryManagerId = 2,
                        LocationId = 3,
                        Keyword = "d c",
                },
                new Employee {
                        EmployeeId = 5, FirstName = "r", LastName = "m", Email = "rm@email.com",
                        Username = "rm",
                        DepartmentId = 2, JobTitleId = 2,
                        PrimaryManagerId = 4,
                        LocationId = 3,
                        Keyword = "r m a",
                },

                new Employee {
                        EmployeeId = 6, FirstName = "d", LastName = "A", Email = "da@email.com",
                        Username = "da",
                        DepartmentId = 2, JobTitleId = 2,
                        PrimaryManagerId = 4,
                        LocationId = 1,
                        Keyword = "d a",
                },
                new Employee {
                        EmployeeId = 7, FirstName = "e", LastName = "z", Email = "ezemail.com",
                        Username = "ez",
                        DepartmentId = 2, JobTitleId = 2,
                        PrimaryManagerId = 4,
                        LocationId = 1,
                        Keyword = "e z",
                },
                new Employee {
                        EmployeeId = 8, FirstName = "r", LastName = "mo", Email = "rmo@email.com",
                        Username = "rmo",
                        DepartmentId = 2, JobTitleId = 2,
                        PrimaryManagerId = 4,
                        LocationId = 3,
                        Keyword = "r m o",
                },
                new Employee {
                        EmployeeId = 9, FirstName = "j", LastName = "c", Email = "jc@email.com",
                        Username = "jc",
                        DepartmentId = 1, JobTitleId = 1,
                        PrimaryManagerId = 2,
                        LocationId = 1,
                        Keyword = "j c",
                        FullNumber = "123-9876",
                        Extension = "123"
                },
                new Employee {
                        EmployeeId = 10, FirstName = "a", LastName = "mm", Email = "amm@email.com",
                        Username = "amm",
                        DepartmentId = 1, JobTitleId = 1,
                        PrimaryManagerId = 2,
                        LocationId = 2,
                        Keyword = "a mm",
                },
                new Employee {
                        EmployeeId = 11, FirstName = "a", LastName = "l", Email = "al@email.com",
                        Username = "al",
                        DepartmentId = 1, JobTitleId = 1,
                        PrimaryManagerId = 3,
                        LocationId = 2,
                        Keyword = "a l",
                },
                new Employee
                {
                    EmployeeId = 12, FirstName = "x", Email = "x@email.com",
                        Username = "x",
                        DepartmentId = 1, JobTitleId = 1,
                        PrimaryManagerId = 3,
                        LocationId = 2,
                        Keyword = "x",
                }
            };

            context.Employees.AddRange(employees);
            context.SaveChanges();
        }

        private static void SeedLocation(HubDbContext context)
        {
            var locations = new[]
            {
                new Location {LocationId=1, LocationName="IT Building"},
                new Location {LocationId=2, LocationName="Accounting Building"},
                new Location {LocationId=3, LocationName="Remote Worker"},
                new Location {LocationId=4, LocationName="Exec Building"},
            };
            context.Locations.AddRange(locations);
            context.SaveChanges();
        }

        private static void SeedJobTitle(HubDbContext context)
        {
            var jobTitles = new[]
            {
                new JobTitle { JobTitleId = 1, JobTitleName = "Systems Administrator"},
                new JobTitle { JobTitleId = 2, JobTitleName = "Payroll"},
                new JobTitle { JobTitleId = 3, JobTitleName = "CFO"},
                new JobTitle { JobTitleId = 4, JobTitleName = "CEO"},
            };

            context.JobTitles.AddRange(jobTitles);
            context.SaveChanges();
        }

        private static void SeedDepartment(HubDbContext context)
        {
           var departments = new[]
           {
                new Department { DepartmentId = 1, DepartmentName = "IT"},
                new Department { DepartmentId = 2, DepartmentName = "Accounting"},
                new Department { DepartmentId = 3, DepartmentName = "Billing"},
                new Department { DepartmentId = 4, DepartmentName = "Exec"},
            };

            context.Departments.AddRange(departments);
            context.SaveChanges();
        }
    }
}
