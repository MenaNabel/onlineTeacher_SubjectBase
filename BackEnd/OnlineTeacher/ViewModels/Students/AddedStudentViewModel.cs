using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineTeacher.ViewModels.Students
{
    public class AddedStudentViewModel
    {
        public AddedStudentViewModel(string Name, string phone, string ParentPhone, int studentID, int levelID, string Email, string City, string School = "لا يوجد مدرسه")
        {
            this.Name = Name;
            this.Phone = phone;
            this.ID = studentID;
            this.LevelID = levelID;
            this.Email = Email;
            this.City = City;
            this.school = School;
            this.ParentPhone = ParentPhone;
        }
        public AddedStudentViewModel()
        {

        }
       
        public string Name { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string ParentPhone { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string school { get; set; }

        [Range(minimum: 1, maximum: double.MaxValue)]
        public int LevelID { get; set; }
        public int ID { get;  set; }
       
    }
}
