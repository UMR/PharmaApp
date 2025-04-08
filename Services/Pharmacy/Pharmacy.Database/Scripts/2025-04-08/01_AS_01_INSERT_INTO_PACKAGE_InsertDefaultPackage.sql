USE [PharmaAppDB]
GO

INSERT INTO [dbo].[Package]([Id], [Name], [Description], [Price], [CurrencyCode], [CommissionInPercent], [CreatedDate], [CreatedBy])
VALUES 
(
	'91dd28d1-980c-4031-897d-9215c7954eed',
	'Single scan package',
	'There will be only 1 scan under this package',
	99,
	'INR',
	15.34,
	GETDATE(),
	'b6970dae-1d97-4884-be10-56a0c5088f0b'
);