CREATE TABLE dbo.CoreEmployee
(
	CoreEmployeeId BIGINT IDENTITY(1,1) NOT NULL,
	FirstName VARCHAR(500) NOT NULL,
	LastName VARCHAR(500),
	BirthDate DATETIME,
	Pan VARCHAR(200) NOT NULL,
	AddedBy VARCHAR(500) NOT NULL,
	AddedOn DATETIME NOT NULL
)
GO
ALTER TABLE dbo.CoreEmployee ADD CONSTRAINT PK_CoreEmployee PRIMARY KEY(CoreEmployeeId)
GO
ALTER TABLE dbo.CoreEMployee ADD CONSTRAINT UQ_CoreEmployee UNIQUE(Pan)
GO


CREATE TABLE dbo.SysApi
(
	SysApiId INT IDENTITY(1,1) NOT NULL,
	Name VARCHAR(200) NOT NULL,
	Code VARCHAR(200) NOT NULL
)
ALTER TABLE dbo.SysApi ADD CONSTRAINT PK_SysApi PRIMARY KEY(SysApiId)
GO
ALTER TABLE dbo.SysApi ADD CONSTRAINT UQ_SysApi UNIQUE(Code)
GO

CREATE TABLE dbo.CoreSysApiAccess
(
	CoreSysApiAccessId BIGINT IDENTITY(1,1) NOT NULL,
	SysApiId INT NOT NULL,
	Token VARCHAR(200),
	AddedBy VARCHAR(500) NOT NULL,
	AddedOn DATETIME NOT NULL
)
GO
ALTER TABLE dbo.CoreSysApiAccess ADD CONSTRAINT PK_CoreSysApiAccess PRIMARY KEY(CoreSysApiAccessId)
GO
ALTER TABLE dbo.CoreSysApiAccess ADD CONSTRAINT UQ_CoreSysApiAccess_SysApiId_Token UNIQUE(SysApiId,Token)
GO
ALTER TABLE dbo.CoreSysApiAccess ADD CONSTRAINT FK_CoreSysApiAccess_SysApiId FOREIGN KEY (SysApiId) REFERENCES dbo.SysApi(SysApiId)
GO


CREATE TABLE dbo.CoreApiLog
(
	CoreApiLogId BIGINT IDENTITY(1,1) NOT NULL,
	SysApiId INT NOT NULL,
	RequestId VARCHAR(500),
	Token VARCHAR(200) NOT NULL,
	Request VARCHAR(MAX),
	Response VARCHAR(MAX),
	TimeTakenInMs BIGINT,
	AddedBy VARCHAR(500) NOT NULL,
	AddedOn DATETIME NOT NULL
)
GO
ALTER TABLE dbo.CoreApiLog ADD CONSTRAINT PK_CoreApiLog PRIMARY KEY(CoreApiLogId)
GO
ALTER TABLE dbo.CoreApiLog ADD CONSTRAINT FK_CoreApiLog_SysApiId FOREIGN KEY (SysApiId) REFERENCES dbo.SysApi(SysApiId)
GO

INSERT INTO dbo.SysApi(Name,Code) VALUES('Employee Api','A1')
GO
DECLARE @SysApiId INT
SELECT @SysApiId = SysApiId FROM dbo.SysApi WHERE Code = 'A1'
INSERT INTO dbo.CoreSysApiAccess(SysApiId,Token,AddedBy,AddedOn) VALUES(@SysApiId,NEWID(),'System',GETDATE())
GO

CREATE FUNCTION dbo.Split
(
    @Line nvarchar(MAX),
    @SplitOn nvarchar(5) = ','
)
RETURNS @RtnValue table
(
    --Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    Data nvarchar(100) NOT NULL
)
AS
BEGIN
    IF @Line IS NULL RETURN

    DECLARE @split_on_len INT = LEN(@SplitOn)
    DECLARE @start_at INT = 1
    DECLARE @end_at INT
    DECLARE @data_len INT

    WHILE 1=1
    BEGIN
        SET @end_at = CHARINDEX(@SplitOn,@Line,@start_at)
        SET @data_len = CASE @end_at WHEN 0 THEN LEN(@Line) ELSE @end_at-@start_at END
        INSERT INTO @RtnValue (data) VALUES( SUBSTRING(@Line,@start_at,@data_len) );
        IF @end_at = 0 BREAK;
        SET @start_at = @end_at + @split_on_len
    END

    RETURN
END
GO


CREATE PROCEDURE dbo.SysApi_GetAllApi
AS
BEGIN
	
	SELECT
		SysApiId,
		Name,
		Code
	FROM dbo.SysApi

END
GO


CREATE PROCEDURE dbo.SysApi_GetAccessTokens
AS
BEGIN

	SELECT
		api.SysApiId,
		api.Code,
		acc.Token
	FROM dbo.CoreSysApiAccess acc
	INNER JOIN dbo.SysApi api ON api.SysApiId = acc.SysApiId
	
END
GO

CREATE PROCEDURE dbo.CoreApiLog_GetRequestIdByApi
(
	@ApiCode VARCHAR(200)
)
AS
BEGIN

	DECLARE @SysApiId INT
	SELECT  @SysApiId = SysApiId  FROM dbo.SysApi WHERE Code = @ApiCode

	SELECT DISTINCT RequestId
	FROM dbo.CoreApiLog 
	WHERE SysApiId = @SysApiId

END
GO

CREATE PROCEDURE dbo.CoreEmployee_GetByPan
(
	@Pan VARCHAR(MAX)
)
AS
BEGIN
	

	SELECT Data
	INTO #Pan
	FROM dbo.Split(@pan,',')
	

	SELECT
		emp.CoreEmployeeId,
		p.Data AS Pan,
		emp.FirstName,
		emp.LastName,
		emp.BirthDate,
		CASE WHEN emp.Pan IS NULL THEN 'Employee not found' ELSE '' END AS Description
	FROM #Pan p
	LEFT JOIN dbo.CoreEmployee emp ON p.Data = emp.Pan

END
GO

CREATE PROCEDURE dbo.CoreApiLog_Insert
(
	@ApiId INT,
	@RequestId VARCHAR(500),
	@Token VARCHAR(500),
	@Request VARCHAR(MAX)
)
AS
BEGIN

	INSERT INTO dbo.CoreApiLog
	(
		SysApiId,
		RequestId,
		Token,
		Request,
		AddedBy,
		AddedOn
	)
	VALUES
	(
		@ApiId,
		@RequestId,
		@Token,
		@Request,
		@Token,
		GETDATE()
	)

	SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE dbo.CoreApiLog_Update
(
	@ApiLogId BIGINT,
	@Response VARCHAR(MAX),
	@TimeInMs BIGINT
)
AS
BEGIN

	UPDATE c
	SET c.Response=@Response,
	c.TimeTakenInMs=@TimeInMs
	FROM dbo.CoreApiLog c
	WHERE c.CoreApiLogId = @ApiLogId

END
GO

CREATE PROCEDURE dbo.CoreEmployee_DeleteByPan
(
	@Pan VARCHAR(MAX)
)
AS
BEGIN

	DELETE emp
	FROM dbo.Split(@Pan,',') t
	INNER JOIN dbo.CoreEmployee emp ON emp.Pan = t.Data

END