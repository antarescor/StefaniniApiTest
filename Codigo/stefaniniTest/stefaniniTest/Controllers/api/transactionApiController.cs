using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using stefaniniTest.Models;
using stefaniniTest.ViewModels;

namespace stefaniniTest.Controllers.api
{
    public class transactionApiController : ApiController
    {
        private stefaniniTestEntities db = new stefaniniTestEntities();


        [HttpGet]
        [Route("api/GetAllBanks/")]
        public IQueryable<UsersViewModel> GetBanks()
        {
            IQueryable<UsersViewModel> listOfBanks = null;

            listOfBanks = db.users.Where(x => x.user_type.Equals(1)).Select(r => new UsersViewModel
            {
                nombre = r.user_name,
                user_id = r.user_id
            });

            return listOfBanks;
        }

        [HttpGet]
        [Route("api/GetAllUser/")]
        public IQueryable<UsersViewModel> GetUsers()
        {
            IQueryable<UsersViewModel> listOfUsers = null;

            listOfUsers = db.users.Select(r => new UsersViewModel
            {
                nombre = r.user_name,
                user_id = r.user_id
            });

            return listOfUsers;
        }

        [HttpGet]
        [Route("api/GetUser/")]
        public IQueryable<UsersViewModel> GetUser(int user_id)
        {
            IQueryable<UsersViewModel> user = null;

            user = db.users.Where(x => x.user_id == user_id).Select(r => new UsersViewModel
            {
                nombre = r.user_name,
                user_id = r.user_id
            });

            return user;
        }

        [HttpGet]
        [Route("api/GetAllAccountsByUser/")]
        public IQueryable<AccountsViewModel> GetAllAccountsByUser(int user_id)
        {
            IQueryable<AccountsViewModel> listOfUserAccounts = null;

            listOfUserAccounts =
                from ac in db.accounts
                join us in db.users on ac.account_bank_id equals us.user_id into result
                from rs in result
                where
                    ac.account_client_owner_id == user_id
                select new AccountsViewModel()
                {
                    account_number = ac.account_number,
                    account_all_detail = "Cuenta # " + ac.account_number + " - (" + ac.account_description + "), " + rs.user_name,
                    blance = ac.account_balance
                };

            return listOfUserAccounts;
        }

        [HttpGet]
        [Route("api/GetHistoricalByAccount/")]
        public IQueryable<TransactionViewModel> GetHistoricalByAccount(int account_id)
        {
            var historicalList = db.transactions.Where(x => x.transaction_origin_account_id == account_id || x.transaction_target_account_id == account_id)
                .Select(r => new TransactionViewModel
                {
                    transaction_id = r.transaction_id,
                    transaction_date = r.transaction_date,
                    transaction_type = r.transaction_type,
                    target_account_id = r.transaction_target_account_id,
                    origin_account_id = r.transaction_origin_account_id,
                    transaction_description_detailed = r.transactions_description
                });
         

            return historicalList;
        }

        [HttpPost]
        [Route("api/setTransaction/")]
        public IHttpActionResult setTransaction(TransactionViewModel transaccion)
        {
            bool result = true;

            switch (transaccion.transaction_type)
            {
                case 1://Retiro en efectivo
                    result = cashWithdrawal(transaccion);
                    break;
                case 2:// Transferencia
                    result = bankTransfer(transaccion);
                    break;
            }


            if (result)
            {
                var myCustomMessage = "Transaccion exitosa";
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, myCustomMessage));
            }
            else
            {
                var myCustomMessage = "La transaccion no pudo realizarse";
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, myCustomMessage));
            }
        }

        private bool cashWithdrawal(TransactionViewModel transaction)
        {
            Double GMF;

            //si es cliente, cobre si no no
            if (transaction.amount >= 9600000)
                // si hay fondos suficientes
                GMF = (transaction.amount * 4) / 1000;
            else
                GMF = -1;

            return cashWithdrawalBD(transaction, GMF); ;
        }

        private bool cashWithdrawalBD(TransactionViewModel transaction, Double GMF)
        {
            bool result;
            transactions transactionRegister = new transactions();
            transactions transactionGMF = new transactions();

            using (DbContextTransaction dbTransaction = db.Database.BeginTransaction())
            {

                Double amountTotal = (GMF == -1 ? 0 : GMF) + transaction.amount;
                bool yesOrNot = fundsAvailable(transaction.origin_account_id, amountTotal);

                if (!yesOrNot)
                    return false;

                try
                {
                    //retirar dinero
                    accounts account = db.accounts.Find(transaction.origin_account_id);
                    account.account_balance -= amountTotal;

                    //guardar transaccion princiapal
                    transactionRegister.transaction_date = DateTime.Now;
                    transactionRegister.transaction_type = transaction.transaction_type;
                    transactionRegister.transaction_origin_account_id = transaction.origin_account_id;
                    transactionRegister.transaction_target_account_id = null;
                    transactionRegister.transactions_description = db.transactions_type.Find(transaction.transaction_type).transaction_description + " (" + transaction.amount + ")";
                    db.transactions.Add(transactionRegister);

                    //cobar GMF
                    if (GMF != -1)
                    {
                        //poner dinero de GMT en banco correspondiente
                        int bank_id = account.account_bank_id;
                        accounts accountBank = db.accounts.Where(x => x.account_client_owner_id == bank_id).FirstOrDefault();
                        accountBank.account_balance += GMF;

                        //guardar transaccion de cobro de GMF
                        transactionGMF.transaction_date = DateTime.Now;
                        transactionGMF.transaction_type = 3;
                        transactionGMF.transaction_origin_account_id = transaction.origin_account_id;
                        transactionGMF.transaction_target_account_id = accountBank.account_number;
                        transactionGMF.transactions_description = db.transactions_type.Find(3).transaction_description + " (" + GMF + ")";
                        db.transactions.Add(transactionGMF);
                    }

                    db.SaveChanges();
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                    //throw;
                }

                if (result)
                    dbTransaction.Commit();
                else
                    dbTransaction.Rollback();
            }
            return result;
        }

        private bool bankTransfer(TransactionViewModel transaction)
        {
            Double GMF;

            //si es cliente, cobre si no no
            if (transaction.user_type == 0)
                //calcular GMF
                GMF = (transaction.amount * 4) / 1000;
            else
                GMF = -1;

            return bankTransferBD(transaction, GMF);

        }

        private bool bankTransferBD(TransactionViewModel transaction, Double GMF)
        {

            accounts accountAux = db.accounts.Find(transaction.target_account_id);
            if (accountAux == null)
                return false;

            if (accountAux.account_bank_id != transaction.bank_id)
                return false;

            bool result;
            transactions transactionRegister = new transactions();
            transactions transactionGMF = new transactions();

            using (DbContextTransaction dbTransaction = db.Database.BeginTransaction())
            {
                Double amountTotal = (GMF == -1 ? 0 : GMF) + transaction.amount;
                bool yesOrNot = fundsAvailable(transaction.origin_account_id, amountTotal);

                if (!yesOrNot)
                    return false;

                try
                {
                    //retirar dinero
                    accounts accountOrigin = db.accounts.Find(transaction.origin_account_id);
                    accounts accountTarget = db.accounts.Find(transaction.target_account_id);
                    accountOrigin.account_balance -= transaction.amount;
                    accountTarget.account_balance += transaction.amount;

                    //guardar transaccion princiapal
                    transactionRegister.transaction_date = DateTime.Now;
                    transactionRegister.transaction_type = transaction.transaction_type;
                    transactionRegister.transaction_origin_account_id = transaction.origin_account_id;
                    transactionRegister.transaction_target_account_id = transaction.target_account_id;
                    transactionRegister.transactions_description = db.transactions_type.Find(transaction.transaction_type).transaction_description + " (" + transaction.amount + ")";
                    db.transactions.Add(transactionRegister);

                    //cobar GMF
                    if (GMF != -1)
                    {
                        //poner dinero de GMT en banco correspondiente
                        int bank_id = accountOrigin.account_bank_id;
                        accounts accountBank = db.accounts.Where(x => x.account_client_owner_id == bank_id).FirstOrDefault();
                        accountOrigin.account_balance -= GMF;
                        accountBank.account_balance += GMF;

                        //guardar transaccion de cobro de GMF
                        transactionGMF.transaction_date = DateTime.Now;
                        transactionGMF.transaction_type = 3;
                        transactionGMF.transaction_origin_account_id = transaction.origin_account_id;
                        transactionGMF.transaction_target_account_id = accountBank.account_number;
                        transactionGMF.transactions_description = db.transactions_type.Find(3).transaction_description + " (" + GMF + ")";
                        db.transactions.Add(transactionGMF);
                    }

                    db.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    result = false;
                    //throw;
                }

                if (result)
                    dbTransaction.Commit();
                else
                    dbTransaction.Rollback();
            }
            return result;
        }

        private bool fundsAvailable(int aconunt_id, Double amount)
        {
            accounts account = db.accounts.Find(aconunt_id);

            if (account.account_balance < amount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
