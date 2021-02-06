CREATE TABLE Telemetry
(
	Id BIGINT IDENTITY(1,1) NOT NULL,
	NodeId VARCHAR(50) NOT NULL,
	ApplicationUri VARCHAR(500) NOT NULL,
	MetricDisplayName VARCHAR(500) NOT NULL,
	MetricValue DECIMAL(15,5) NOT NULL,
	MetricSourceTimestamp DATETIME NOT NULL
)