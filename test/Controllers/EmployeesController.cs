using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;
using test.Models.Domain;

namespace test.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
           var employees =await mvcDemoDbContext.Employees.ToListAsync();

            return View(employees);

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequst)
        {

            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequst.Name,
                Email = addEmployeeRequst.Email,
                Salary = addEmployeeRequst.Salary,
                DateOfBirth = addEmployeeRequst.DateOfBirth,
                Department = addEmployeeRequst.Department
            };


            await mvcDemoDbContext.Employees.AddAsync(employee);
            await mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        


        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee =await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewmodel = new UpdateEmployeeView()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Salary = employee.Salary,
                DateOfBirth = employee.DateOfBirth,
                Department = employee.Department
            };

             return await Task.Run(() => View("View",viewmodel));
            }
            return RedirectToAction("Index");
            

        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeView model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth= model.DateOfBirth;
                employee.Department = model.Department;

                await mvcDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");


        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeView model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if(employee != null)
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");


            }
            return RedirectToAction("Index");



        }





    }
}
