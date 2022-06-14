using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace OnlineTeacher.DataAccess.Context
{
    [Index(nameof(Month), nameof(Type))]
    public class Lecture
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LectureLink { get; set; }
        public string FileName { get;  set; }
        public string Description { get; set; }
        public int Month { get; set; }
        public string Type { get; set; }
        public bool IsAppear { get; set; }
        public bool IsFree { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey(nameof(Subject))]
        public int SubjectID { get; set; }
        [ForeignKey(nameof(previousLecture))]
        public int? LectureID { get; set; }
        public Subject Subject { get; set; }       
        public Lecture previousLecture { get; set; }
        public LectureDetails File { get; set; }
    }
}