
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[SelectRandomProverb]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 Id, Content, IsVisible, CreatedOn
	FROM dbo.Proverbs 
	ORDER BY RAND();
END
GO
