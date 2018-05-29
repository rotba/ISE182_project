USE [MS3]
GO

-- Creates the login. 
CREATE LOGIN publicUser
    WITH PASSWORD = 'isANerd';  
GO  

-- Creates a database user for the login created above.  
CREATE USER publicUser FOR LOGIN publicUser;  
GO  

