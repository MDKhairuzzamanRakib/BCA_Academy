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

        public ActionResult Edit(int? id)
        {
            Student student = db.Students.First(x => x.StudentId == id);
            var studentSubject = db.AdmissionEntries.Where(x => x.StudentId == id).ToList();

            StudentVM studentVM = new StudentVM()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                BirthDate = student.BirthDate,
                Age = student.Age,
                Picture = student.Picture,
                MaritalStatus = student.MaritalStatus,
            };
            Session["image"] = student.Picture;
            if (studentSubject.Count()>0)
            {
                foreach (var item in studentSubject)
                {
                    studentVM.SubjectList.Add(item.SubjectId);
                }
            }
            return View(studentVM);
        }

        [HttpPost]
        public ActionResult Edit(StudentVM studentVM, int[] subjectId)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    StudentId = studentVM.StudentId,
                    StudentName = studentVM.StudentName,
                    BirthDate = studentVM.BirthDate,
                    Age = studentVM.Age,
                    MaritalStatus = studentVM.MaritalStatus,
                };
                HttpPostedFileBase file = studentVM.PictureFile;
                if (file != null)
                {
                    string filePath = Path.Combine(
                        "/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    student.Picture = filePath;
                }

                var subjectEntry = db.AdmissionEntries.Where(x => x.StudentId == student.StudentId).ToList();

                foreach (var item in subjectEntry)
                {
                    db.AdmissionEntries.Remove(item);
                }
                foreach (var item in subjectId)
                {
                    AdmissionEntry admissionEntry = new AdmissionEntry()
                    {
                        StudentId = student.StudentId,
                        SubjectId = item
                    };
                    db.AdmissionEntries.Add(admissionEntry);
                }
                db.Entry(student).State= EntityState.Modified;
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");

        }

        public ActionResult Delete(int id)
        {
            Student student = db.Students.First(x => x.StudentId == id);

            var studentSubject = db.AdmissionEntries.Where(x => x.StudentId == id).ToList();
            StudentVM studentVM = new StudentVM()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                BirthDate = student.BirthDate,
                Age = student.Age,
                Picture = student.Picture,
                MaritalStatus = student.MaritalStatus,
            };

            if (studentSubject.Count>0)
            {
                foreach (var item in studentSubject)
                {
                    studentVM.SubjectList.Add(item.SubjectId);
                }
            }
            return View(studentVM);

        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            Student student = db.Students.Find(id);
            AdmissionEntry admissionEntry = db.AdmissionEntries.First(x => x.StudentId == id);
            if (student == null)
            {
                return HttpNotFound();
            }
            db.Entry(student).State = EntityState.Deleted;
            db.Entry(admissionEntry).State = EntityState.Deleted;
            db.SaveChanges();

            return PartialView("_success");
        }

    }
}