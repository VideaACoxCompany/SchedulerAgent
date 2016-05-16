PRINT 'Start of post-deployment scripts'

PRINT 'Begin Seeding wis Tables'

:r .\Seeding.wis.BatchProcessingStatus.sql
GO
Print 'End of Seeding.wis.BatchProcessingStatus.sql'

:r .\Seeding.wis.BatchProcessingType.sql
GO
Print 'End of Seeding.wis.BatchProcessingType.sql'

:r .\Seeding.wis.BatchStatisticType.sql
GO
Print 'End of Seeding.wis.BatchStatisticType.sql'

PRINT 'End Seeding wis Tables'

PRINT 'End of post-deployment scripts'