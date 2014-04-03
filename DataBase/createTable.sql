USE [OrmDemoDB]
GO

/****** Object:  Table [dbo].[U_Student]    Script Date: 11/18/2013 11:40:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[U_Student](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[U_Name] [varchar](50) NULL,
	[U_Age] [int] NULL,
	[U_Address] [varchar](100) NULL,
	[U_Gender] [varchar](10) NULL,
	[U_CreateTime] [datetime] NULL,
 CONSTRAINT [PK_Student_1] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


