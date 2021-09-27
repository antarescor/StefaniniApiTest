USE [stefaniniTest]
GO

/****** Object:  Table [dbo].[transactions]    Script Date: 27/09/2021 6:03:01 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[transactions](
	[transaction_id] [int] IDENTITY(1,1) NOT NULL,
	[transaction_date] [datetime] NOT NULL,
	[transaction_type] [int] NOT NULL,
	[transaction_origin_account_id] [int] NOT NULL,
	[transaction_target_account_id] [int] NULL,
	[transactions_description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_transactions] PRIMARY KEY CLUSTERED 
(
	[transaction_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[transactions]  WITH CHECK ADD  CONSTRAINT [FK_transaction_target_account] FOREIGN KEY([transaction_target_account_id])
REFERENCES [dbo].[accounts] ([account_number])
GO

ALTER TABLE [dbo].[transactions] CHECK CONSTRAINT [FK_transaction_target_account]
GO

ALTER TABLE [dbo].[transactions]  WITH CHECK ADD  CONSTRAINT [FK_transactions_origin_accounts_id] FOREIGN KEY([transaction_origin_account_id])
REFERENCES [dbo].[accounts] ([account_number])
GO

ALTER TABLE [dbo].[transactions] CHECK CONSTRAINT [FK_transactions_origin_accounts_id]
GO

ALTER TABLE [dbo].[transactions]  WITH CHECK ADD  CONSTRAINT [FK_transactions_transactions_type] FOREIGN KEY([transaction_type])
REFERENCES [dbo].[transactions_type] ([transaction_type_id])
GO

ALTER TABLE [dbo].[transactions] CHECK CONSTRAINT [FK_transactions_transactions_type]
GO

