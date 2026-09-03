CREATE PROCEDURE [dbo].[SaveBatchZipToDisk]
@BatchId INT NULL, @Url NVARCHAR (4000) NULL, @ApiKey NVARCHAR (200) NULL, @OutputFolder NVARCHAR (4000) NULL
AS EXTERNAL NAME [API_Test_Calls].[API_Test_Calls.BatchFileApi].[SaveBatchZipToDisk]

