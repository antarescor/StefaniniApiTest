using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stefaniniTest.ViewModels
{
    public class UsersViewModel
    {

        [Display(Name = "Identificacaciond de usuario")]
        public int user_id { get; set; }

        [Display(Name = "Nombre  de usuario")]
        public string nombre { get; set; }

        [Display(Name = "Nombre  de usuario")]
        public int user_type { get; set; }

    }
}