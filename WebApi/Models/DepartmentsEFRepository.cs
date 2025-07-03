
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Models
{
    public class DepartmentsEFRepository : IDepartmentsRepository
    {
        public CompanyDbContext Context { get; }

        public DepartmentsEFRepository(CompanyDbContext context)
        {
            Context = context;
        }

        
        public void AddDepartment(Department? department)
        {
            if(department == null)
            {
                return;
            }

            Context.Add(department);
            Context.SaveChanges();

        }

        public bool DeleteDepartment(Department? department)
        {
            if (department == null)
            {
                return false;
            }

            var dep = Context.Departments?.Find(department.Id);
            if (dep == null) return false;

            Context.Departments?.Remove(department);
            Context.SaveChanges();

            return true;

        }

        public bool DepartmentExists(int departmentId)
        {
            var exists = Context.Departments?.Any(x => x.Id == departmentId);

            return exists.HasValue && exists.Value;
        }

        public Department? GetDepartmentById(int id)
        {
            return Context.Departments?.Find(id);
        }

        public List<Department> GetDepartments(string? filter = null)
        {
            if(string.IsNullOrWhiteSpace(filter))
            {
                return Context.Departments?.ToList();
            }

            return Context.Departments?
                .Where(x => EF.Functions.Like(x.Name, $"%{filter}%"))
                .ToList();
        }

        public bool UpdateDepartment(Department? department)
        {
            if (department == null)
            {
                return false;
            }

            var dep = Context.Departments?.Find(department.Id);
            if (dep == null) return false;

            dep.Name = department.Name;
            dep.Description = department.Description;
            dep.Email = department.Email;

            Context.SaveChanges();


            return true;
        }
    }
}
