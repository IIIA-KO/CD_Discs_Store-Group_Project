USE [CDDiscsGroupProject]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](100) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[ContactPhone] [nvarchar](20) NOT NULL,
	[ContactMail] [nvarchar](100) NOT NULL,
	[BirthDay] [date] NOT NULL,
	[MarriedStatus] [bit] NULL,
	[Sex] [bit] NULL,
	[HasChild] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Disc]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Disc](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Price] [decimal](9, 2) NOT NULL,
	[LeftOnStock] [int] NOT NULL,
	[Rating] [decimal](18, 0) NOT NULL,
	[CoverImagePath] [nvarchar](250) NULL,
	[ImageStorageName] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiscFilm]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiscFilm](
	[IdDisc] [uniqueidentifier] NOT NULL,
	[IdFilm] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiscMusic]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiscMusic](
	[IdDisc] [uniqueidentifier] NOT NULL,
	[IdMusic] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Film]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Film](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Genre] [nvarchar](50) NOT NULL,
	[Producer] [nvarchar](50) NOT NULL,
	[MainRole] [nvarchar](50) NOT NULL,
	[AgeLimit] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MailList]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailList](
	[Id] [uniqueidentifier] NOT NULL,
	[Mail] [nvarchar](100) NOT NULL,
	[IdClient] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Music]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Music](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Genre] [nvarchar](50) NOT NULL,
	[Artist] [nvarchar](50) NOT NULL,
	[Language] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperationLog]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperationLog](
	[Id] [uniqueidentifier] NOT NULL,
	[OperationType] [uniqueidentifier] NOT NULL,
	[OperationDateTimeStart] [datetime] NOT NULL,
	[OperationDateTimeEnd] [datetime] NULL,
	[IdOrder] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperationType]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperationType](
	[Id] [uniqueidentifier] NOT NULL,
	[TypeName] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [uniqueidentifier] NOT NULL,
	[OrderDateTime] [datetime] NOT NULL,
	[IdClient] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItem](
	[Id] [uniqueidentifier] NOT NULL,
	[IdOrder] [uniqueidentifier] NOT NULL,
	[IdDisc] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhoneList]    Script Date: 10.01.2024 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneList](
	[Id] [uniqueidentifier] NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[IdClient] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Client] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Disc] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Disc] ADD  DEFAULT ((0)) FOR [LeftOnStock]
GO
ALTER TABLE [dbo].[Disc] ADD  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[Disc] ADD  DEFAULT ('') FOR [CoverImagePath]
GO
ALTER TABLE [dbo].[Disc] ADD  DEFAULT ('') FOR [ImageStorageName]
GO
ALTER TABLE [dbo].[Film] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[MailList] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Music] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[OperationLog] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[OperationType] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[OrderItem] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PhoneList] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[DiscFilm]  WITH CHECK ADD FOREIGN KEY([IdDisc])
REFERENCES [dbo].[Disc] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DiscFilm]  WITH CHECK ADD FOREIGN KEY([IdFilm])
REFERENCES [dbo].[Film] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DiscMusic]  WITH CHECK ADD FOREIGN KEY([IdDisc])
REFERENCES [dbo].[Disc] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DiscMusic]  WITH CHECK ADD FOREIGN KEY([IdMusic])
REFERENCES [dbo].[Music] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MailList]  WITH CHECK ADD FOREIGN KEY([IdClient])
REFERENCES [dbo].[Client] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OperationLog]  WITH CHECK ADD FOREIGN KEY([IdOrder])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OperationLog]  WITH CHECK ADD FOREIGN KEY([OperationType])
REFERENCES [dbo].[OperationType] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([IdClient])
REFERENCES [dbo].[Client] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD FOREIGN KEY([IdDisc])
REFERENCES [dbo].[Disc] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD FOREIGN KEY([IdOrder])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PhoneList]  WITH CHECK ADD FOREIGN KEY([IdClient])
REFERENCES [dbo].[Client] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Film]  WITH CHECK ADD CHECK  (([AgeLimit]>=(0)))
GO
