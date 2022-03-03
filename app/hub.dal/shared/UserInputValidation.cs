using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace hub.dal.shared
{
    public static class UserInputValidation
    {
        //static class & method coz I done want to use the method from multiple classes with creating onew object for each
        public static string GetUserInputWithoutSpecialCharacterAndWhitespaces(string userInput)
        {
            var alphaNumeric = "[^a-zA-Z0-9 ]";
            var searchKeyword = userInput.Trim();
            searchKeyword = Regex.Replace(searchKeyword, alphaNumeric, String.Empty);
            return searchKeyword;
        }
    }
}
