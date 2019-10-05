use master
go
 
if not ('QuangHuyMedical' in (select name from sysdatabases))
    create database QuangHuyMedical
go

use QuangHuyMedical
go
-- (1) UserList
if not exists (select name from sysobjects where name = 'UserList' and type='U')
CREATE TABLE [dbo].[UserList](
	[UserId] [int] IDENTITY(0,1) NOT NULL,
	[UserName] [nchar](100) COLLATE Vietnamese_CI_AS NOT NULL,
	[Sex] [nchar](10) COLLATE Vietnamese_CI_AS NOT NULL,
	[WorkGroupId] [nchar](50) NOT NULL,
	[Password] [nchar](150) COLLATE Vietnamese_CI_AS NULL,
 CONSTRAINT [PK_UserList] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
begin transaction
INSERT INTO [QuangHuyMedical].[dbo].[UserList]([UserName],[Sex],[WorkGroupId],[Password]) VALUES ('sys','male','Admin','YslwPITD33Of1WqxOU6MPWQr4YAovsEOQK/5Dh2XxQd7UycJAiPFM8lzs4pBQenKBx2nymOKwUfYKMchj7/SjmQ30XINlA==')
commit transaction
go

--(2)	SickData
if object_id('dbo.SickData') is not null
begin
  drop table dbo.SickData
end
CREATE TABLE [dbo].[SickData](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[SickNum] [nchar](20) COLLATE Vietnamese_CI_AS NOT NULL,
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
	[DataPath] [nchar](250) COLLATE Vietnamese_CI_AS NULL,
 CONSTRAINT [PK__SickData__5FB337D6] PRIMARY KEY CLUSTERED 
(
	[SickNum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
go

--(3)	CheckRecord
if not exists(select name from sysobjects where name = 'CheckRecord' and type='U')
CREATE TABLE [dbo].[CheckRecord](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[CheckNum] [nchar](20) COLLATE Vietnamese_CI_AS NOT NULL,
	[ItemCode] [nchar](6) COLLATE Vietnamese_CI_AS NULL,
	[ItemContentCode] [char](6) COLLATE Vietnamese_CI_AS NULL
) ON [PRIMARY]
go


--(4)	CheckItemContent
if not exists (select name from sysobjects where name = 'CheckItemContent' and type='U')
CREATE TABLE [dbo].[CheckItemContent](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[ItemCode] [nchar](6) COLLATE Vietnamese_CI_AS NULL,
	[ContentCode] [nchar](6) COLLATE Vietnamese_CI_AS NULL,
	[Content] [nvarchar](100) COLLATE Vietnamese_CI_AS NULL,
	[ShowNum] [int] NULL
) ON [PRIMARY]
go

--(5)	CheckItem
if not exists (select name from sysobjects where name = 'CheckItem' and type='U')
CREATE TABLE [dbo].[CheckItem](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[ItemCode] [nchar](10) COLLATE Vietnamese_CI_AS NULL,
	[Content] [nchar](100) COLLATE Vietnamese_CI_AS NULL,
	[LegendEditable] [nchar](20) COLLATE Vietnamese_CI_AS NULL,
	[ShowNum] [int] NULL,
	[IsDisplay] [int] NULL CONSTRAINT [DF_CheckItem_IsDisplay]  DEFAULT ((0))
) ON [PRIMARY]
go

--(6)	BackupInfo
if not exists (select name from sysobjects where name = 'BackupInfo' and type='U')
CREATE TABLE [dbo].[BackupInfo](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[BackupName] [char](40) COLLATE Vietnamese_CI_AS NOT NULL,
	[BackupPath] [char](80) COLLATE Vietnamese_CI_AS NOT NULL,
	[BackupTime] [datetime] NOT NULL,
	[BackupStart] [datetime] NULL,
	[BackupEnd] [datetime] NULL
) ON [PRIMARY]
go

--(7) Roles LIST
if not exists (select name from sysobjects where name = 'RolesList' and type='U')
CREATE TABLE [dbo].[RolesList](
	[Num] [int] NOT NULL,
	[RoleName] [nchar](50) NOT NULL
) ON [PRIMARY]
begin transaction
INSERT INTO [QuangHuyMedical].[dbo].[RolesList]([Num],[RoleName]) VALUES ('0',N'Tạo tài khoản mới')
INSERT INTO [QuangHuyMedical].[dbo].[RolesList]([Num],[RoleName]) VALUES ('1',N'Sửa thông tin tài khoản')
INSERT INTO [QuangHuyMedical].[dbo].[RolesList]([Num],[RoleName]) VALUES ('2',N'Xóa tài khoản')
INSERT INTO [QuangHuyMedical].[dbo].[RolesList]([Num],[RoleName]) VALUES ('3',N'Thay đổi mật khẩu tài khoản')
INSERT INTO [QuangHuyMedical].[dbo].[RolesList]([Num],[RoleName]) VALUES ('4',N'Cài đặt hệ thống')
INSERT INTO [QuangHuyMedical].[dbo].[RolesList]([Num],[RoleName]) VALUES ('5',N'Xem báo cáo')
INSERT INTO [QuangHuyMedical].[dbo].[RolesList]([Num],[RoleName]) VALUES ('6',N'Xóa bệnh nhân')
INSERT INTO [QuangHuyMedical].[dbo].[RolesList]([Num],[RoleName]) VALUES ('7',N'Thay đổi danh mục khám bệnh')
INSERT INTO [QuangHuyMedical].[dbo].[RolesList]([Num],[RoleName]) VALUES ('8',N'Sao lưu , khôi phục dữ liệu ')
INSERT INTO [QuangHuyMedical].[dbo].[RolesList]([Num],[RoleName]) VALUES ('9',N'Xem lịch sử hệ thống')
commit transaction
go

--(8) Work Group
if not exists (select name from sysobjects where name = 'WorkGroup' and type='U')
CREATE TABLE [dbo].[WorkGroup](
	[WorkGroupId] [nchar](50) NOT NULL,
	[Descript] [nchar](50) COLLATE Vietnamese_CI_AS NOT NULL,
) ON [PRIMARY]
begin transaction
INSERT INTO [QuangHuyMedical].[dbo].[WorkGroup]([WorkGroupId],[Descript]) VALUES ('Doctor',N'Bác sỹ')
INSERT INTO [QuangHuyMedical].[dbo].[WorkGroup]([WorkGroupId],[Descript]) VALUES ('Manager',N'Quản Lý')
INSERT INTO [QuangHuyMedical].[dbo].[WorkGroup]([WorkGroupId],[Descript]) VALUES ('Admin',N'Admin')
commit transaction
go

--(9) User Group Role
if not exists (select name from sysobjects where name = 'UserGroupRole' and type='U')
CREATE TABLE [dbo].[UserGroupRole](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [nchar](50) NOT NULL,
	[RoleId] [int] NOT NULL
) ON [PRIMARY]
begin transaction
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '0', 'Admin');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '1', 'Admin');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '2', 'Admin');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '3', 'Admin');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '4', 'Admin');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '5', 'Admin');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '6', 'Admin');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '7', 'Admin');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '8', 'Admin');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '9', 'Admin');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '4', 'Manager');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '5', 'Manager');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '7', 'Manager');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '9', 'Manager');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '4', 'Doctor');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '5', 'Doctor');
INSERT INTO [QuangHuyMedical].[dbo].[UserGroupRole] ([RoleId], [GroupId]) VALUES ( '7', 'Doctor');
commit transaction
go

--(9) Work Log
if not exists (select name from sysobjects where name = 'WorkLog' and type='U')
CREATE TABLE [dbo].[WorkLog](
	[Num] [int] IDENTITY(1,1) NOT NULL,
	[Type] [char](4) NULL,
	[UserId] [int] NULL,
	[TimeLog] [datetime] NULL,
	[Source] [nchar](20) NULL,
	[UserName] [nchar](80) NULL,
	[Descript] [nchar](200) NULL,
	[SickName] [nchar](70) NULL,
PRIMARY KEY CLUSTERED 
(
	[Num] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
go	   

--(9) Database version
if not exists (select name from sysobjects where name = 'DbVersion' and type='U')
CREATE TABLE [dbo].[DbVersion](
	[Version] [int] NOT NULL
) ON [PRIMARY]
begin transaction
INSERT INTO [QuangHuyMedical].[dbo].[DbVersion] ([Version]) VALUES ( '380');
commit transaction
GO
commit;