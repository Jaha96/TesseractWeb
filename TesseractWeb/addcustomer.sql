create procedure AddCustomer
(

            @UserName NVARCHAR(50),
            @Password VARCHAR(50),
            @Email VARCHAR(50),
            @UserId INT output
)
as
insert into UserTable(UserName, Password, Email)
            values (@UserName, @Password, @Email)
select @UserId = @@IDENTITY