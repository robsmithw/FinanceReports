using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceReporting.Models
{
    /* May need to add another model for register
     * As we will have more required than just username
     * and the password
    */
    public class UserViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
    }
}
