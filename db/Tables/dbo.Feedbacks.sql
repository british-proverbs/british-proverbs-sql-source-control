CREATE TABLE [dbo].[Feedbacks]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[UserId] [int] NOT NULL,
[Content] [nvarchar] (max) COLLATE Latin1_General_CI_AS NOT NULL,
[CreatedOn] [datetimeoffset] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Feedbacks] ADD CONSTRAINT [PK_Feedbacks] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Feedbacks] ADD CONSTRAINT [FK_Feedbacks_Users] FOREIGN KEY ([Id]) REFERENCES [dbo].[Users] ([Id])
GO
