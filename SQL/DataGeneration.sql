USE CDDiscsGroupProject

-----------------------------------------Client DATA GENERATION-----------------------------------------
GO
------------------RANDOM DATE GENERATION------------------
DECLARE @StartDate AS DATE;
DECLARE @EndDate AS DATE;

SELECT @StartDate = '01/01/1970',
       @EndDate = GETDATE();
------------------RANDOM DATE DENERATION------------------
BEGIN 
	DECLARE @MetaClient TABLE 
	(
		M_Id INT,
		M_FirstName NVARCHAR(50),
		M_LastName NVARCHAR(50),
		M_AddressPart NVARCHAR (50),
		M_City NVARCHAR(50),
		M_ContactPhone VARCHAR(3),
		M_ContactMail NVARCHAR(100),
		M_BirthDay DATE,
		M_MarriedStatus BIT,
		M_Sex BIT,
		M_Child BIT
	)

	DECLARE @number INT,
	@FirstName NVARCHAR(50), @LastName NVARCHAR(50), 
	@Address NVARCHAR (100), @City NVARCHAR (50), 
	@ContactPhone NVARCHAR(20), @ContactMail NVARCHAR(100), @BirthDay DATE,
	@MarriedStatus BIT, @Sex BIT, @Child BIT 

	SET @number = 100;
	INSERT INTO @MetaClient
	VALUES(1, N'Steve',		N'Young',		N'Bard Boulevard',		N'Depton',		'073', '@gmail.com',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (2, N'Michael',	N'Ross',		N'Ironwood Street',		N'Advill',		'075', '@icloud.com',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (3, N'John',		N'Bennett',		N'Beach Street',		N'Berner',		'096', '@host.org',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (4, N'Peter',		N'Stewart',		N'Dawn Boulevard',		N'Dowelle',		'095', '@mail.org',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (5, N'Mara',		N'Parker',		N'Great Route',			N'Ireport',		'098', '@ukr.net',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (6, N'Marry',		N'Lee',			N'Summit Lane',			N'Millemount',	'063', '@yahoo.net',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (7, N'Natalie',	N'Morgan',		N'Station Row',			N'Giverne',		'067', '@tele.info',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (8, N'Sophia',	N'Walker',		N'Starlight Way',		N'Coplecross',	'068', '@web.info',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)))

	WHILE @number > 0
    BEGIN
		SELECT @FirstName =		M_FirstName		FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @LastName =		M_LastName		FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @Address =		M_AddressPart	FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @City =			M_City			FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @ContactPhone =	M_ContactPhone	FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @ContactMail =	M_ContactMail	FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @BirthDay =		M_Birthday		FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @MarriedStatus =	M_MarriedStatus	FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @Sex =			M_Sex			FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @Child =			M_Child			FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)

		SET @Address = N'street' + ' ' + @Address + ' ' + CAST((FLOOR(RAND()*(100-1+1)+1)) AS VARCHAR(3))
		SET @ContactPhone = @ContactPhone + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(9-0+1)+0)) AS VARCHAR(3))
		SET @ContactMail = LOWER(@FirstName) + '.' + LOWER(@LastName) + @ContactMail
		SET @number -= 1

		INSERT INTO Client (FirstName, LastName, [Address], City, ContactPhone, ContactMail, BirthDay, MarriedStatus, Sex, HasChild)
		VALUES (@FirstName, @LastName, @Address, @City, @ContactPhone, @ContactMail, @BirthDay, @MarriedStatus, @Sex, @Child)
    END
END

SELECT * FROM Client
-----------------------------------------Client DATA GENERATION-----------------------------------------




-----------------------------------------PhoneList DATA GENERATION-----------------------------------------
GO
BEGIN
	DECLARE @MetaPhoneList TABLE
	(
		M_Id INT,
		M_Phone NVARCHAR(20),
		M_IdClient UNIQUEIDENTIFIER
	)

	DECLARE @number INT,  @Phone NVARCHAR(20), @IdClient UNIQUEIDENTIFIER

	SET @number = 40
	INSERT INTO @MetaPhoneList
	VALUES	(1, '073', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(2, '075', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(3, '096', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(4, '095', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(5, '098', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(6, '063', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(7, '067', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(8, '068', (SELECT TOP 1 Id FROM Client ORDER BY NEWID()))

	WHILE @number > 0
	BEGIN
		SELECT @Phone = M_Phone FROM @MetaPhoneList WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @IdClient = M_IdClient FROM @MetaPhoneList WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
	
		SET @Phone = @Phone + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(9-0+1)+0)) AS VARCHAR(3))

		INSERT INTO PhoneList (Phone, IdClient)
		VALUES (@Phone, @IdClient)

		SET @number -= 1
	END
END

SELECT * FROM PhoneList
-----------------------------------------PhoneList DATA GENERATION-----------------------------------------





-----------------------------------------MailList DATA GENERATION-----------------------------------------
GO
BEGIN
    DECLARE @MetaMailList TABLE
    (
        M_Id INT,
        M_Mail NVARCHAR(100),
        M_IdClient UNIQUEIDENTIFIER
    )

    DECLARE @number INT,  @Mail NVARCHAR(100), @IdClient UNIQUEIDENTIFIER

    SET @number = 40
    INSERT INTO @MetaMailList
    VALUES  (1, '@gmail.com',   (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (2, '@icloud.com',  (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (3, '@host.org',    (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (4, '@mail.org',    (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (5, '@ukr.net',     (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (6, '@yahoo.net',   (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (7, '@tele.info',   (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (8, '@web.info',    (SELECT TOP 1 Id FROM Client ORDER BY NEWID()))

    WHILE @number > 0
    BEGIN
        SELECT @Mail = M_Mail, @IdClient = M_IdClient FROM @MetaMailList WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
        
        SET @Mail = LOWER((SELECT FirstName FROM Client WHERE Id = @IdClient)) + '.' + LOWER((SELECT LastName FROM Client WHERE Id = @IdClient)) + @Mail

        INSERT INTO MailList(Mail, IdClient)
        VALUES (@Mail, @IdClient)

        SET @number -= 1
    END
END


SELECT * FROM MailList
-----------------------------------------MailList DATA GENERATION-----------------------------------------





-----------------------------------------Music DATA GENERATION-----------------------------------------
GO
BEGIN
	DECLARE @MetaMusic TABLE
	(
		M_Id INT,
		M_Name NVARCHAR(50),
		M_Genre NVARCHAR(50),
		M_Artist NVARCHAR(50),
		M_Language NVARCHAR(50)
	)
	DECLARE @number INT, @Name NVARCHAR(50), @Genre NVARCHAR(50), @Artist NVARCHAR(50), @Language NVARCHAR(50)

	SET @number = 40;
	INSERT INTO @MetaMusic
	VALUES  (1,  'Steps Of The Storm',					'Pop',			'High Fire',				'English'),
			(2,  'Think For A Moment Of Peace',			'Rock',			'Echo Chords',				'Spanish'),
			(3,  'Street',								'Hip-Hop',		'Simple Spark',				'Fhench'),
			(4,  'Memories',							'Metal',		'Lost Aces',				'German'),
			(5,  'Dreams Of The Devil',					'Jazz',			'Adorable Enemies',			'Italian'),
			(6,  'Way Of Heaven',						'Blues',		'Glass Union',				'Ukrainian'),
			(7,  'Tracks Of My Angel',					'Country',		'Flux',						'Polish'),
			(8,  'Living Of My Enemies',				'EDM',			'Spoof',					'Arabic'),
			(9,  'Desired And Angel',					'Latin',		'Gesture',					'Japanese'),
			(10, 'Wicked And Soul',						'R&B',			'Rapture',					'Korean'),
			(11, 'You Knock Me Off My Feet',			'Folk',			'Strife',					'Hindi'),
			(12, 'It`s Time For Rock And Roll',			'World',		'Oasis of Integrity',		'Chinese'),
			(13, 'Get It Together',						'New Age',		'Cipher of Doubt',			'Welsh'),
			(14, 'Babe, I`m Lonely',					'Acoustic',		'Salvo of Rage',			'English'),
			(15, 'Rock My World',						'Pop',			'Figures of One Night',		'Spanish'),
			(16, 'She Thinks I Rock All Night',			'Rock',			'Marvel of Habit',			'Fhench'),
			(17, 'He Hopes I`m Nothing Without You',	'Hip-Hop',		'Season of Obscurity',		'German'),
			(18, 'She Knows You Called For Me',			'Metal',		'Association of Utopia',	'Italian'),
			(19, 'She Hopes I Live On The Wild Side',	'Jazz',			'Theory',					'Ukrainian'),
			(20, 'I Go My Own Way',						'Blues',		'Delirium',					'Polish'),
			(21, 'She Thinks He`s Going To Hell',		'Country',		'Warmth of Velocity',		'Arabic'),
			(23, 'She Knows He`s Rock`N Roll',			'EDM',			'Epoch of Fiction',			'Japanese'),
			(24, 'She Hates You Rock My World',			'Latin',		'Personality of Luck',		'Korean'),
			(25, 'He Thinks He`s Going To Hell',		'R&B',			'Dynamite Sounds',			'Hindi')

	WHILE @number > 0
	BEGIN
		SELECT @Name =		M_Name		FROM @MetaMusic WHERE M_Id =  FLOOR(RAND()*(25-1+1)+1)
		SELECT @Genre =		M_Genre		FROM @MetaMusic WHERE M_Id =  FLOOR(RAND()*(25-1+1)+1)
		SELECT @Artist =	M_Artist	FROM @MetaMusic WHERE M_Id =  FLOOR(RAND()*(25-1+1)+1)
		SELECT @Language =	M_Language	FROM @MetaMusic WHERE M_Id =  FLOOR(RAND()*(25-1+1)+1)

		INSERT INTO Music (Name, Genre, Artist, Language)
		VALUES (@Name, @Genre, @Artist, @Language)

		SET @number -= 1
	END
END

SELECT * FROM Music
-----------------------------------------Music DATA GENERATION-----------------------------------------





-----------------------------------------Film  DATA GENERATION-----------------------------------------
GO
BEGIN
	DECLARE @MetaFilm TABLE
	(
		M_Id INT,
		M_Name NVARCHAR(50),
		M_Genre NVARCHAR(30),
		M_Producer NVARCHAR(50),
		M_MainRole NVARCHAR(50),
		M_AgeLimit INT
	)
	DECLARE @number INT, @Id INT, @Name NVARCHAR(50), @Genre NVARCHAR(50), @Producer NVARCHAR(50), @MainRole NVARCHAR(50), @AgeLimit INT

	SET @number = 40
	INSERT INTO @MetaFilm
	VALUES  (1,		'Warrior Without Desire',		'Classic',		'Maximo Wheeler',		'Brynlee Rojas',		6),
			(2,		'Parrot of Reality',			'Drama',		'Jerry Buck',			'Serena Mcdowell',		12),
			(3,		'Criminals Without Courage',	'Thriller',		'Tyshawn Frost',		'Jazlyn Griffin',		14),
			(4,		'Heirs of the Ancestors',		'Action',		'Heath Mckinney',		'Urijah Trevino',		16),
			(5,		'Creators and Owls',			'Comedy',		'Rafael Rich',			'Natalie Shah',			18),
			(6,		'Kings and Priests',			'Romance',		'Kailey Sellers',		'Nathanael Cordova',	0),
			(7,		'Victory of the River',			'Musical',		'Bradyn Mckenzie',		'Ella Landry',			6),
			(8,		'Influence of Darkness',		'Animation',	'Kael Boyer',			'Carsen Rose',			12),
			(9,		'Bound to My Soul',				'Horror',		'Shamar Love',			'Vaughn Cherry',		14),
			(10,	'Bathing In the World',			'Foreign',		'Ryan Gonzalez',		'Isiah Curtis',			16),
			(11,	'Dead In Dreams',				'Independent',	'Yusuf Horton',			'Alicia Cervantes',		18),
			(12,	'Dead In the City',				'Documentary',	'Kassandra Raymond',	'Maximillian Collier',	0),
			(13,	'Blood At the River',			'Classic',		'Helen Krause',			'Richard Bautista',		6),
			(14,	'Sounds In the City',			'Drama',		'Ally Alvarado',		'Jaylen Barajas',		12),
			(15,	'Faith of Nightmares',			'Thriller',		'Lorenzo Bishop',		'Caden Mcbride',		14),
			(16,	'Meeting In the Immortals',		'Action',		'Zaid Newman',			'Jamarcus Garner',		16),
			(17,	'Eliminating My Family',		'Comedy',		'Deacon Baker',			'Casey Wolfe',			18),
			(18,	'Songs of the North',			'Romance',		'Ainsley Hernandez',	'Craig Tapia',			0),
			(19,	'Destroying Nature',			'Musical',		'Gilbert Mcdowell',		'Kaliyah Harrell',		6),
			(20,	'Answering the South',			'Animation',	'Evan Hobbs',			'Danica Bullock',		12),
			(21,	'Praised by the South',			'Horror',		'Madden Ewing',			'Deacon Estrada',		14),
			(22,	'The Time of Empire',			'Foreign',		'Alisa Sanford',		'Jagger Solomon',		16),
			(23,	'Dancing In the dark',		    'Independent',	'Judith Le',			'Nataly Poole',			18),
			(24,	'Mending Eternity',				'Documentary',	'Marc Horton',			'Glenn Barnett',		0),
			(25,	'Rescue In the Dark',			'Classic',		'Giovanni Sanders',		'Belen Chase',			6)

	WHILE @number > 0
	BEGIN
		SELECT @Name =		M_Name		FROM @MetaFilm WHERE M_Id =	FLOOR(RAND()*(25-1+1)+1)
		SELECT @Genre =		M_Genre		FROM @MetaFilm WHERE M_Id =	FLOOR(RAND()*(25-1+1)+1)
		SELECT @Producer =	M_Producer	FROM @MetaFilm WHERE M_Id =	FLOOR(RAND()*(25-1+1)+1)
		SELECT @MainRole =	M_MainRole	FROM @MetaFilm WHERE M_Id =	FLOOR(RAND()*(25-1+1)+1)
		SELECT @AgeLimit =	M_AgeLimit	FROM @MetaFilm WHERE M_Id =	FLOOR(RAND()*(25-1+1)+1)

		INSERT INTO Film (Name, Genre, Producer, MainRole, AgeLimit)
		VALUES (@Name, @Genre, @Producer, @MainRole, @AgeLimit);

		SET @number -= 1
	END
END

SELECT * FROM Film
-----------------------------------------Film  DATA GENERATION-----------------------------------------




GO
------------------Disc DATA GENERATION------------------
BEGIN
	DECLARE @MetaDisc TABLE
	(
		M_Id INT,
		M_Name NVARCHAR(50),
		M_Price DECIMAL(9, 2)
	)

	DECLARE @number INT, @Name NVARCHAR(50), @Price DECIMAL(9, 2)

	SET @number = 200;
	INSERT INTO @MetaDisc
	VALUES  
		(1, 'Action Movie Collection', RAND()*(50-5+1)+6),
		(2, 'Drama Film Compilation', RAND()*(50-5+1)+6),
		(3, 'Sci-Fi Classics Set', RAND()*(50-5+1)+6),
		(4, 'Romantic Movies Bundle', RAND()*(50-5+1)+6),
		(5, 'Thriller Box Set', RAND()*(50-5+1)+6),
		(6, 'Comedy Festival Pack', RAND()*(50-5+1)+6),
		(7, 'Fantasy Films Assortment', RAND()*(50-5+1)+6),
		(8, 'Family Movie Night Selection', RAND()*(50-5+1)+6),
		(9, 'Classic Cinema Compilation', RAND()*(50-5+1)+6),
		(10, 'Documentary Series', RAND()*(50-5+1)+6),
		(11, 'Epic Adventure Movies', RAND()*(50-5+1)+6),
		(12, 'Historical Drama Bundle', RAND()*(50-5+1)+6),
		(13, 'Music Hits Collection', RAND()*(50-5+1)+6),
		(14, 'Rock Legends Anthology', RAND()*(50-5+1)+6),
		(15, 'Pop Music Extravaganza', RAND()*(50-5+1)+6),
		(16, 'Jazz and Blues Spectacle', RAND()*(50-5+1)+6),
		(17, 'Country Music Bonanza', RAND()*(50-5+1)+6),
		(18, 'Rap and Hip-Hop Showcase', RAND()*(50-5+1)+6),
		(19, 'Electronic Beats Collection', RAND()*(50-5+1)+6),
		(20, 'Classical Symphony Set', RAND()*(50-5+1)+6),
		(21, 'Fitness Workout Mix', RAND()*(50-5+1)+6),
		(22, 'Chillout and Relaxation Tunes', RAND()*(50-5+1)+6),
		(23, 'Travel and Adventure Soundtrack', RAND()*(50-5+1)+6),
		(24, 'Love Songs for Every Occasion', RAND()*(50-5+1)+6),
		(25, 'Motivational Speech Series', RAND()*(50-5+1)+6),
		(26, 'Mindfulness Meditation Set', RAND()*(50-5+1)+6),
		(27, 'Language Learning Audio Pack', RAND()*(50-5+1)+6),
		(28, 'Cooking and Culinary Delights', RAND()*(50-5+1)+6),
		(29, 'Travel Documentary Special', RAND()*(50-5+1)+6),
		(30, 'Wildlife and Nature Exploration', RAND()*(50-5+1)+6),
		(31, 'Technology and Innovation Series', RAND()*(50-5+1)+6),
		(32, 'Science Fiction Audiobooks', RAND()*(50-5+1)+6),
		(33, 'Business and Finance Essentials', RAND()*(50-5+1)+6),
		(34, 'Self-Help and Personal Growth', RAND()*(50-5+1)+6),
		(35, 'Health and Wellness Podcasts', RAND()*(50-5+1)+6),
		(36, 'Fashion and Style Podcast', RAND()*(50-5+1)+6),
		(37, 'Art and Creativity Masterclass', RAND()*(50-5+1)+6),
		(38, 'Photography Tips and Tricks', RAND()*(50-5+1)+6),
		(39, 'Gaming and Entertainment Hub', RAND()*(50-5+1)+6),
		(40, 'Tech Reviews and Gadgets', RAND()*(50-5+1)+6)

	WHILE @number > 0
	BEGIN
		SELECT @Name =	M_Name	FROM @MetaDisc WHERE M_Id = FLOOR(RAND()*(40-1+1)+1)
		SELECT @Price = M_Price FROM @MetaDisc WHERE M_Id = FLOOR(RAND()*(40-1+1)+1)

		INSERT INTO Disc (Name, Price, LeftOnStock, Rating)
		VALUES (@Name, @Price, CAST((FLOOR(RAND()*(100-1+1)+1)) AS INT), RAND()*(5-1+1)+1)

		SET @number -=1
	END
END

SELECT * FROM Disc
------------------Disc DATA GENERATION------------------



GO
------------------DiscMusic DATA GENERATION------------------
DECLARE @DiscMusic TABLE
(
	IdDisc UNIQUEIDENTIFIER,
	IdMusic UNIQUEIDENTIFIER
)

DECLARE @DiscId UNIQUEIDENTIFIER, @MusicId UNIQUEIDENTIFIER

SET @DiscId = (SELECT TOP 1 Id FROM Disc ORDER BY NEWID())
SET @MusicId = (SELECT TOP 1 Id FROM Music ORDER BY NEWID())
INSERT INTO @DiscMusic (IdDisc, IdMusic) VALUES (@DiscId, @MusicId)

DECLARE @counter INT
SET @counter = 1
WHILE @counter < 50
BEGIN
	SET @DiscId = (SELECT TOP 1 Id FROM Disc ORDER BY NEWID())
	SET @MusicId = (SELECT TOP 1 Id FROM Music ORDER BY NEWID())
	INSERT INTO @DiscMusic (IdDisc, IdMusic) VALUES (@DiscId, @MusicId)
	SET @counter = @counter + 1
END

INSERT INTO DiscMusic (IdDisc, IdMusic)
SELECT IdDisc, IdMusic FROM @DiscMusic
------------------DiscMusic DATA GENERATION------------------





GO
------------------DiscFilm DATA GENERATION------------------
DECLARE @DiscFilm TABLE
(
	IdDisc UNIQUEIDENTIFIER,
	IdFilm UNIQUEIDENTIFIER
)

DECLARE @FilmDiscId UNIQUEIDENTIFIER, @FilmId UNIQUEIDENTIFIER, @counter INT = 0

SET @FilmDiscId = (SELECT TOP 1 Id FROM Disc ORDER BY NEWID())
SET @FilmId = (SELECT TOP 1 Id FROM Film ORDER BY NEWID())
INSERT INTO @DiscFilm (IdDisc, IdFilm) VALUES (@FilmDiscId, @FilmId)

SET @counter = 1
WHILE @counter < 50
BEGIN
	SET @FilmDiscId = (SELECT TOP 1 Id FROM Disc ORDER BY NEWID())
	SET @FilmId = (SELECT TOP 1 Id FROM Film ORDER BY NEWID())
	INSERT INTO @DiscFilm (IdDisc, IdFilm) VALUES (@FilmDiscId, @FilmId)
	SET @counter = @counter + 1
END

INSERT INTO DiscFilm (IdDisc, IdFilm)
SELECT IdDisc, IdFilm FROM @DiscFilm
------------------DiscFilm DATA GENERATION------------------




-----------------------------------------OperationType DATA GENERATION-----------------------------------------
INSERT INTO OperationType(TypeName)
VALUES ('Purchase'), ('Rent')

SELECT * FROM OperationType
-----------------------------------------OperationType DATA GENERATION-----------------------------------------


GO
------------------OperationLog, Order, OrderItem DATA GENERATION------------------
DECLARE @OperationLog TABLE
(
    Id UNIQUEIDENTIFIER,
    OperationType UNIQUEIDENTIFIER,
    OperationDateTimeStart DATETIME,
    OperationDateTimeEnd DATETIME,
    IdOrder UNIQUEIDENTIFIER
)

DECLARE @number INT
SET @number = 15000

WHILE @number > 0
BEGIN
    DECLARE @OperationLogId UNIQUEIDENTIFIER, @OperationTypeId UNIQUEIDENTIFIER, @OperationDateTimeStart DATETIME, @OperationDateTimeEnd DATETIME, @OrderLogId UNIQUEIDENTIFIER
    SET @OperationLogId = NEWID()
    SET @OperationTypeId = (SELECT TOP 1 Id FROM OperationType ORDER BY NEWID())
    SET @OperationDateTimeStart = DATEADD(DAY, RAND()*(DATEDIFF(DAY, '2022-01-01', '2023-12-31')), '2022-01-01')
    SET @OperationDateTimeEnd = DATEADD(DAY, RAND()*(DATEDIFF(DAY, '2022-01-01', '2023-12-31')), '2022-01-01')

    SET @OrderLogId = NEWID()
    INSERT INTO [Order] (Id, OrderDateTime, IdClient)
    VALUES (@OrderLogId, @OperationDateTimeStart, (SELECT TOP 1 Id FROM Client ORDER BY NEWID()))

    DECLARE @OrderItemId UNIQUEIDENTIFIER, @DiscId UNIQUEIDENTIFIER, @Quantity INT
    SET @OrderItemId = NEWID()
    SET @DiscId = (SELECT TOP 1 Id FROM Disc ORDER BY NEWID())
    SET @Quantity = ROUND(RAND()*(10-1)+1, 0)

    INSERT INTO OrderItem (Id, IdOrder, IdDisc, Quantity)
    VALUES (@OrderItemId, @OrderLogId, @DiscId, @Quantity)

    INSERT INTO @OperationLog (Id, OperationType, OperationDateTimeStart, OperationDateTimeEnd, IdOrder)
    VALUES (@OperationLogId, @OperationTypeId, @OperationDateTimeStart, @OperationDateTimeEnd, @OrderLogId)

    SET @number = @number - 1
END

INSERT INTO OperationLog (Id, OperationType, OperationDateTimeStart, OperationDateTimeEnd, IdOrder)
SELECT Id, OperationType, OperationDateTimeStart, OperationDateTimeEnd, IdOrder FROM @OperationLog
------------------OperationLog, Order, OrderItem DATA GENERATION------------------