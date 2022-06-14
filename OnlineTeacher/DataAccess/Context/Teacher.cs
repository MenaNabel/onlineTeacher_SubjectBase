using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    [Index(nameof(UserID))]
    public class Teacher
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string SecretarialPhoneNumber { get; set; }
        public string VodafonCachPhoneNumber { get; set; }
        public string ActivationSubscriptionPhoneNumber { get; set; }
        public string FacebookLink { get; set; }
        public string WhatsappLink { get; set; }
        public string TelegramLink { get; set; }
        public string UserID { get; set; }
        public byte[] PersonalImage { get; set; }
        public string File { get; set; }
    }
}
