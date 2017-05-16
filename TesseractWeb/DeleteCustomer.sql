create procedure DeleteCustomer
(
            @UserId INT
)
as
Delete from UserTable where UserId=@UserId