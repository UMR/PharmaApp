USE [PharmaAppDB]
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer](
	[Id] [uniqueidentifier] NOT NULL,	
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[Mobile] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[Age] [smallint] NOT NULL,
	[Weight] [float](24) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[Customer] ADD CONSTRAINT [DF_Customer_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO