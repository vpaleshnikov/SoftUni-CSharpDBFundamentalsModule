CREATE TRIGGER tr_LogsNotificationEmails ON Logs FOR INSERT
AS
BEGIN
  DECLARE @Recipient INT = (SELECT AccountId FROM inserted);
  DECLARE @OldBalance DECIMAL(15, 2) = (SELECT OldSum FROM inserted);
  DECLARE @NewBalance DECIMAL(15, 2) = (SELECT NewSum FROM inserted);
  DECLARE @Subject VARCHAR(200) = CONCAT('Balance change for account: ', @Recipient);
  DECLARE @Body VARCHAR(200) = CONCAT('On ', GETDATE(), ' your balance was changed from ', @OldBalance, ' to ', @NewBalance, '.');  

  INSERT INTO NotificationEmails (Recipient, Subject, Body) 
  VALUES (@Recipient, @Subject, @Body)
END