
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnlineTeacher.DataAccess;
using OnlineTeacher.Services.Students.Helper;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Services.Emails;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.Shared.ViewModel;
using OnlineTeacher.ViewModels.Identity.Login;
using OnlineTeacher.ViewModels.Identity.Regiesteration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Services
{
    public class UserServices : IUserServices
    {
        private IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManger;
        private IConfiguration _config;
        private IServiceProvider _services;
        private IStudentAsync _Student;
        private IEmailSender _emailSender;
        private INetwork _NetworkHandeler;
        public UserServices(

            UserManager<ApplicationUser> userManger,
            IConfiguration config,
            IServiceProvider serviceProvider,
            IEmailSender emailSender 
           
            )
        {

            _userManger = userManger;
            _config = config;
            _services = serviceProvider;
            _emailSender = emailSender;
           

        }

        #region  User Data Services
        public  int GetStudentID()
        {
            _httpContextAccessor = GetHttpContextAccessor();
            var St = _httpContextAccessor.HttpContext?.User.FindFirstValue(CustomeClaim.StudentID);
            if (string.IsNullOrEmpty(St) || St == "0")
            {

                _Student = _services.GetRequiredService<IStudentAsync>();
                var student = _Student.GetAsyncWithoutValidate(GetCurrentUserID());
                student.Wait();
                if (student is null)
                    return 0;
                return student.Result.ID;
            }
            return int.Parse(St);

        }
        public string GetCurrentUserID()
        {
            _httpContextAccessor = GetHttpContextAccessor();
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public string GetCurrentUserName()
        {
            _httpContextAccessor = GetHttpContextAccessor();
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        }
        public string GetCurrentUserRole()
        {
            _httpContextAccessor = GetHttpContextAccessor();
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
        }
        private IHttpContextAccessor GetHttpContextAccessor()
        {
            return _services.GetRequiredService<IHttpContextAccessor>();
        }
        
        private NetworkViewMode GetVisitorIp()
        {
          
            _NetworkHandeler =  _services.GetRequiredService<INetwork>();

            return _NetworkHandeler.GetVisitorIp();

        }
        public NetworkViewMode GetVisitorIp(HttpContext context)
        {

            _NetworkHandeler =  _services.GetRequiredService<INetwork>();

            return _NetworkHandeler.GetVisitorIp(context);

        }


        #endregion
        [Authorize(Roles =Roles.Admin)]
        public async Task<UserMangerResonse> ChangeIP(ChangeIpViewModel model)
        {
            var user = await GetUser(model.Email);
            if (user is null)
                return new UserMangerResonse("Invalid email", false);

            if (user.DeleteIp())
                
                if (await Update(user))
                    return new UserMangerResonse("changed successfuly", true);

                else
                    return new UserMangerResonse("not updated successfuly", true);

            return new UserMangerResonse("not deleted successfuly", false);


        }
        public async Task<UserMangerResonse> RegiesterUserAsync(RegiesterViewModel model)
        {
            // if model not have value throw exception
            if (model is null)
                throw new NullReferenceException("Regiester model is null");

            // check that password and confirm pass match
            if (model.Password != model.ConfirmPassword)
                return new UserMangerResonse
                {
                    IsSuccess = false,
                    Message = "تأكيد كلمة المرور مختلفة عن كلمة المرور, تأكد من إدخالهم بشكل صحيح",
                };

            // create new User Identity
            var IdentityUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email + model.Name,
                Name = model.Name// make user name is email comapct with name
            };
            var Result = await _userManger.CreateAsync(IdentityUser, model.Password);
            
            if (Result.Succeeded)
            {
                //var ConfigirationEmailToken = await _userManger.GenerateEmailConfirmationTokenAsync(IdentityUser);
                //var UserEncodingToken = Encoding.UTF8.GetBytes(ConfigirationEmailToken);
                //var validateEmailToken = WebEncoders.Base64UrlEncode(UserEncodingToken);

                //var Url = $"{_config["AppUrl"]}/api/auth/confirmEmail?userID={IdentityUser.Id}&token={validateEmailToken}";
                //await _emailSender.SendEmailAsync(model.Email, "Active Email", EmailForm.Draw("Active Email", "Active Email", Url));


                if (!await AssignToRoleStudent(IdentityUser))
                {

                    return new UserMangerResonse("user is  created but not assign to role and student not created", false);
                }

                if (!await CreateStudentAsync(model,IdentityUser.Id))
                    return new UserMangerResonse("user is  created but student not", false);

              

                return new UserMangerResonse("user is  created successfuly", true);


            }

            return new UserMangerResonse
            {
                Message = "user is not created",
                IsSuccess = false,
                Errors = Result.Errors.Select(e => e.Description)
            };

        }


        public async Task<UserMangerResonse> LoginUserAsync(LoginViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException("Login model can't be null");

            var user = await GetUser(model.Email);
            // if not found return with errors and not vailed login
            if (user is null)
                return new UserMangerResonse("Invalid email or password ", false);


            if (!await IsValiedLogin(user, model.Password))
                return new UserMangerResonse("Invalid email or password ", false);

            //if(!user.EmailConfirmed)
            //    return new UserMangerResonse("Email Not Confirmed", false);

            //#region Validation IPs

           
            //NetworkViewMode NetworkInfo = GetVisitorIp();
            //if (!user.IsAssignedIp(NetworkInfo))
            //{
            //    if (!user.Assign(NetworkInfo))
            //        return new UserMangerResonse("Invalid Ip", false);
            //    if (!await Update(user))
            //    {
            //        return new UserMangerResonse("have a problem please try again", false);
            //    }
            //}


            //#endregion

            // get user Role
            var Role = await GetRoleAsync(user);
            var _Student = _services.GetRequiredService<IStudentAsync>();
            var Student = await _Student.GetAsyncWithoutValidate(user.Id);
            

            // Get Security Key From Setting
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthSetting:Key"]));

            var AccessToken = GenerateAccessToken(user, Key, Role,Student?.ID);
            // convert token to String 
            var TokenAsString = new JwtSecurityTokenHandler().WriteToken(AccessToken);
            return new UserMangerResonse(TokenAsString, true, AccessToken.ValidTo);
            
        }
        public async Task<UserMangerResonse> GoogleLoginUserAsync(GoogleLoginViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException("Login model can't be null");

            var user = await GetUser(model.Email);
            // if not found return with errors and not vailed login
            if (user is null)
                return new UserMangerResonse("Invalid email or password ", false);


            //if(!user.EmailConfirmed)
            //    return new UserMangerResonse("Email Not Confirmed", false);

            // get user Role
            var Role = await GetRoleAsync(user);
            var _Student = _services.GetRequiredService<IStudentAsync>();
            var Student = await _Student.GetAsyncWithoutValidate(user.Id);


            // Get Security Key From Setting
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthSetting:Key"]));

            var AccessToken = GenerateAccessToken(user, Key, Role, Student?.ID);
            // convert token to String 
            var TokenAsString = new JwtSecurityTokenHandler().WriteToken(AccessToken);
            return new UserMangerResonse(TokenAsString, true, AccessToken.ValidTo);

        }

        public async Task<UserMangerResonse> GoogleRegiesterUserAsync(GoogleLoginViewModel model)
        {
            // if model not have value throw exception
            if (model is null)
                throw new NullReferenceException("Regiester model is null");

            // create new User Identity
            var IdentityUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name// make user name is email comapct with name
            };
            var Result = await _userManger.CreateAsync(IdentityUser);

            if (Result.Succeeded)
            {
                //var ConfigirationEmailToken = await _userManger.GenerateEmailConfirmationTokenAsync(IdentityUser);
                //var UserEncodingToken = Encoding.UTF8.GetBytes(ConfigirationEmailToken);
                //var validateEmailToken = WebEncoders.Base64UrlEncode(UserEncodingToken);

                //var Url = $"{_config["AppUrl"]}/api/auth/confirmEmail?userID={IdentityUser.Id}&token={validateEmailToken}";
                //await _emailSender.SendEmailAsync(model.Email, "Active Email", EmailForm.Draw("Active Email", "Active Email", Url));


                if (!await AssignToRoleStudent(IdentityUser))
                {

                    return new UserMangerResonse("user is  created but not assign to role and student not created", false);
                }

                _Student = _services.GetRequiredService<IStudentAsync>();
                var student = new ViewModels.Students.AddedStudentViewModel(model.Name,null, IdentityUser.Id, 1, model.Email);



                var AddedStudnet = await _Student.Add(student);

                if (!(AddedStudnet is not null))
                    return new UserMangerResonse("user is  created but student not", false);



                return new UserMangerResonse("user is  created successfuly", true);
            }

            return new UserMangerResonse
            {
                Message = "user is not created",
                IsSuccess = false,
                Errors = Result.Errors.Select(e => e.Description)
            };

        }
        public async Task<UserMangerResonse> ConfirmEmailAsync(string userID, string Token)
        {

            var user = await _userManger.FindByIdAsync(userID);
            if (user == null)
                return new UserMangerResonse("User Not Found", false);

            var decodedToken = WebEncoders.Base64UrlDecode(Token);
            var normalToken = Encoding.UTF8.GetString(decodedToken);
            var result = await _userManger.ConfirmEmailAsync(user , normalToken);
            if (result.Succeeded)
                return new UserMangerResonse("Confirm Successfuly", true);

            var response = new UserMangerResonse("Confirm Successfuly", false);
            response.Errors = result.Errors.Select(e => e.Description);
            return response;
           
        }
        public async Task<UserMangerResonse> ForgetPasswordAsync(string email) 
        {

            var user = await _userManger.FindByEmailAsync(email);
            if (user is null)
                return new UserMangerResonse("Not Find Email", false);
            var token = await _userManger.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var ValidTaken = WebEncoders.Base64UrlEncode(encodedToken);

            //var Url = $"{_config["AppUrl"]}/api/auth/ResetPassword?email={email}&token={ValidTaken}";
            var Url = $"https://localhost:3000/ResetPassword?email={email}&token={ValidTaken}";
            await _emailSender.SendEmailAsync(user.Email, "Forget Password", EmailForm.Draw("Forget Password", " Reset Password ", Url));

            return new UserMangerResonse("Reset Password Url has been send to the Email Successfuly", true);

        }


        public async Task<UserMangerResonse> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserMangerResonse
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            if (model.NewPassword != model.ConfirmPassword)
                return new UserMangerResonse
                {
                    IsSuccess = false,
                    Message = "Password doesn't match its confirmation",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManger.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
                return new UserMangerResonse
                {
                    Message = "Password has been reset successfully!",
                    IsSuccess = true,
                };

            return new UserMangerResonse
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        public async Task<UserMangerResonse> changePasswordAsync(ChangePasswordViewModel model) {
           var user = await _userManger.FindByIdAsync(model.ID);
            if (user is null)
                return new UserMangerResonse("not found user", false);
            var Result = await _userManger.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if(Result.Succeeded)
                return new UserMangerResonse("user Password change successfuly ", true);
           
            var ResponseWithError = new UserMangerResonse("user Password not  change successfuly ", false);
            ResponseWithError.Errors = Result.Errors.Select(e => e.Description);
            return ResponseWithError;
        }

        #region Helper
        private async Task<IList<string>> GetRoleAsync(ApplicationUser user)
        {

            return await _userManger.GetRolesAsync(user);


        }
        private async Task<ApplicationUser> GetUser(string Email)
        {

            return await _userManger.FindByEmailAsync(Email);
        }
        private async Task<bool> IsValiedLogin(ApplicationUser user, string password)
        {


            return await _userManger.CheckPasswordAsync(user, password);
        }
        private Claim[] GenerateClaims(ApplicationUser user, IList<string> Role , int? StudentID)
        {
            

            int studentID = StudentID is not null ? int.Parse(StudentID.ToString()) : 0;
            return new[] {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier , user.Id),
                new Claim(ClaimTypes.Role ,  Role.FirstOrDefault()),
                new Claim(ClaimTypes.Name ,  user.Name),
                new Claim(CustomeClaim.StudentID ,  Convert.ToString(studentID)),
               
           };
        }
        private JwtSecurityToken GenerateAccessToken(ApplicationUser user, SymmetricSecurityKey Key, IList<string> Role, int? StudentID)
        {
            // Create Token 
            var Token = new JwtSecurityToken(
                 issuer: _config["AuthSetting:Issuer"],
                 audience: _config["AuthSetting:Audience"],
                 claims: GenerateClaims(user, Role,  StudentID),
                 expires: DateTime.Now.AddDays(3), // this acces will expire after 3 day
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
                 );

            return Token;
        }
        public async Task<string> CreateAdminAdndAssignToRole()
        {
            var RoleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();


            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync(Roles.Admin);
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            ApplicationUser user = await GetUser(AdminData.Email);
            if (user is null)
            {
                var IdentityUser = new ApplicationUser
                {
                    Email = AdminData.Email,
                    UserName = AdminData.Email + AdminData.Name, // make user name is email comapct with name
                    Name = AdminData.Name
                };
                var Result = await _userManger.CreateAsync(IdentityUser, AdminData.password);
                if (!Result.Succeeded)
                    throw new Exception("Admin Regiest have problem");
            }

            var roles = await _userManger.GetRolesAsync(user);
            if (roles.Contains(Roles.Admin))
                return user.Id;
            await _userManger.AddToRoleAsync(user, Roles.Admin);
            return user.Id;

        }

        private async Task<bool> AssignToRoleStudent(ApplicationUser user)
        {
            var RoleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();


            IdentityResult roleResult;
            //Adding student Role
            var roleCheck = await RoleManager.RoleExistsAsync(Roles.Student);
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole(Roles.Student));
            }


            if (user is null)
            {
                return false;
            }

            var identiityRole = await _userManger.AddToRoleAsync(user, Roles.Student);
            return identiityRole.Succeeded;
        }
        private async Task<bool> CreateStudentAsync(RegiesterViewModel model, string UserID)
        {
            _Student = _services.GetRequiredService<IStudentAsync>();
            var student = new ViewModels.Students.AddedStudentViewModel(model.Name, model.PhoneNumber, UserID, model.LevelID, model.Email);
            
            
           
            var AddedStudnet = await _Student.Add(student);
            return AddedStudnet is not null ? true : false;

        }

        public async Task<bool> Update(ApplicationUser user)
        {
          var userUpdated = await  _userManger.FindByIdAsync(user.Id);

            userUpdated.Name = user.Name;
            userUpdated.PhoneNumber = user.PhoneNumber;
            userUpdated.VisitorIP = user.VisitorIP;
            userUpdated.VisitorIP2 = user.VisitorIP2;
            userUpdated.VisitorIpsAssignedNo = user.VisitorIpsAssignedNo;
            

            var Result = await _userManger.UpdateAsync(userUpdated);
            return Result.Succeeded;
        }

        public async Task<UserMangerResonse> DeleteuserByEmail(string email)
        {
         var user  = await   _userManger.FindByEmailAsync(email);
            if (user is null)
                return new UserMangerResonse("Not Found" , false);
          var result = await  _userManger.DeleteAsync(user);
            if(result.Succeeded)
            return new UserMangerResonse("deleted", true);

            var res = new UserMangerResonse("Not deleted", false);
            res.Errors = result.Errors.Select(e => e.Description);
            return res;
        }

        #endregion


    }
}
