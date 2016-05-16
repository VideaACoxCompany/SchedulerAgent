PRINT 'Start seeding wis.BatchStatisticType'
MERGE [wis].[BatchStatisticType] t
using (
	values('RowCount')
) as tempt([Name])
	on (t.Name = tempt.Name)
when not matched by target
	then insert ([Name]) values(tempt.[Name]);
PRINT 'End seeding wis.BatchStatisticType'
go