USE [PharmaAppDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserLogin](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[LogInTime] [datetime] NOT NULL,
	[LogOutTime] [datetime] NULL,
 CONSTRAINT [PK_UserLogin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserLogin] ADD  CONSTRAINT [DF_UserLogin_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[UserLogin] ADD  CONSTRAINT [DF_UserLogin_LogInTime]  DEFAULT (getdate()) FOR [LogInTime]
GO

ALTER TABLE [dbo].[UserLogin]  WITH CHECK ADD  CONSTRAINT [FK_UserLogin_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[UserLogin] CHECK CONSTRAINT [FK_UserLogin_User]
GO
