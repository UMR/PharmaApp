USE [PharmaAppDB]
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE [dbo].[Package](
	[Id] [uniqueidentifier] NOT NULL,		
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[Price] [money] NOT NULL,
	[CurrencyCode] [nvarchar](5) NOT NULL,
	[CommissionInPercent] [decimal] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetimeoffset] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[UpdatedDate] [datetimeoffset] NULL
 CONSTRAINT [PK_Package] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Package] ADD CONSTRAINT [DF_Package_Id] DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Package] ADD CONSTRAINT [DF_Package_CreatedDate] DEFAULT (GETUTCDATE()) FOR [CreatedDate]
GO
