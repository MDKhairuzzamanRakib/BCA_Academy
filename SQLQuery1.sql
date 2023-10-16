drop database if exists  BCA_Academy
CREATE DATABASE BCA_Academy
GO

USE BCA_Academy
GO

CREATE TABLE Student
(
	StudentId INT PRIMARY KEY IDENTITY,
	StudentName VARCHAR(50) ,
	BirthDate DATE ,
	Age INT ,
	Picture VARCHAR(MAX) ,
	MaritalStatus BIT 
)
CREATE TABLE Subject
(
	SubjectId INT PRIMARY KEY IDENTITY,
	SubjectName VARCHAR(50)
)

CREATE TABLE AdmissionEntry
(
	AdmissionEntryId INT PRIMARY KEY IDENTITY,
	StudentId INT REFERENCES Student(StudentId),
	SubjectId INT REFERENCES Subject(SubjectId)
)