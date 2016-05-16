PRINT 'Start seeding wis.BatchProcessingType'
MERGE [wis].[BatchProcessingType] t
using (
	values('Traffic'), ('Platform')
) as tempt([Name])
	on (t.Name = tempt.Name)
when not matched by target
	then insert ([Name]) values(tempt.[Name]);
PRINT 'End seeding wis.BatchProcessingType'
go