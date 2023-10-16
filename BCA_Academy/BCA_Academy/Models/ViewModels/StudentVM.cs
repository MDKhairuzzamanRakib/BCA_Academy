using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BCA_Academy.Models.ViewModels
{
    public class StudentVM
    {

        public StudentVM()
        {
            this.SubjectList = new List<int>();
        }
        public int StudentId { get; set; }
        [Required]
        public string StudentName { get; set; }
        [Required,Column(TypeName ="date"), DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        public HttpPostedFileBase PictureFile { get; set; }
        public bool MaritalStatus { get; set; }
        public List<int> SubjectList { get; set; }
    }
}