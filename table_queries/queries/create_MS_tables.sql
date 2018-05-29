use [MS3]
GO

--remove the table if exists
if exists (select 1
            from  sysobjects
           where  id = object_id('Messages')
            and   type = 'U')
   drop table [dbo].[Messages]
go


--remove the table if exists
if exists (select 1
            from  sysobjects
           where  id = object_id('Users')
            and   type = 'U')
   drop table [dbo].[Users]
go

/*==============================================================*/
/* Table: "User"                                                */
/*==============================================================*/
create table "Users" (
   Id							int			     identity,
   Group_Id						int			     not null,
   Nickname						char(8)	         not null,   
   Password						char(64)         not null,   
   constraint PK_USER primary key (Id),
   CONSTRAINT uq_group_nick UNIQUE(Nickname, Group_Id)

)
go


/*==============================================================*/
/* Table: "Messages"                                               */
/*==============================================================*/
create table "Messages" (
   Guid						char(68)	    not null,
   User_Id					int			    FOREIGN KEY REFERENCES Users(Id),
   SendTime					datetime		not null,
   Body						nchar(100)		not null,      
   constraint PK_MESSAGES primary key (Guid)
)
go

--remove the index if exists
if exists (select 1
            from  sysindexes
           where  id    = object_id('Messages')
            and   name  = 'IndexMessagesTS'
            and   indid > 0
            and   indid < 255)
   drop index Messages.IndexMessagesTS
go

/*==============================================================*/
/* Index: IndexMessagesTS                                        */
/*==============================================================*/
create index IndexMessagesTS on dbo.[Messages] (
SendTime DESC
)
go

--remove the index if exists
if exists (select 1
            from  sysindexes
           where  id    = object_id('Messages')
            and   name  = 'IndexMessagesGuid'
            and   indid > 0
            and   indid < 255)
   drop index Messages.IndexMessagesGuid
go

/*==============================================================*/
/* Index: IndexMessagesTS                                        */
/*==============================================================*/
create index IndexMessagesGuid on dbo.[Messages] (
Guid ASC
)
go
