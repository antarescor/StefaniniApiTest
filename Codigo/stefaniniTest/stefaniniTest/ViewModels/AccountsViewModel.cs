using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stefaniniTest.ViewModels
{
    public class AccountsViewModel
    {
        [Display(Name = "Numero de cuenta")]
        public int account_number { get; set; }

        [Display(Name = "Descripcion de cuenta y banco")]
        public string account_all_detail{ get; set; }

        public Double blance { get; set; }
    }
}