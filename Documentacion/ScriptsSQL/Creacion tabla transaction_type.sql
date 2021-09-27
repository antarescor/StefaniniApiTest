USE [stefaniniTest]
GO

/****** Object:  Table [dbo].[transactions_type]    Script Date: 27/09/2021 6:03:25 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[transactions_type](
	[transaction_type_id] [int] IDENTITY(1,1) NOT NULL,
	[transaction_description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_transactions_type] PRIMARY KEY CLUSTERED 
(
	[transaction_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

