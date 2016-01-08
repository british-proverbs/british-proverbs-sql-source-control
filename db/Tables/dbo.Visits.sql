CREATE TABLE [dbo].[Visits]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[CreatedOn] [datetimeoffset] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Visits] ADD CONSTRAINT [PK_Visits] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
