using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using OnlineTeacher.DataAccess;
using OnlineTeacher.Services.Lectures;
using OnlineTeacher.Services.Lectures.Helper;
using OnlineTeacher.Services.Reviews;
using OnlineTeacher.Services.Reviews.Helper;
using OnlineTeacher.Services.Exams;
using OnlineTeacher.Services.Exams.Helper;
using OnlineTeacher.Services.Questions;
using OnlineTeacher.Services.Questions.Helper;
using OnlineTeacher.Services.Subjects;
using OnlineTeacher.Services.Subjects.Helper;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Maaping;
using OnlineTeacher.Shared.Services;
using Threenine.Data;
using OnlineTeacher.Services.Levels.Helper;
using OnlineTeacher.Services.Levels;
using OnlineTeacher.Services.Teachers.Helper;
using OnlineTeacher.Services.Teachers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using OnlineTeacher.Services.Students.Helper;
using OnlineTeacher.Services.Students;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.DataAccess.Repository.CustomeRepository;
using OnlineTeacher.Services.Subscriptions.Helper;
using OnlineTeacher.Services.Subscriptions;
using OnlineTeacher.Services.StudentExams.Helper;
using OnlineTeacher.Services.StudentExams;
using OnlineTeacher.Shared.Services.Emails;
using OnlineTeacher.Services.Home;
using OnlineTeacher.Services.Home.Helper;
using OnlineTeacher.Services.Lectures.Refactoring;
using OnlineTeacher.DataAccess.Repository.CustomeRepository.Lectures;

namespace OnlineTeacher
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        
            // config the identity user schema and connect to it 
            services.AddDbContext<OnlineExamContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // config the  identity user and Role
            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                // config user password validation 
                option.Password.RequireDigit = true; // password must have at least digit
                option.Password.RequireLowercase = true;// password must have at least one lower case alhpabitic
                option.Password.RequireUppercase = false;  //password not must contain upercase 
                option.Password.RequiredLength = 8;// password must have at least 8 charcter 
                option.Password.RequireNonAlphanumeric = false; // password not must contain non alpha numeric charchter 
                option.User.RequireUniqueEmail = true; // requeired Uniqe Email 
               

            }).AddEntityFrameworkStores<OnlineExamContext>() // add schema to DB
            .AddDefaultTokenProviders(); // add token provider that use to confirmation (email - phone) and two factor Login


            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            { // config authontication by JWT more info can visit https://jwt.io/introduction
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true, // valied to lissen to this end point
                   ValidateAudience = true,
                    ValidIssuer = Configuration["AuthSetting:Issuer"],
                    ValidAudience= Configuration["AuthSetting:Audience"],
                    RequireExpirationTime = true, // must define specific time to expire the token after Login
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSetting:Key"])), // Key  that use to hash 
                    ValidateIssuerSigningKey = true,
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Roles.Student,
                   authBuilder =>
                   {
                       authBuilder.RequireRole(Roles.Student
                           );
                   });
                options.AddPolicy(Roles.Admin,
                    authBuilder =>
                    {
                        authBuilder.RequireRole(Roles.Admin);
                    });
                options.AddPolicy(Roles.Admin+","+Roles.Student,
                    authBuilder =>
                    {
                        authBuilder.RequireRole(Roles.Admin, Roles.Student);
                    });
                options.AddPolicy(Roles.Student+","+Roles.Admin,
                    authBuilder =>
                    {
                        authBuilder.RequireRole(Roles.Admin , Roles.Student);
                    });
               

            });





            services.AddTransient<ISubject , SubjectServices>();
            services.AddTransient<ISubjectAsync, SubjectServicesAsync>();


            services.AddTransient<IEmailSender, MailKitEmailSender>();
            services.Configure<MailKitEmailSenderOptions>(options =>
            {
                options.Host_Address = Configuration["ExternalProviders:MailKit:SMTP:Address"];
                options.Host_Port = Convert.ToInt32(Configuration["ExternalProviders:MailKit:SMTP:Port"]);
                options.Host_Username = Configuration["ExternalProviders:MailKit:SMTP:Account"];
                options.Host_Password = Configuration["ExternalProviders:MailKit:SMTP:Password"];
                options.Sender_EMail = Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
                options.Sender_Name = Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
            });

            services.AddTransient<IReviewAsync, ReviewServicesAsync>();
            services.AddTransient<IFileImageUploading, FileImage>(); 
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
          


            services.AddTransient<IFileImageUploading, FileImage>();
            services.AddTransient<ISudentLectureService, StudyLectureServicesAsync>();
            services.AddTransient<IExamAsync, ExamServicesAsync>();
            services.AddTransient<IExamingServicesAsync, ExamServicesAsync>();
            services.AddTransient<IStudentExamServiceAsync, StudentExamServiceAsync>();
            services.AddTransient<ILevelAsync, LevelServicesAsync>();
            services.AddTransient<IQuestionAsync, QuestionServiceAsync>();
            services.AddTransient<IAssign, AssignServices>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<ISettingAsync, SettingsAsync>();
            services.AddTransient<IStudentAsync, StudentServicesAsync>();
            services.AddTransient<ISubscriptionReposiory, SubscribitionRepository>();
            services.AddTransient<ISubscribtion, SubscrbitionServicesAsync>();
            services.AddTransient<ISiteInfo, SiteInfoServicesAsync>();
            services.AddTransient<IHonerList, HonerListServicesAsync>();
            services.AddTransient<ILectureServices, LectureServices>();
            services.AddTransient<ILectureRepo, LecturesRepoAsyncCustom>();
            
            #region StudyLecture
            services.AddTransient<IStudyLecture, StudyLectureServices>();
            services.AddTransient<IStudyLectureAsync, StudyLectureServicesAsync>();
            services.AddTransient<ILectureServicesForStudent, LectureServicesForStudent>();
            #endregion

            #region OnlineLecture
            services.AddTransient<IOnlineLecture, OnlineLectureServices>();
            services.AddTransient<IOnlineLectureAsync, OnlineLectureServicesAsync>();
            #endregion

            // Auto Mapper Configurations
            #region Auto Mapper
            var mapperConfig = new MapperConfiguration(mc =>
              {
                  mc.AddProfile(new Mapping());
              });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion
            

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient(typeof(IDeleteRepository<>), typeof(DeleteRepository<>));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineTeacher", Version = "v1" });
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3001", "http://localhost:3000", "https://petersalamamath.com", "https://admin.petersalamamath.com")
                        .AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "370218926467-27bb9dmk2l6l5scn5p81oq6kk1bcdajt.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineTeacher v1"));
            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
         
        }
    }
}