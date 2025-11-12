CREATE PROCEDURE [dbo].[spUsers_Register]
    @userName nvarchar(16),
    @firstName nvarchar(50),
    @lastName nvarchar(50),
    @password nvarchar(16)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if username exists
    IF EXISTS (SELECT 1 FROM dbo.Users WHERE UserName = @userName)
    BEGIN
        RAISERROR('Username already exists', 16, 1);
        RETURN;
    END
    
    -- Insert new user
    INSERT INTO dbo.Users (UserName, FirstName, LastName, Password)
    VALUES (@userName, @firstName, @lastName, @password);
END