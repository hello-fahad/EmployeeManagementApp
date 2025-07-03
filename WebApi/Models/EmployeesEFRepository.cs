
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using WebApi.Data;

namespace WebApi.Models
{
    public class EmployeesEFRepository : IEmployeesRepository
    {

        public CompanyDbContext Context { get; }

        public EmployeesEFRepository(CompanyDbContext context)
        {
            Context = context;
        }


        public void AddEmployee(Employee? employee)
        {
            if (employee == null) return;

            Context.Employees?.Add(employee);
            Context.SaveChanges();
        }

        public bool DeleteEmployee(Employee? employee)
        {
            if (employee == null) return false;

            var emp = Context.Employees?.Find(employee.Id);
            if (emp == null) return false;

            Context.Employees?.Remove(employee);
            Context.SaveChanges();

            return true;
        }

        public bool EmployeeExists(int employeeId)
        {
            var exists = Context.Employees?.Any(x => x.Id == employeeId);

            return exists.HasValue && exists.Value;
        }

        public Employee? GetEmployeeById(int id)
        {
            return Context.Employees?.Include(x => x.Department).First(x => x.Id == id);
        }

        public List<Employee> GetEmployees(string? filter = null, int? departmentId = null)
        {
            if (departmentId.HasValue)
            {
                return Context.Employees?
                    .Where(x => x.DepartmentId == departmentId.Value)
                    .Include(x => x.Department)
                    .ToList();
            }
            else if(!string.IsNullOrWhiteSpace(filter))
            {
                return Context.Employees?
                    .Where(x => EF.Functions.Like(x.Name, $"%{filter}%"))
                    .Include(x => x.Department)
                    .ToList();
            }

            return Context.Employees?.Include(x => x.Department).ToList();
        }

        public bool UpdateEmployee(Employee? employee)
        {
            if (employee == null) return false;

            var emp = Context.Employees?.Find(employee.Id);
            if (emp == null) return false;

            emp.Name = employee.Name;
            emp.Position = employee.Position;
            emp.Salary = employee.Salary;
            emp.DepartmentId = employee.Id;

            Context.SaveChanges();

            return true;

        }
    }
}
