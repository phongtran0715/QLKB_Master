use master
EXEC sp_attach_db @dbname = N'QuangHuyMedical', 
   @filename1 = N'c:\Program Files\Microsoft SQL Server\MSSQL\Data\QuangHuyMedical.mdf', 
   @filename2 = N'c:\Program Files\Microsoft SQL Server\MSSQL\Data\QuangHuyMedical_log.ldf'

