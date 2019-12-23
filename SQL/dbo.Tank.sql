/****** When using local data file, no need to use databasename ******/
/******
USE [TankDB]
GO
******/

/****** 对象: Table [dbo].[Tank] 脚本日期: 2017/8/31 5:58:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Tank];


GO
CREATE TABLE [dbo].[Tank] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (MAX)   NOT NULL,
    [Stage]            INT             NOT NULL,
    [Type]             VARCHAR (MAX)   NOT NULL,
    [PlaceOfOrigin]    VARCHAR (MAX)   NULL,
    [Manufacturer]     VARCHAR (MAX)   NULL,
    [ServiceStartYear] INT             NULL,
    [ServiceEndYear]   INT             NULL,
    [Weight]           NUMERIC (18, 2) NULL,
    [Length]           NUMERIC (18, 2) NULL,
    [Width]            NUMERIC (18, 2) NULL,
    [Height]           NUMERIC (18, 2) NULL,
    [Crew]             SMALLINT        NULL,
    [Armor]            VARCHAR (MAX)   NULL,
    [MainArmament]     VARCHAR (MAX)   NULL,
    [SecondaryArmament]   VARCHAR (MAX)   NULL,
    [Engine]           VARCHAR (MAX)   NULL,
    [Power]            NUMERIC (18, 2) NULL,
    [Transmission]     VARCHAR (MAX)   NULL,
    [Suspension]       VARCHAR (MAX)   NULL,
    [GroundClearance]  NUMERIC (18, 2) NULL,
    [FuelCapacity]     NUMERIC (18)    NULL,
    [Range]            NUMERIC (18, 2) NULL,
    [Speed]            NUMERIC (18, 2) NULL,
    [Description]      VARCHAR (MAX)   NULL,
    [ImagePath]        VARCHAR (MAX)   NULL,
    [ImageFile]        VARCHAR (MAX)   NULL, 
    CONSTRAINT [PK_Tank] PRIMARY KEY ([Id])
);


