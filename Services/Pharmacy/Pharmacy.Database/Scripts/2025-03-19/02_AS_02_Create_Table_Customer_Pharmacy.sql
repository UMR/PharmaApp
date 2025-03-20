USE [PharmaAppDB]
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer_Pharmacy](
	[Id] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[PharmacyId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,	
 CONSTRAINT [PK_Customer_Pharmacy] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[Customer_Pharmacy] ADD  CONSTRAINT [DF_Customer_Pharmacy_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Customer_Pharmacy] WITH CHECK ADD CONSTRAINT [FK_Customer_Pharmacy_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO

ALTER TABLE [dbo].[Customer_Pharmacy] WITH CHECK ADD CONSTRAINT [FK_Customer_Pharmacy_Pharmacy] FOREIGN KEY([PharmacyId])
REFERENCES [dbo].[Pharmacy] ([Id])
GO
