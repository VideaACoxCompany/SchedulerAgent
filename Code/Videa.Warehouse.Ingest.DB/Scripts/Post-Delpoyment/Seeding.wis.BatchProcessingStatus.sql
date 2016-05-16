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
go