using BCA_Academy.Models;
using BCA_Academy.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCA_Academy.Controllers
{
    public class StudentController : Controller
    {
    AcademyEntities db = new AcademyEntities();
        public ActionResult Index()
        {
            var students = db.Students.Include(x=>x.AdmissionEntries.Select(b=>b.Subject)).OrderByDescending(x=>x.StudentId).ToList();
            return View(students);
        }

        public ActionResult AddNewSubject(int? id)
        {
            ViewBag.subjects = new SelectList
                (db.Subjects.ToList(), "SubjectId", "SubjectName",
                (id != null) ? id.ToString() : "");
            return PartialView("_addNewSubject");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentVM studentVM, int[] subjectId)
        {
            if(ModelState.IsValid)
            {
                Student student = new Student()
                {
                    StudentName = studentVM.StudentName,
                    BirthDate = studentVM.BirthDate,
                    Age = studentVM.Age,
                    MaritalStatus = studentVM.MaritalStatus,
                };
                HttpPostedFileBase file = studentVM.PictureFile;
                if (file!=null)
                {
                    string filePath = Path.Combine(
                        "/Images/",Guid.NewGuid().ToString()+Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    student.Picture = filePath;
                }
                foreach (var item in subjectId)
                {
                    AdmissionEntry admissionEntry = new AdmissionEntry()
                    {
                        Student = student,
                        StudentId = student.StudentId,
                        SubjectId = item
                    };
                    db.AdmissionEntries.Add(admissionEntry);
                }
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
    }
}