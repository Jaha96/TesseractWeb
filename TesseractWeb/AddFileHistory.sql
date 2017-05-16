create procedure AddFileHistory
(

            @OutputFile VARCHAR(100),
            @InputImage VARCHAR(100),
            @Date Date,
			@UserId int,
            @HistoryId INT output
)
as
insert into FileHistory(OutputFile, InputImage, Date,UserId)
            values (@OutputFile, @InputImage, @Date,@UserId)
select @HistoryId = @@IDENTITY