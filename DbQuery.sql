CREATE TABLE Students (
    StudentId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(200) NOT NULL, -- Store hashed passwords
    EmailVerified BIT NOT NULL DEFAULT 0, -- Boolean column to indicate email verification status
    RegistrationDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Tutors (
    TutorId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(200) NOT NULL,
    EmailVerified BIT NOT NULL DEFAULT 0,
    ExpertiseArea NVARCHAR(100), -- Field to store the expertise area of the tutor
    RegistrationDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Admins (
    AdminId INT PRIMARY KEY IDENTITY(1,1),
    DisplayName NVARCHAR(100) NOT NULL,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(200) NOT NULL,
    RegistrationDate DATETIME DEFAULT GETDATE()); 