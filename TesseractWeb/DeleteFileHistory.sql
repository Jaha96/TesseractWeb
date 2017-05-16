create procedure DeleteFileHistory
(
            @HistoryId INT
)
as
Delete from FileHistory where HistoryId=@HistoryId