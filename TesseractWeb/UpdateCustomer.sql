create procedure UpdateCustomer
(

            @UserName NVARCHAR(50)=null,
            @Password VARCHAR(50)=null,
            @Email VARCHAR(50)=null,
            @UserId INT
)
as
Update UserTable set 
	UserName  = ISNULL(@UserName, UserName),
	Password=ISNULL(@Password,Password),
	Email=ISNULL(@Email,Email) 
	where UserId=@UserId