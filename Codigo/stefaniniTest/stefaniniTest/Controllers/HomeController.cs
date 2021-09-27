using Newtonsoft.Json;
using stefaniniTest.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace stefaniniTest.Controllers
{
    public class HomeController : Controller
    {
        private string UriBase = "https://localhost:44331/";


        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Home Page";

            HttpClient client = new HttpClient();
            var json = await client.GetStringAsync(UriBase+"api/GetAllUser/");

            List<UsersViewModel> userList = JsonConvert.DeserializeObject<List<UsersViewModel>>(json);

            ViewBag.userList = userList;
            return View();
        }


        public async Task<ActionResult> TransactionView(int user_id)
        {

            ViewBag.Title = "Home Page";

            HttpClient client = new HttpClient();
            var json = await client.GetStringAsync(UriBase+ "api/GetUser/?user_id="+user_id);

            List<UsersViewModel> userList = JsonConvert.DeserializeObject<List<UsersViewModel>>(json);
            var user = userList[0];

            var json2 = await client.GetStringAsync(UriBase + "api/GetAllBanks/");
            List<UsersViewModel> bankList = JsonConvert.DeserializeObject<List<UsersViewModel>>(json2);

            var json3 = await client.GetStringAsync(UriBase + "api/GetAllAccountsByUser/?user_id=" + user_id);
            List<AccountsViewModel> userCounts = JsonConvert.DeserializeObject<List<AccountsViewModel>>(json3);

            TransactionFormViewModel transactionForm = new TransactionFormViewModel();
            transactionForm.bank_list = bankList;
            transactionForm.amount = 0;
            transactionForm.user_type = user.user_type;
            transactionForm.origin_account_list = userCounts;

            ViewBag.user = user;
            ViewBag.bank_list = bankList;
            return View(transactionForm);
        }

        public async Task<ActionResult> userAccounts(int user_id)
        {
            HttpClient client = new HttpClient();            

            var json = await client.GetStringAsync(UriBase + "api/GetAllAccountsByUser/?user_id=" + user_id);
            List<AccountsViewModel> userCounts = JsonConvert.DeserializeObject<List<AccountsViewModel>>(json);

            ViewBag.userAccountList = userCounts;
            return View();
        }

        public async Task<ActionResult> historicalTransaction(int account_id)
        {
            HttpClient client = new HttpClient();

            var json = await client.GetStringAsync(UriBase + "api/GetHistoricalByAccount/?account_id=" + account_id);
            List<TransactionViewModel> historilcalTransactionList = JsonConvert.DeserializeObject<List<TransactionViewModel>>(json);

            ViewBag.userAccountList = historilcalTransactionList;
            return View();
        }

    }
}
