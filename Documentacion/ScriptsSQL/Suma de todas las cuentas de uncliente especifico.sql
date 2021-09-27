use stefaniniTest;

select SUM(acc.account_balance) from accounts as acc
where acc.account_client_owner_id = 1082870533;
