using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stefaniniTest.ViewModels
{
    public class TransactionViewModel
    {
        public int transaction_type { get; set; }

        public int bank_id { get; set; }

        public int? target_account_id { get; set; }

        public int origin_account_id { get; set; }

        public Double amount { get; set; }

        public int user_type { get; set; }
    }
}