USE [MS3]

--give permissions to user publicUser to change the relevant tables

GRANT SELECT ON OBJECT::dbo.[Messages] TO publicUser;
GRANT INSERT ON OBJECT::dbo.[Messages] TO publicUser;
GRANT UPDATE ON OBJECT::dbo.[Messages] TO publicUser;
GRANT DELETE ON OBJECT::dbo.[Messages] TO publicUser;

GRANT SELECT ON OBJECT::dbo.[Users] TO publicUser;
GRANT INSERT ON OBJECT::dbo.[Users] TO publicUser;
GRANT UPDATE ON OBJECT::dbo.[Users] TO publicUser;
GRANT DELETE ON OBJECT::dbo.[Users] TO publicUser;
GO --GO GO