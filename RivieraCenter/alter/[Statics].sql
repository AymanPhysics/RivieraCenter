alter TABLE [dbo].[Statics]add
	[ServerName] [nvarchar](100) NULL,
	[ServerDatabaseName] [nvarchar](100) NULL
go
update [dbo].[Statics]set [ServerName] ='41.000.000.000',[ServerDatabaseName] ='OMEGA'

go
delete SubServers
insert SubServers([ServerName],[ServerDatabaseName] )select '41.000.000.000','OMEGA'
insert SubServers([ServerName],[ServerDatabaseName] )select '41.000.000.000','OMEGA'
insert SubServers([ServerName],[ServerDatabaseName] )select '41.000.000.000','OMEGA'
GO
