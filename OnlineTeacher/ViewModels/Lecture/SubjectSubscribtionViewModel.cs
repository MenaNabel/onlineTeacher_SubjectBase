using OnlineTeacher.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Lecture
{
    public class SubjectSubscribtionViewModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
    }
}
