using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineTeacher.ViewModels.Students
{
    public class AddedStudentViewModel
    {
        public AddedStudentViewModel(string Name , string phone  , string UserID , int levelID, string Email)
        {
            this.Name = Name;
            this.Phone = phone;
            this.UserID = UserID;
            this.LevelID = levelID;
            this.Email = Email;
        }
        public AddedStudentViewModel()
        {

        }
       
        public string Name { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
   
        [Range(minimum: 1, maximum: double.MaxValue)]
        public int LevelID { get; set; }
        public string UserID { get;  set; }
       
    }
}
