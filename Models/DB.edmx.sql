
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/27/2021 18:34:19
-- Generated from EDMX file: C:\Users\argen\source\repos\Challenge\Models\DB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [database];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TeacherSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subjects] DROP CONSTRAINT [FK_TeacherSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_SubjectSubject_Student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subjects_Students] DROP CONSTRAINT [FK_SubjectSubject_Student];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentSubject_Student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subjects_Students] DROP CONSTRAINT [FK_StudentSubject_Student];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Students]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Students];
GO
IF OBJECT_ID(N'[dbo].[Subjects_Students]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subjects_Students];
GO
IF OBJECT_ID(N'[dbo].[Subjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subjects];
GO
IF OBJECT_ID(N'[dbo].[Teachers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teachers];
GO
IF OBJECT_ID(N'[dbo].[Admins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Admins];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Students'
CREATE TABLE [dbo].[Students] (
    [id_student] int IDENTITY(1,1) NOT NULL,
    [DNI] nvarchar(max)  NOT NULL,
    [file_number] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Subjects_Students'
CREATE TABLE [dbo].[Subjects_Students] (
    [id_subject_student] int IDENTITY(1,1) NOT NULL,
    [id_subject] int  NOT NULL,
    [id_student] int  NOT NULL
);
GO

-- Creating table 'Subjects'
CREATE TABLE [dbo].[Subjects] (
    [id_subject] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [desc] nvarchar(max)  NULL,
    [time_from] time  NOT NULL,
    [time_to] time  NOT NULL,
    [capacity] int  NOT NULL,
    [id_teacher] int  NOT NULL
);
GO

-- Creating table 'Teachers'
CREATE TABLE [dbo].[Teachers] (
    [id_teacher] int IDENTITY(1,1) NOT NULL,
    [first_name] nvarchar(max)  NOT NULL,
    [last_name] nvarchar(max)  NOT NULL,
    [active] bit  NOT NULL
);
GO

-- Creating table 'Admins'
CREATE TABLE [dbo].[Admins] (
    [id_admin] int IDENTITY(1,1) NOT NULL,
    [username] nvarchar(max)  NOT NULL,
    [password] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id_student] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [PK_Students]
    PRIMARY KEY CLUSTERED ([id_student] ASC);
GO

-- Creating primary key on [id_subject_student] in table 'Subjects_Students'
ALTER TABLE [dbo].[Subjects_Students]
ADD CONSTRAINT [PK_Subjects_Students]
    PRIMARY KEY CLUSTERED ([id_subject_student] ASC);
GO

-- Creating primary key on [id_subject] in table 'Subjects'
ALTER TABLE [dbo].[Subjects]
ADD CONSTRAINT [PK_Subjects]
    PRIMARY KEY CLUSTERED ([id_subject] ASC);
GO

-- Creating primary key on [id_teacher] in table 'Teachers'
ALTER TABLE [dbo].[Teachers]
ADD CONSTRAINT [PK_Teachers]
    PRIMARY KEY CLUSTERED ([id_teacher] ASC);
GO

-- Creating primary key on [id_admin] in table 'Admins'
ALTER TABLE [dbo].[Admins]
ADD CONSTRAINT [PK_Admins]
    PRIMARY KEY CLUSTERED ([id_admin] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [id_teacher] in table 'Subjects'
ALTER TABLE [dbo].[Subjects]
ADD CONSTRAINT [FK_TeacherSubject]
    FOREIGN KEY ([id_teacher])
    REFERENCES [dbo].[Teachers]
        ([id_teacher])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeacherSubject'
CREATE INDEX [IX_FK_TeacherSubject]
ON [dbo].[Subjects]
    ([id_teacher]);
GO

-- Creating foreign key on [id_subject] in table 'Subjects_Students'
ALTER TABLE [dbo].[Subjects_Students]
ADD CONSTRAINT [FK_SubjectSubject_Student]
    FOREIGN KEY ([id_subject])
    REFERENCES [dbo].[Subjects]
        ([id_subject])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubjectSubject_Student'
CREATE INDEX [IX_FK_SubjectSubject_Student]
ON [dbo].[Subjects_Students]
    ([id_subject]);
GO

-- Creating foreign key on [id_student] in table 'Subjects_Students'
ALTER TABLE [dbo].[Subjects_Students]
ADD CONSTRAINT [FK_StudentSubject_Student]
    FOREIGN KEY ([id_student])
    REFERENCES [dbo].[Students]
        ([id_student])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentSubject_Student'
CREATE INDEX [IX_FK_StudentSubject_Student]
ON [dbo].[Subjects_Students]
    ([id_student]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------