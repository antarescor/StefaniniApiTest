USE [stefaniniTest]
GO

/****** Object:  Table [dbo].[accounts]    Script Date: 27/09/2021 6:01:38 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[accounts](
	[account_number] [int] IDENTITY(1,1) NOT NULL,
	[account_bank_id] [int] NOT NULL,
	[account_client_owner_id] [int] NOT NULL,
	[account_balance] [float] NOT NULL,
	[account_state] [tinyint] NOT NULL,
	[account_description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_accounts] PRIMARY KEY CLUSTERED 
(
	[account_number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[accounts]  WITH CHECK ADD  CONSTRAINT [FK_account_client_owner_id] FOREIGN KEY([account_client_owner_id])
REFERENCES [dbo].[users] ([user_id])
GO

ALTER TABLE [dbo].[accounts] CHECK CONSTRAINT [FK_account_client_owner_id]
GO

ALTER TABLE [dbo].[accounts]  WITH CHECK ADD  CONSTRAINT [FK_acount_bank_id] FOREIGN KEY([account_bank_id])
REFERENCES [dbo].[users] ([user_id])
GO

ALTER TABLE [dbo].[accounts] CHECK CONSTRAINT [FK_acount_bank_id]
GO


