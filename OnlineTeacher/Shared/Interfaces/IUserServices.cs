using OnlineTeacher.DataAccess;
using OnlineTeacher.Shared.ViewModel;
using OnlineTeacher.ViewModels.Identity.Login;
using OnlineTeacher.ViewModels.Identity.Regiesteration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Interfaces
{
   public interface IUserServices
    {
        #region Services Data
        string GetCurrentUserID();
        string GetCurrentUserName();
        string GetCurrentUserRole();
        int GetStudentID();

        #endregion

        Task<UserMangerResonse> RegiesterUserAsync(RegiesterViewModel model);
        Task<UserMangerResonse> LoginUserAsync(LoginViewModel model);
        Task<UserMangerResonse> GoogleLoginUserAsync(GoogleLoginViewModel model);
        Task<UserMangerResonse> GoogleRegiesterUserAsync(GoogleLoginViewModel model);
        Task<string> CreateAdminAdndAssignToRole();

        Task<bool> Update(ApplicationUser user);
        Task<UserMangerResonse> ConfirmEmailAsync(string userID, string Token);

        Task<UserMangerResonse> DeleteuserByEmail(string email);

        Task<UserMangerResonse> ForgetPasswordAsync(string email);

        Task<UserMangerResonse> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<UserMangerResonse> changePasswordAsync(ChangePasswordViewModel model);
    }
}
