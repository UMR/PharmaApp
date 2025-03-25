USE [PharmaAppDB]
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PaymentDetails](
	[Id] [uniqueidentifier] NOT NULL,	
	[PharmacyId] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[PackageId] [uniqueidentifier] NOT NULL,
	[PackagePrice] [money] NOT NULL,
	[PackageCommissionInPercent] [decimal] NOT NULL,
	[PackageLastUpdatedOn] [datetimeoffset] NOT NULL,
	[PackageLastUpdatedBy] [uniqueidentifier] NOT NULL,
	[OrderId] [nvarchar](50) NOT NULL,
	[PaymentId] [nvarchar](50) NOT NULL,
	[Signature] [nvarchar](50) NOT NULL,
	[PaymentLog] [nvarchar](max) NOT NULL,
	[PaidAmount] [money] NOT NULL,
	[Discount] [decimal] NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetimeoffset] NOT NULL
 CONSTRAINT [PK_PaymentDetails] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentDetails] ADD CONSTRAINT [DF_PaymentDetails_Id] DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[PaymentDetails] ADD CONSTRAINT [DF_PaymentDetails_CreatedDate] DEFAULT (GETUTCDATE()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[PaymentDetails] WITH CHECK ADD CONSTRAINT [FK_PaymentDetails_Pharmacy] FOREIGN KEY([PharmacyId])
REFERENCES [dbo].[Pharmacy] ([Id])
GO

ALTER TABLE [dbo].[PaymentDetails] WITH CHECK ADD CONSTRAINT [FK_PaymentDetails_Package] FOREIGN KEY([PackageId])
REFERENCES [dbo].[Package] ([Id])
GO

ALTER TABLE [dbo].[PaymentDetails] WITH CHECK ADD CONSTRAINT [FK_PaymentDetails_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO