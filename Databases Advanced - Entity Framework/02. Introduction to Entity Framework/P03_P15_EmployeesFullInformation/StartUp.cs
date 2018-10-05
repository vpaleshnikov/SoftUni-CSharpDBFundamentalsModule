using System;
using System.Linq;
using P03_P15_EmployeesFullInformation.Data;
using P03_P15_EmployeesFullInformation.Data.Models;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace P03_P15_EmployeesFullInformation
{
    public class StartUp
    {
        public static void Main()
        {
            using (var dbContext = new SoftUniContext())
            {
                //P03_EmployeesFullInformation
                //GetFullInformationForEmployees(dbContext);

                //P04_EmployeesWithSalaryOver50000
                //GetEmployeesWithSalaryOver50000(dbContext);

                //P05_EmployeesFromResearchAndDevelopment
                //GetEmployeesFromResearchAndDevelopment(dbContext);

                //P06_AddingANewAddressAndUpdatingEmployee
                //AddingANewAddressAndUpdatingEmployee(dbContext);

                //P07_EmployeesAndProjects
                //GetEmployeesWhoHaveProjectsStartedInThePeriod2001_2003(dbContext);

                //P08_AddressesByTown
                //GetAddressesByTownName(dbContext);

                //P09_Employee147
                //PrintEmployeeWithId147(dbContext);

                //P10_DepartmentsWithMoreThan5Employees
                //GetDepartmentsWithMoreThan5Employees(dbContext);

                //P11_FindLatest10Projects
                //GetLatest10Projects(dbContext);

                //P12_IncreaseSalaries
                //IncreaseSalaries(dbContext);

                //P13_FindEmployeesByFirstNameStartingWithSa
                //GetEmployeesByFirstNameStartingWithSa(dbContext);

                //P14_DeleteProjectById
                //DeleteProjectById(dbContext);

                //P15_RemoveTowns
                //RemoveTowns(dbContext);
            }
        }

        //P15_RemoveTowns
        private static void RemoveTowns(SoftUniContext dbContext)
        {
            var townName = Console.ReadLine();
            var town = dbContext
                .Towns
                .Include(t => t.Addresses)
                .SingleOrDefault(t => t.Name == townName);

            var adressCount = 0;
            if (town != null)
            {
                adressCount = town.Addresses.Count;

                dbContext
                    .Employees
                    .Where(e => e.AddressId != null && town.Addresses.Any(a => a.AddressId == e.AddressId))
                    .ToList()
                    .ForEach(e => e.Address = null);

                dbContext.SaveChanges();

                dbContext
                    .Addresses
                    .RemoveRange(town.Addresses);

                dbContext.Towns.Remove(town);

                dbContext.SaveChanges();
            }

            Console.WriteLine($"{adressCount} address in {townName} was deleted");
        }

        //P14_DeleteProjectById
        private static void DeleteProjectById(SoftUniContext dbContext)
        {
            var projectId = 2;

            var empProjects = dbContext
                .EmployeesProjects
                .Where(ep => ep.ProjectId == projectId);
            
            dbContext.EmployeesProjects.RemoveRange(empProjects);

            var project = dbContext.Projects.Find(projectId);
            dbContext.Projects.Remove(project);

            dbContext.SaveChanges();

            dbContext
                .Projects
                .Take(10)
                .Select(p => p.Name)
                .ToList()
                .ForEach(p => Console.WriteLine(p));
        }

        //P13_FindEmployeesByFirstNameStartingWithSa
        private static void GetEmployeesByFirstNameStartingWithSa(SoftUniContext dbContext)
        {
            dbContext
                .Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    FullName = $"{e.FirstName} {e.LastName}",
                    e.JobTitle,
                    e.Salary
                })
                .ToList()
                .ForEach(e => Console.WriteLine($"{e.FullName} - {e.JobTitle} - (${e.Salary:F2})"));
        }

        //P12_IncreaseSalaries
        private static void IncreaseSalaries(SoftUniContext dbContext)
        {
            dbContext
                .Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services")
                .ToList()
                .ForEach(e => e.Salary += e.Salary * 0.12m);

            dbContext.SaveChanges();

            var employees = dbContext
                .Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    FullName = $"{e.FirstName} {e.LastName}",
                    e.Salary
                });

            foreach (var e in employees)
            {
                Console.WriteLine($"{e.FullName} (${e.Salary:F2})");
            }
        }

        //P11_FindLatest10Projects
        private static void GetLatest10Projects(SoftUniContext dbContext)
        {
            var projects = dbContext
                .Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                });

            using (var writer = new StreamWriter("projects.txt"))
            {
                foreach (var p in projects)
                {
                    writer.WriteLine(p.Name);
                    writer.WriteLine(p.Description);
                    writer.WriteLine(p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
                }
            }
        }

        //P10_DepartmentsWithMoreThan5Employees
        private static void GetDepartmentsWithMoreThan5Employees(SoftUniContext dbContext)
        {
            var departments = dbContext
                .Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    d.Manager.FirstName,
                    d.Manager.LastName,
                    Employees = d.Employees
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .Select(e => new { e.FirstName, e.LastName, e.JobTitle })
                });

            using (var writer = new StreamWriter("employees.txt"))
            {
                foreach (var d in departments)
                {
                    writer.WriteLine($"{d.Name} - {d.FirstName} {d.LastName}");

                    foreach (var e in d.Employees)
                    {
                        writer.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                    }

                    writer.WriteLine(new string('-', 10));
                }
            }
        }

        //P09_Employee147
        private static void PrintEmployeeWithId147(SoftUniContext dbContext)
        {
            var employees = dbContext
                .Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FullName = $"{e.FirstName} {e.LastName}",
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(ep => ep.Project.Name).OrderBy(ep => ep)
                });

            foreach (var e in employees)
            {
                Console.WriteLine($"{e.FullName} - {e.JobTitle}");

                foreach (var p in e.Projects)
                {
                    Console.WriteLine(p);
                }
            }
        }

        //P08_AddressesByTown
        private static void GetAddressesByTownName(SoftUniContext dbContext)
        {
            var addresses = dbContext
                .Addresses
                .Include(a => a.Town)
                .Include(a => a.Employees)
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList();

            foreach (var a in addresses)
            {
                Console.WriteLine($"{a.AddressText}, {a.Town.Name} - {a.Employees.Count} employees");
            }
        }

        //P07_EmployeesAndProjects
        private static void GetEmployeesWhoHaveProjectsStartedInThePeriod2001_2003(SoftUniContext dbContext)
        {
            var employees = dbContext
                .Employees
                .Where(e => e.EmployeesProjects
                    .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Take(30)
                .Select(e => new
                {
                    employeeFullName = $"{e.FirstName} {e.LastName}",
                    managerFullname = $"{e.Manager.FirstName} {e.Manager.LastName}",
                    Projects = e.EmployeesProjects
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate,
                            EndDate = ep.Project.EndDate
                        })
                });

            foreach (var e in employees)
            {
                Console.WriteLine($"{e.employeeFullName} – Manager: {e.managerFullname}");

                foreach (var p in e.Projects)
                {
                    var startDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    if (p.EndDate == null)
                    {
                        Console.WriteLine($"--{p.ProjectName} - {startDate} - not finished");
                        continue;
                    }

                    var endDate = p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    Console.WriteLine($"--{p.ProjectName} - {startDate} - {endDate}");
                }
            }
        }

        //P06_AddingANewAddressAndUpdatingEmployee
        private static void AddingANewAddressAndUpdatingEmployee(SoftUniContext dbContext)
        {
            var address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            dbContext
                .Addresses
                .Add(address);

            var nakov = dbContext
                .Employees
                .Where(e => e.LastName == "Nakov")
                .FirstOrDefault();

            nakov.Address = address;
            dbContext.SaveChanges();

            var addresses = dbContext
                .Employees
                .OrderByDescending(a => a.AddressId)
                .Take(10)
                .Select(a => a.Address.AddressText);

            foreach (var a in addresses)
            {
                Console.WriteLine(a);
            }
        }

        //P05_EmployeesFromResearchAndDevelopment
        private static void GetEmployeesFromResearchAndDevelopment(SoftUniContext dbContext)
        {
            var employees = dbContext
                .Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                });


            foreach (var e in employees)
            {
                Console.WriteLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:F2}");
            }
        }

        //P04_EmployeesWithSalaryOver50000
        private static void GetEmployeesWithSalaryOver50000(SoftUniContext dbContext)
        {
            var employeeNames = dbContext
                .Employees
                .Where(e => e.Salary > 50000)
                .Select(e => e.FirstName)
                .OrderBy(e => e);

            foreach (var name in employeeNames)
            {
                Console.WriteLine(name);
            }
        }

        //P03_EmployeesFullInformation
        private static void GetFullInformationForEmployees(SoftUniContext dbContext)
        {
            var employees = dbContext
                .Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    JobTitle = e.JobTitle,
                    Salary = $"{e.Salary:f2}"
                })
                .ToList();

            foreach (var e in employees)
            {
                Console.WriteLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary}");
            }
        }
    }
}
