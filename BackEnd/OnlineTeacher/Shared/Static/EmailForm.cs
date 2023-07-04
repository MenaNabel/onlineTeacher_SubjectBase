using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Static
{
    public static class EmailForm
    {/// <summary>
     /// draw templeate for emails
     /// </summary>
     /// <param name="emailResonsability">
     /// that email send for what ? like Active Email or Forget Email
     /// </param>
     /// <param name="url">
     /// Url that added in click button to return : like reset password 
     /// </param>
     /// <param name="ActionDescribiton">
     /// describiton added in click button to return  
     /// </param>
     /// <returns></returns>
        public static string Draw(string emailResonsability , string ActionDescribiton,  string url) {
          string  template  = $"<h1> Welcome To Sigma </h1> " +
                              $"<h2> {emailResonsability}</h2>" +
                              $"<p>To {ActionDescribiton} <a href='{url}'> Click Here </a> </p> ";
            return template;
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="emailResonsability">
         /// that email send for what ? like Active Email or Forget Email
         /// </param>
         /// <param name="info">
         /// info added to emails
         /// </param>
         /// <returns></returns>
        public static string Draw(string emailResonsability , string info ) {
          string  template  = $"<h1> Welcome To Sigma </h1> " +
                              $"<h2> {emailResonsability}</h2>" +
                              $"<p> {info} </p> ";
            return template;
        }
    }
}
