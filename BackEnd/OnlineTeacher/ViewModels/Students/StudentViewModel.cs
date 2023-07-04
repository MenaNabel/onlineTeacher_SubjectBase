using OnlineTeacher.ViewModels.Levels;

namespace OnlineTeacher.ViewModels.Students
{
    public class StudentViewModel
    {
        public int ID { get; set; }
      
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ParentPhone { get; set; }
        public string City { get; set; }
        public int LevelID { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string school { get; set; }
        public string LevelName { get; set; }
        public LevelViewModel Level { get; set; }

    }
    public class StudentViewModelWithoutImage
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public int LevelID { get; set; }
        public string Email { get; set; }
        public LevelViewModel Level { get; set; }

    }
}
