use stefaniniTest;

select * from transactions as tra
where (tra.transaction_origin_account_id = 1 
		or tra.transaction_target_account_id = 1)
		and tra.transaction_date >= '2021-09-26 20:23:00.000' 
		and tra.transaction_date <= '2021-09-26 23:59:59.999'
		