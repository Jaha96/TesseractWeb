CREATE TABLE [dbo].[UserTable] (
    [UserId]     INT           IDENTITY (1, 1) NOT NULL,
    [UserName]   NVARCHAR (50) NOT NULL,
    [Password]   VARCHAR (50)  NOT NULL,
    [Email]      VARCHAR (50)  NOT NULL,
    [LoginCount] INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

CREATE TABLE [dbo].[FileHistory] (
    [HistoryId]  INT           IDENTITY (1, 1) NOT NULL,
    [OutputFile] VARCHAR (100) NOT NULL,
    [InputImage] VARCHAR (100) NOT NULL,
    [Date]       DATETIME      NOT NULL,
    [UserId]     INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([HistoryId] ASC),
    CONSTRAINT [FK_FileHistory_ToTable] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserTable] ([UserId])
);

