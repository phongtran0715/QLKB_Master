use master
if not ('QuangHuyMedical' in (select name from sysdatabases))
    create database QuangHuyMedical
go

use QuangHuyMedical

-- (1) UserList
if not exists (select name from sysobjects where name = 'UserList' and type='U')

CREATE TABLE [dbo].[UserList](
	[UserNum] [nchar](100) COLLATE Vietnamese_CI_AS NOT NULL,
	[UserName] [nchar](100) COLLATE Vietnamese_CI_AS NOT NULL,
	[Sex] [nchar](10) COLLATE Vietnamese_CI_AS NOT NULL,
	[WorkGroup] [nchar](20) COLLATE Vietnamese_CI_AS NULL,
	[Password] [nchar](20) COLLATE Vietnamese_CI_AS NULL,
	[USERTYPE] [nchar](20) COLLATE Vietnamese_CI_AS NULL,
	[UserId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK__UserList__7F60ED59] PRIMARY KEY CLUSTERED 
(
	[UserNum] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]



--(2)	SickData
if not exists (select name from sysobjects where name = 'SickData' and type='U')

CREATE TABLE [dbo].[SickData](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[SickNum] [nchar](20) NOT NULL,
	[SickName] [nchar](70) COLLATE Vietnamese_CI_AS NOT NULL,
	[Sex] [nchar](10) COLLATE Vietnamese_CI_AS NULL,
	[Birthday] [datetime] NULL,
	[InsuranceId] [nchar](15) COLLATE Vietnamese_CI_AS NULL,
	[Telephone] [nchar](40) NULL,
	[Createtime] [date] NOT NULL,
	[Address] [nchar](100) COLLATE Vietnamese_CI_AS NULL,
	[IDType] [nchar](50) NULL,
	[IDCode] [nchar](50) NULL,
	[Age] [int] NULL,
	[Occupation] [nchar](50) COLLATE Vietnamese_CI_AS NULL,
	[SickHistoryNote] [nchar](200) COLLATE Vietnamese_CI_AS NULL,
	[CauseCheck] [nchar](100) COLLATE Vietnamese_CI_AS NULL,
 CONSTRAINT [PK__SickData__5FB337D6] PRIMARY KEY CLUSTERED 
(
	[SickNum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


--(3)	HospitalInfo
if not exists (select name from sysobjects where name = 'HospitalInfo' and type='U')
CREATE TABLE [dbo].[HospitalInfo](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[HospitalName] [nchar](100) COLLATE Vietnamese_CI_AS NOT NULL,
	[HospitalAddress] [nchar](100) COLLATE Vietnamese_CI_AS NOT NULL,
	[HospitalCall] [nchar](20) COLLATE Vietnamese_CI_AS NULL,
	[HospitalPost] [nchar](20) COLLATE Vietnamese_CI_AS NULL
) ON [PRIMARY]

--(4)	CheckRecord
if not exists(select name from sysobjects where name = 'CheckRecord' and type='U')
CREATE TABLE [dbo].[CheckRecord](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[CheckNum] [nchar](20) COLLATE Vietnamese_CI_AS NOT NULL,
	[ItemCode] [nchar](6) COLLATE Vietnamese_CI_AS NULL,
	[ItemContentCode] [char](6) COLLATE Vietnamese_CI_AS NULL
) ON [PRIMARY]


--(5)	CheckItemContent
if not exists (select name from sysobjects where name = 'CheckItemContent' and type='U')
CREATE TABLE [dbo].[CheckItemContent](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[ItemCode] [nchar](6) COLLATE Vietnamese_CI_AS NULL,
	[ContentCode] [nchar](6) COLLATE Vietnamese_CI_AS NULL,
	[Content] [nvarchar](100) COLLATE Vietnamese_CI_AS NULL,
	[ShowNum] [int] NULL
) ON [PRIMARY]

--(6)	CheckItem
if not exists (select name from sysobjects where name = 'CheckItem' and type='U')
CREATE TABLE [dbo].[CheckItem](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[ItemCode] [nchar](10) COLLATE Vietnamese_CI_AS NULL,
	[Content] [nchar](100) COLLATE Vietnamese_CI_AS NULL,
	[LegendEditable] [nchar](20) COLLATE Vietnamese_CI_AS NULL,
	[ShowNum] [int] NULL,
	[IsDisplay] [int] NULL CONSTRAINT [DF_CheckItem_IsDisplay]  DEFAULT ((0))
) ON [PRIMARY]

--(10)	BackupInfo
if not exists (select name from sysobjects where name = 'BackupInfo' and type='U')
CREATE TABLE [dbo].[BackupInfo](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[BackupName] [char](40) COLLATE Vietnamese_CI_AS NOT NULL,
	[BackupPath] [char](80) COLLATE Vietnamese_CI_AS NOT NULL,
	[BackupTime] [datetime] NOT NULL,
	[BackupStart] [datetime] NULL,
	[BackupEnd] [datetime] NULL
) ON [PRIMARY]


INSERT INTO [QuangHuyMedical].[dbo].[UserList]
           ([UserNum]
           ,[UserName]
           ,[Sex]
           ,[WorkGroup]
           ,[Password]
           ,[USERTYPE])
     VALUES
           ('1','sys','male','','admin','')
		   