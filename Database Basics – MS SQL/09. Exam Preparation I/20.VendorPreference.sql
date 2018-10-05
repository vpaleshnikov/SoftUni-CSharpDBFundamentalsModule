WITH CTE_VendorPreference
AS
(
    SELECT m.MechanicId, v.VendorId, SUM(op.Quantity) AS ItemsFromVendor FROM Mechanics AS m
    JOIN Jobs AS j ON j.MechanicId = m.MechanicId
    JOIN Orders AS o ON o.JobId = j.JobId
    JOIN OrderParts op ON op.OrderId = o.OrderId
    JOIN Parts AS p ON p.PartId = op.PartId
    JOIN Vendors v ON v.VendorId = p.VendorId
    GROUP BY m.MechanicId, v.VendorId
)

SELECT m.FirstName + ' ' + LastName AS Mechanic,
       v.Name AS Vendor,
       c.ItemsFromVendor AS Parts,
CAST(CAST(CAST(ItemsFromVendor AS DECIMAL(6, 2)) / (SELECT SUM(ItemsFromVendor) FROM CTE_VendorPreference WHERE MechanicId = c.MechanicId) * 100 AS INT) AS VARCHAR(20)) + '%' AS Preference
FROM CTE_VendorPreference AS c
JOIN Mechanics m ON m.MechanicId = c.MechanicId
JOIN Vendors v ON v.VendorId = c.VendorId
ORDER BY Mechanic, Parts DESC, Vendor