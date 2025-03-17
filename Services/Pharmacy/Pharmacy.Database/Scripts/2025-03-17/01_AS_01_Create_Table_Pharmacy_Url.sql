USE [PharmaAppDB]
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Pharmacy_Url](
	[Id] [bigint] NOT NULL,
	[Url] [nvarchar](20) NOT NULL,
	[PharmacyId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Pharmacy_Url] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[Pharmacy_Url] ADD  CONSTRAINT [DF_Pharmacy_Url_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Pharmacy_Url]  WITH CHECK ADD  CONSTRAINT [FK_Pharmacy_Url_Pharmacy] FOREIGN KEY([PharmacyId])
REFERENCES [dbo].[Pharmacy] ([Id])
GO
