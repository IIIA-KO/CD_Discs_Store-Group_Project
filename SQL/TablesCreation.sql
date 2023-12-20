CREATE DATABASE CDDiscsGroupProject

USE CDDiscsGroupProject

GO
CREATE TABLE Client
(
    Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    [Address] NVARCHAR(100) NOT NULL,
    City NVARCHAR(50) NOT NULL,
	ContactPhone NVARCHAR(20) NOT NULL,
	ContactMail NVARCHAR(100) NOT NULL,
	BirthDay DATE NOT NULL,
	MarriedStatus BIT,
	Sex BIT,
	HasChild BIT
)

GO
CREATE TABLE PhoneList
(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
	Phone NVARCHAR(20) NOT NULL,
	IdClient UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Client(Id) ON DELETE CASCADE NOT NULL
)

GO
CREATE TABLE MailList
(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
	Mail NVARCHAR(100) NOT NULL,
	IdClient UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Client(Id) ON DELETE CASCADE NOT NULL
)

GO
CREATE TABLE Music
(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	Genre NVARCHAR(50) NOT NULL,
	Artist NVARCHAR(50) NOT NULL,
	[Language] NVARCHAR(50) NOT NULL
)

GO
CREATE TABLE Disc
(	
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	Price DECIMAL(9, 2) NOT NULL
)

GO
CREATE TABLE Film
(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	Genre NVARCHAR(50) NOT NULL,
	Producer NVARCHAR(50) NOT NULL,
	MainRole NVARCHAR(50) NOT NULL,
	AgeLimit INT CHECK(AgeLimit >= 0) NOT NULL
)

GO
CREATE TABLE DiscMusic
(
	IdDisc UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Disc(Id) ON DELETE CASCADE NOT NULL, 
	IdMusic UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Music(Id) ON DELETE CASCADE NOT NULL
)

GO
CREATE TABLE DiscFilm
(
	IdDisc UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Disc(Id) ON DELETE CASCADE NOT NULL,
	IdFilm UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Film(Id) ON DELETE CASCADE NOT NULL
)

GO
CREATE TABLE [Order]
(
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
    OrderDateTime DATETIME NOT NULL,
    IdClient UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Client(Id) ON DELETE CASCADE NOT NULL
)

GO
CREATE TABLE OrderItem
(
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
    IdOrder UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [Order](Id) ON DELETE CASCADE NOT NULL,
    IdDisc UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Disc(Id) ON DELETE CASCADE NOT NULL,
    Quantity INT NOT NULL
)

GO
CREATE TABLE OperationType
(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
	TypeName NVARCHAR(30) NOT NULL
)

GO
CREATE TABLE OperationLog
(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
	OperationType UNIQUEIDENTIFIER FOREIGN KEY REFERENCES OperationType(Id) ON DELETE CASCADE NOT NULL,
	OperationDateTimeStart DATETIME NOT NULL,
	OperationDateTimeEnd DATETIME,
	IdOrder UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [Order](Id) ON DELETE CASCADE NOT NULL,
)