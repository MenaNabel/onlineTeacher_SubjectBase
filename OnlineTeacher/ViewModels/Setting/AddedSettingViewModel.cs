using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Setting
{
    public class AddedSettingViewModel
    {
      
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        
        public string Address { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [Phone]
        public string SecretarialPhoneNumber { get; set; }
        [Required]
        [Phone]
        public string VodafonCachPhoneNumber { get; set; }
        [Required]
        public string ActivationSubscriptionPhoneNumber { get; set; }
        [Required]
        [Url]
        public string FacebookLink { get; set; }
        [Required]
        [Url]
        public string WhatsappLink { get; set; }
        [Required]
        [Url]
        public string TelegramLink { get; set; }
        public string FileName { get; set; }
        public IFormFile FormFile { get; set; }
        public string File { get; set; }
    }
}
