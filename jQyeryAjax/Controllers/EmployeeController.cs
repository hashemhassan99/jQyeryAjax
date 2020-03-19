using jQyeryAjax.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQyeryAjax.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAll()
        {
            return View(GetAllEmployee());
        }
        IEnumerable<Employee> GetAllEmployee()
        {
            using (DBModel db= new DBModel())
            {
                return db.Employees.ToList<Employee>();
            }
        }
       public ActionResult AddorEdit(int id=0)
        {
            Employee emp = new Employee();  
            //update function or edit function
            if(id !=0)
            {
                using (DBModel db= new DBModel())
                {
                    emp = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault<Employee>();
                }
            }
            return View(emp);
        }
        [HttpPost]
        public ActionResult AddorEdit(Employee emp)
        {
            try {
                //check if emp has an image or not
                if (emp.ImageUpload != null)
                {
                    String fileName = Path.GetFileNameWithoutExtension(emp.ImageUpload.FileName);
                    String extention = Path.GetExtension(emp.ImageUpload.FileName);
                    //i will create custom file name to avoid repetiation in folder file name
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
                    emp.ImagePath = "~/AppFiles/Images" + fileName;
                    emp.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images"), fileName));
                }
                using (DBModel db = new DBModel())
                {
                    //now for edit
                    if (emp.EmployeeID == 0)
                    {
                        db.Employees.Add(emp);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                   
                }
                //here i will return json object to print the sucsses mesage or the fail message
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Submited Successfily" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false,  message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
           
        }   
        public ActionResult Delet(int id)
        {
            try
            {
                using(DBModel db= new DBModel())
                {
                    Employee emp = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault<Employee>();
                    db.Employees.Remove(emp);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Deleted Successfily" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}