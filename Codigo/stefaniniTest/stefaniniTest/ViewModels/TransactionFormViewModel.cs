using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stefaniniTest.ViewModels
{
    public class TransactionFormViewModel
    {
        [Required]
        [Display(Name = "tipo de transaccion")]
        public int transaction_type { get; set; }

        //[Required]
        //[Display(Name = "tipo de transaccion")]
        //public int bank_id { get; set; }

        [Required]
        [Display(Name = "tipo de transaccion")]
        public List<TransactionViewModel> transaction_list{ get; set; }

        [Required]
        [Display(Name = "Cuenta de origen")]
        public List<AccountsViewModel> origin_account_list { get; set; }

        [Required]
        [Display(Name = "Seleccione banco de destino")]
        public List<UsersViewModel> bank_list { get; set; }

        [Display(Name = "Cuenta destino")]
        public int? target_account_id { get; set; }

        [Required]
        [Display(Name = "Cuenta de origen")]
        public int origin_account_id { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public Double amount { get; set; }

        [Required]
        [Display(Name = "Tipo de usuario")]
        public int user_type { get; set; }
    }
}