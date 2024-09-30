-- Create the Product table with ProdID as a string (VARCHAR) and unique
CREATE TABLE Product (
    ProdID VARCHAR(20) PRIMARY KEY,  -- ProdID as a string and unique
    ProdName NVARCHAR(100),
    Quantity INT,
    Price DECIMAL(10, 2),
    Origin NVARCHAR(50),
    ExpireDate DATETIME
);

-- Delete product with ProdID 'abcxyz' if it exists
DELETE FROM Product 
WHERE ProdID = 'abcxyz';

-- Insert or update dummy data into the Product table
-- Use this to update existing rows if the same ProdID exists
INSERT INTO Product (ProdID, ProdName, Quantity, Price, Origin, ExpireDate)
VALUES
    ('P001', 'Widget A - Updated', 120, 18.99, 'USA', '2024-12-31'),
    ('P002', 'Gadget B', 50, 29.99, 'China', '2025-06-30'),
    ('P003', 'Tool C - Updated', 85, 35.99, 'Germany', '2024-09-30'),
    ('P004', 'Elysia Signet', 50, 29.99, N'Nhật Bản', '2023-06-30'),
    ('P005', 'Device D', 200, 49.99, 'Canada', '2026-08-15'),
    ('P006', 'Accessory E', 150, 14.99, 'South Korea', '2025-02-28'),
    ('P007', 'Appliance F', 80, 99.99, 'France', '2025-12-31');
