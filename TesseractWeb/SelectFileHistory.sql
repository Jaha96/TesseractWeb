create procedure SelectFileHistory
(
            @UserId INT
)
as
Select * from FileHistory where UserId=@UserId