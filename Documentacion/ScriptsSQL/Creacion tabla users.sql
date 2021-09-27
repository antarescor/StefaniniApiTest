USE [stefaniniTest]
GO

/****** Object:  Table [dbo].[users]    Script Date: 27/09/2021 6:03:45 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[users](
	[user_id] [int] NOT NULL,
	[user_name] [nvarchar](50) NOT NULL,
	[user_type] [tinyint] NOT NULL,
	[user_pass] [nvarchar](50) NOT NULL,
	[user_state] [tinyint] NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

