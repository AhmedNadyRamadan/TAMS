
go
CREATE TABLE Students
(
	Id INT IDENTITY PRIMARY KEY,
	[Name] VARCHAR(20) NOT NULL,
	Email VARCHAR(255) NOT NULL UNIQUE,
	CONSTRAINT CK_email_Student CHECK (Email LIKE '_%@_%._%'),
	CONSTRAINT CK_name_minlength_Student CHECK (LEN([Name]) >= 3)

);
---
go
CREATE TABLE Labs
(
	Id INT IDENTITY PRIMARY KEY,
	[Name] VARCHAR(20) NOT NULL,
	TotalDegree INT NOT NULL,
	NoOfSessions INT NOT NULL,
	CONSTRAINT CK_TotalDegree_LAB CHECK (TotalDegree > 10),
	CONSTRAINT CK_name_minlength_LAB CHECK (LEN([Name]) >= 3)

);
---
go
CREATE TABLE [Sessions]
(
	Id INT IDENTITY PRIMARY KEY,
	LabID INT,
	[Date] Date NOT NULL,
	CONSTRAINT FK_Session_Lab FOREIGN KEY (LabID) REFERENCES Labs(Id),
	CONSTRAINT CK_Date_Session CHECK ([Date] >= CAST(GETDATE() AS DATE)),
);
---
go
CREATE TABLE TAs
(
	Id INT IDENTITY PRIMARY KEY,
	[Name] VARCHAR(20) NOT NULL,
	Email VARCHAR(255) NOT NULL UNIQUE,
	LabID Int,
	CONSTRAINT CK_email_TA CHECK (Email LIKE '_%@_%._%'),
	CONSTRAINT CK_name_minlength_TA CHECK (LEN([Name]) >= 3),
	CONSTRAINT FK_TA_Lab FOREIGN KEY (LabID) REFERENCES Labs(Id),
	
);
---
go
CREATE TABLE Agendas
(
	Id INT IDENTITY PRIMARY KEY,
	LabID INT,
	TA_ID Int,
	TopicName Varchar(255) NOT NULL unique,
	[Resources] Varchar(max) NOT NULL,
	CONSTRAINT FK_Agenda_TA FOREIGN KEY (TA_ID) REFERENCES TAs(Id),
	CONSTRAINT FK_Agenda_Lab FOREIGN KEY (LabID) REFERENCES Labs(Id),

);
---

go
CREATE TABLE Students_TAs 
(
    StudentId INT,
    TA_Id INT,
    CONSTRAINT PK_Students_TAs PRIMARY KEY (StudentId, TA_Id),
    CONSTRAINT FK_Students_TAs_Student FOREIGN KEY (StudentId) REFERENCES Students(Id),
    CONSTRAINT FK_Students_TAs_TA FOREIGN KEY (TA_Id) REFERENCES TAs(Id)
);
---
go
CREATE TABLE Labs_Students
(
    LabId INT,
    StudentId INT,

    CONSTRAINT PK_Labs_Students PRIMARY KEY (LabId, StudentId),
    CONSTRAINT FK_Labs_Students_Lab FOREIGN KEY (LabId) REFERENCES Labs(Id),
    CONSTRAINT FK_Labs_Students_Student FOREIGN KEY (StudentId) REFERENCES Students(Id)
);
---
go
CREATE TABLE Sessions_Students
(
    SessionId INT,
    StudentId INT,
	Attendance_Degree INT,
    CONSTRAINT PK_Sessions_Students PRIMARY KEY (SessionId, StudentId),
    CONSTRAINT FK_Sessions_Students_Lab FOREIGN KEY (SessionId) REFERENCES [Sessions](Id),
    CONSTRAINT FK_Sessions_Students_Student FOREIGN KEY (StudentId) REFERENCES Students(Id)
);
