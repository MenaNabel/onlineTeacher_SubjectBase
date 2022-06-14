using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Validation
{
    public class NotSqlInjecttionAttribute : ValidationAttribute
    {
        private List<string> SuggestedInjectionWords = new List<string> 
        {
            "select" ,
             "delete",
            "where",
            "*",
            "set null",
            ";",
            "1=1"
           
        };
        private string valueString = "";
        public override bool IsValid(object value)
        {
             // if value is null return is not valid
            if (value is null)
                return false;
            // convert to string and Lower case
            valueString = Convert.ToString(value).ToLower();
            var IsNotValied = SuggestedInjectionWords.Any(checkWordsInTheList); // check is not contain any value of  list 
                
            if (
                // check is not start with select 
                valueString.StartsWith(SuggestedInjectionWords[0]) ||
                valueString.StartsWith(SuggestedInjectionWords[1]) ||
                IsNotValied
                )
                return false; 
            return true;

        }
        private bool checkWordsInTheList(string word) {

            bool IsFound = valueString.Contains(word);
            return IsFound;
        }
    }
}
