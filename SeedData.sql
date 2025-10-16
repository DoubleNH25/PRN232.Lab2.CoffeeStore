-- Insert Categories
INSERT INTO Category (Name, Description, CreatedDate) VALUES
('Coffee', 'Various coffee products', GETDATE()),
('Tea', 'Different types of tea', GETDATE()),
('Desserts', 'Sweet treats and desserts', GETDATE()),
('Sandwiches', 'Fresh sandwiches', GETDATE()),
('Pastries', 'Freshly baked pastries', GETDATE()),
('Smoothies', 'Refreshing smoothies', GETDATE()),
('Juices', 'Fresh fruit juices', GETDATE()),
('Breakfast', 'Breakfast items', GETDATE()),
('Lunch', 'Lunch options', GETDATE()),
('Snacks', 'Quick snacks', GETDATE()),
('Specialty Drinks', 'Specialty beverages', GETDATE()),
('Seasonal Items', 'Seasonal offerings', GETDATE()),
('Bakery', 'Fresh baked goods', GETDATE()),
('Healthy Options', 'Health-conscious choices', GETDATE()),
('Cold Beverages', 'Chilled drinks', GETDATE()),
('Hot Beverages', 'Warm drinks', GETDATE()),
('Local Specialties', 'Items unique to our location', GETDATE()),
('Organic Products', 'Organic selections', GETDATE()),
('Gluten-Free', 'Gluten-free alternatives', GETDATE()),
('Vegan Options', 'Plant-based choices', GETDATE()),
('Holiday Specials', 'Holiday-themed items', GETDATE()),
('Signature Blends', 'Our signature coffee blends', GETDATE()),
('International Flavors', 'Flavors from around the world', GETDATE()),
('Limited Edition', 'Limited time offerings', GETDATE());

-- Insert Users (Admin and Staff)
INSERT INTO [User] (Email, PasswordHash, FirstName, LastName, Role, CreatedDate, IsActive) VALUES
('admin@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Admin@123'), 2), 'Admin', 'User', 'Admin', GETDATE(), 1),
('staff@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Staff', 'User', 'Staff', GETDATE(), 1),
('john.doe@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'John', 'Doe', 'Staff', GETDATE(), 1),
('jane.smith@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Jane', 'Smith', 'Staff', GETDATE(), 1),
('mike.johnson@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Mike', 'Johnson', 'Staff', GETDATE(), 1),
('sarah.williams@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Sarah', 'Williams', 'Staff', GETDATE(), 1),
('david.brown@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'David', 'Brown', 'Staff', GETDATE(), 1),
('lisa.davis@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Lisa', 'Davis', 'Staff', GETDATE(), 1),
('robert.miller@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Robert', 'Miller', 'Staff', GETDATE(), 1),
('emily.wilson@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Emily', 'Wilson', 'Staff', GETDATE(), 1),
('michael.taylor@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Michael', 'Taylor', 'Staff', GETDATE(), 1),
('amanda.thomas@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Amanda', 'Thomas', 'Staff', GETDATE(), 1),
('james.jackson@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'James', 'Jackson', 'Staff', GETDATE(), 1),
('jennifer.white@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Jennifer', 'White', 'Staff', GETDATE(), 1),
('daniel.harris@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Daniel', 'Harris', 'Staff', GETDATE(), 1),
('michelle.martin@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Michelle', 'Martin', 'Staff', GETDATE(), 1),
('christopher.thompson@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Christopher', 'Thompson', 'Staff', GETDATE(), 1),
('jessica.garcia@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Jessica', 'Garcia', 'Staff', GETDATE(), 1),
('matthew.martinez@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Matthew', 'Martinez', 'Staff', GETDATE(), 1),
('ashley.robinson@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Ashley', 'Robinson', 'Staff', GETDATE(), 1),
('joshua.clark@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Joshua', 'Clark', 'Staff', GETDATE(), 1),
('brittany.rodriguez@coffeestore.com', CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'Staff@123'), 2), 'Brittany', 'Rodriguez', 'Staff', GETDATE(), 1);

-- Insert Products
INSERT INTO Product (Name, Description, Price, CategoryId, IsActive) VALUES
('Espresso', 'Strong black coffee', 3.50, 1, 1),
('Cappuccino', 'Espresso with steamed milk', 4.50, 1, 1),
('Latte', 'Espresso with lots of steamed milk', 4.75, 1, 1),
('Americano', 'Espresso with hot water', 3.75, 1, 1),
('Mocha', 'Espresso with chocolate and steamed milk', 5.25, 1, 1),
('Macchiato', 'Espresso with a dash of milk', 4.00, 1, 1),
('Flat White', 'Espresso with microfoam', 4.75, 1, 1),
('Iced Coffee', 'Chilled coffee with ice', 4.25, 1, 1),
('Cold Brew', 'Slow-steeped cold coffee', 4.50, 1, 1),
('Affogato', 'Espresso over vanilla ice cream', 5.75, 1, 1),
('Black Tea', 'Classic black tea', 2.50, 2, 1),
('Green Tea', 'Antioxidant-rich green tea', 2.75, 2, 1),
('White Tea', 'Light and delicate white tea', 3.00, 2, 1),
('Oolong Tea', 'Partially fermented tea', 3.25, 2, 1),
('Chai Tea', 'Spiced tea with milk', 3.75, 2, 1),
('Herbal Tea', 'Caffeine-free herbal blend', 2.75, 2, 1),
('Matcha Latte', 'Ceremonial grade matcha', 4.50, 2, 1),
('Fruit Tea', 'Refreshing fruit infusion', 3.00, 2, 1),
('Earl Grey', 'Black tea with bergamot', 2.75, 2, 1),
('English Breakfast', 'Full-bodied black tea', 2.50, 2, 1),
('Chocolate Cake', 'Rich chocolate cake', 5.50, 3, 1),
('Cheesecake', 'Creamy New York style', 6.00, 3, 1),
('Tiramisu', 'Italian coffee-flavored dessert', 6.50, 3, 1),
('Brownie', 'Fudgy chocolate brownie', 3.75, 3, 1),
('Cookies', 'Freshly baked cookies', 2.50, 3, 1),
('Cupcake', 'Decorated vanilla cupcake', 3.25, 3, 1),
('Muffin', 'Blueberry muffin', 2.75, 3, 1),
('Croissant', 'Buttery French pastry', 2.50, 3, 1),
('Apple Pie', 'Homemade apple pie', 4.75, 3, 1),
('Ice Cream', 'Vanilla bean ice cream', 3.50, 3, 1),
('Turkey Sandwich', 'Fresh turkey with veggies', 7.50, 4, 1),
('Ham Sandwich', 'Honey-glazed ham sandwich', 7.25, 4, 1),
('Chicken Salad Sandwich', 'Grilled chicken salad', 8.00, 4, 1),
('BLT Sandwich', 'Bacon, lettuce, and tomato', 6.75, 4, 1),
('Club Sandwich', 'Triple-decker sandwich', 8.50, 4, 1),
('Veggie Sandwich', 'Fresh vegetable sandwich', 6.50, 4, 1),
('Grilled Cheese', 'Classic grilled cheese', 5.25, 4, 1),
('Panini', 'Pressed Italian sandwich', 7.75, 4, 1),
('Wrap', 'Chicken Caesar wrap', 7.00, 4, 1),
('Bagel', 'Everything bagel with cream cheese', 4.50, 4, 1),
('Croissant', 'Buttery French pastry', 2.50, 5, 1),
('Danish', 'Fruit-filled Danish pastry', 3.25, 5, 1),
('Muffin', 'Assorted muffins', 2.75, 5, 1),
('Scone', 'Freshly baked scone', 2.50, 5, 1),
('Cinnamon Roll', 'Warm cinnamon roll', 3.75, 5, 1),
('Bagel', 'Plain or seeded bagel', 2.25, 5, 1),
('English Muffin', 'Toasted English muffin', 2.00, 5, 1),
('Donut', 'Glazed donut', 1.75, 5, 1),
('Pretzel', 'Soft pretzel with salt', 2.50, 5, 1),
('Biscuit', 'Flaky butter biscuit', 1.50, 5, 1),
('Strawberry Smoothie', 'Fresh strawberry blend', 5.50, 6, 1),
('Banana Smoothie', 'Creamy banana blend', 5.25, 6, 1),
('Mango Smoothie', 'Tropical mango blend', 5.75, 6, 1),
('Berry Smoothie', 'Mixed berry blend', 5.75, 6, 1),
('Chocolate Smoothie', 'Rich chocolate blend', 6.00, 6, 1),
('Green Smoothie', 'Spinach and fruit blend', 5.50, 6, 1),
('Protein Smoothie', 'Protein-packed blend', 6.50, 6, 1),
('Tropical Smoothie', 'Pineapple and coconut blend', 5.75, 6, 1),
('Vanilla Smoothie', 'Classic vanilla blend', 5.25, 6, 1),
('Chocolate Milkshake', 'Thick chocolate shake', 6.00, 6, 1),
('Orange Juice', 'Freshly squeezed orange juice', 3.50, 7, 1),
('Apple Juice', 'Fresh apple juice', 3.25, 7, 1),
('Cranberry Juice', 'Pure cranberry juice', 3.75, 7, 1),
('Grapefruit Juice', 'Fresh grapefruit juice', 3.50, 7, 1),
('Pineapple Juice', 'Tropical pineapple juice', 3.75, 7, 1),
('Tomato Juice', 'Fresh tomato juice', 3.00, 7, 1),
('Vegetable Juice', 'Mixed vegetable blend', 4.00, 7, 1),
('Lemonade', 'Freshly squeezed lemonade', 3.25, 7, 1),
('Iced Tea', 'Chilled tea', 2.75, 7, 1),
('Coconut Water', 'Natural coconut water', 4.00, 7, 1),
('Breakfast Sandwich', 'Egg, cheese, and bacon', 5.50, 8, 1),
('Oatmeal', 'Steel-cut oatmeal', 4.25, 8, 1),
('Yogurt Parfait', 'Greek yogurt with granola', 4.75, 8, 1),
('Breakfast Burrito', 'Scrambled eggs and sausage', 6.25, 8, 1),
('Pancakes', 'Fluffy pancakes with syrup', 5.75, 8, 1),
('French Toast', 'Cinnamon French toast', 6.00, 8, 1),
('Breakfast Platter', 'Complete breakfast platter', 7.50, 8, 1),
('Granola Bowl', 'House-made granola bowl', 5.25, 8, 1),
('Avocado Toast', 'Smashed avocado on toast', 5.00, 8, 1),
('Breakfast Wrap', 'Egg and cheese wrap', 5.25, 8, 1),
('Chips', 'Assorted potato chips', 1.50, 9, 1),
('Nuts', 'Mixed nuts', 2.00, 9, 1),
('Popcorn', 'Freshly popped popcorn', 2.25, 9, 1),
('Trail Mix', 'Custom trail mix', 2.50, 9, 1),
('Energy Bar', 'Protein energy bar', 2.75, 9, 1),
('Fruit Cup', 'Seasonal fruit cup', 3.00, 9, 1),
('Veggie Sticks', 'Carrot and celery sticks', 1.75, 9, 1),
('Hummus', 'Chickpea hummus', 2.50, 9, 1),
('Guacamole', 'Fresh guacamole', 3.25, 9, 1),
('Salsa', 'House-made salsa', 2.25, 9, 1);

-- Create some orders
DECLARE @OrderId INT;
DECLARE @UserId INT;
DECLARE @ProductId INT;
DECLARE @Quantity INT;
DECLARE @UnitPrice DECIMAL(18,2);
DECLARE @TotalAmount DECIMAL(18,2);

-- Create 25 orders
DECLARE @i INT = 1;
WHILE @i <= 25
BEGIN
    -- Randomly select a user
    SELECT TOP 1 @UserId = UserId FROM [User] ORDER BY NEWID();
    
    -- Insert order
    INSERT INTO [Order] (UserId, OrderDate, Status, TotalAmount) 
    VALUES (@UserId, DATEADD(DAY, -@i, GETDATE()), 'Completed', 0);
    
    SET @OrderId = SCOPE_IDENTITY();
    SET @TotalAmount = 0;
    
    -- Add 1-5 order details to each order
    DECLARE @j INT = 1;
    DECLARE @detailCount INT = CAST(RAND()*4+1 AS INT); -- 1-5 details
    
    WHILE @j <= @detailCount
    BEGIN
        -- Randomly select a product
        SELECT TOP 1 @ProductId = ProductId, @UnitPrice = Price FROM Product ORDER BY NEWID();
        SET @Quantity = CAST(RAND()*4+1 AS INT); -- 1-5 quantity
        
        -- Insert order detail
        INSERT INTO OrderDetail (OrderId, ProductId, Quantity, UnitPrice)
        VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice);
        
        SET @TotalAmount = @TotalAmount + (@Quantity * @UnitPrice);
        SET @j = @j + 1;
    END
    
    -- Update order with total amount
    UPDATE [Order] SET TotalAmount = @TotalAmount WHERE OrderId = @OrderId;
    
    -- Create payment for the order
    INSERT INTO Payment (OrderId, Amount, PaymentDate, PaymentMethod)
    VALUES (@OrderId, @TotalAmount, DATEADD(MINUTE, 30, (SELECT OrderDate FROM [Order] WHERE OrderId = @OrderId)), 'Credit Card');
    
    SET @i = @i + 1;
END