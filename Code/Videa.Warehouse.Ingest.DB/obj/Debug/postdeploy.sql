PRINT 'Start of post-deployment scripts'

PRINT 'Begin Seeding wis Tables'

PRINT 'Start seeding wis.BatchProcessingStatus'
MERGE [wis].[BatchProcessingStatus] t
using (
	values('Retrieving'), 
	('Retrieved'), 
	('Validating'), 
	('Validated'), 
	('Prestaging'), 
	('Prestaged'), 
	('Staging'), 
	('Staged'), 
	('Vaulting'), 
	('Vaulted'), 
	('Purging'), 	
	('Purged'),
	('Error')
) as tempt([StatusName])
	on (t.StatusName = tempt.StatusName)
when not matched by target
	then insert ([StatusName]) values(tempt.[StatusName]);
PRINT 'End seeding wis.BatchProcessingStatus'
GO
GO
Print 'End of Seeding.wis.BatchProcessingStatus.sql'

PRINT 'Start seeding wis.BatchProcessingType'
MERGE [wis].[BatchProcessingType] t
using (
	values('Traffic'), ('Platform')
) as tempt([Name])
	on (t.Name = tempt.Name)
when not matched by target
	then insert ([Name]) values(tempt.[Name]);
PRINT 'End seeding wis.BatchProcessingType'
GO
GO
Print 'End of Seeding.wis.BatchProcessingType.sql'

PRINT 'Start seeding wis.BatchStatisticType'
MERGE [wis].[BatchStatisticType] t
using (
	values('RowCount')
) as tempt([Name])
	on (t.Name = tempt.Name)
when not matched by target
	then insert ([Name]) values(tempt.[Name]);
PRINT 'End seeding wis.BatchStatisticType'
GO
GO
Print 'End of Seeding.wis.BatchStatisticType.sql'

PRINT 'End Seeding wis Tables'

PRINT 'End of post-deployment scripts'
GO
