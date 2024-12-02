# Simple FastFood Order Using .NET 8, Dapper, Store Procedure, AES and Serilog

https://github.com/gtechsltn/Sensitive-Data-Security

https://github.com/gtechsltn/mvc-fastfood-order-net8-Dapper-Serialog

```markdown
# FastFood Order Service using .net 8 mvc,Dapper and Serialog

This repository contains the source code for the FastFood service.
It includes functionality for managing customer accounts, shopping carts, and
orders in a fast food delivery system.

## Demo
Visit this link for the Demo :https://fastfooddapper.runasp.net/

## Table of Contents
- [Features](#features)
- [Setup](#setup)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Features

- Authentication and Authorization: Utilizes cookie authentication for customer login
  with authorization policies.
- Customer Account Management: Allows customers to view and update their profile information.
- Shopping Cart Management: Enables customers to add items to their shopping carts, view the
  cart contents, and remove items.
- Order Placement: Facilitates the process of placing orders securely.
- Validation: Utilizes Data Annotations for input validation to ensure proper data integrity.
- Configuration: Email sender credentials can be configured via the appsettings.json file.
- Logging: Integrated Serialog functionality to capture errors and information.
- Dependency Injection: Utilizes dependency injection to manage and inject services.
- Dapper for Data Access: Utilizes Dapper for efficient data access.
- SerialLog for Logging: Implements logging functionalities using SerialLog.

## Setup

1. Clone the repository:

```bash
git clone https://github.com/yourusername/FastFoodService.git
```

2. Navigate to the project directory:

```bash
cd FastFoodService
```

3. Install dependencies:

```bash
dotnet restore
```

4. Execute the following script:
```DatabaseScript
USE [FastFoodDatabase]
GO
/****** Object:  Table [dbo].[AdminTable]    Script Date: 02/05/2024 21:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminTable](
	[AdminId] [int] IDENTITY(1,1) NOT NULL,
	[fullName] [varchar](150) NOT NULL,
	[emailAddress] [varchar](150) NOT NULL,
	[adminPassword] [varchar](15) NOT NULL,
	[regDate] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartTable]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartTable](
	[CustID] [int] NOT NULL,
	[FoodID] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[price] [float] NOT NULL,
	[CartDate] [varchar](100) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryTable]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryTable](
	[catName] [varchar](100) NOT NULL,
	[catDate] [varchar](100) NOT NULL,
	[catID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[catID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[catName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerTable]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerTable](
	[custId] [int] IDENTITY(1,1) NOT NULL,
	[custName] [varchar](200) NOT NULL,
	[custAddress] [varchar](500) NOT NULL,
	[custPhone] [varchar](200) NOT NULL,
	[custEmail] [varchar](200) NOT NULL,
	[custPassword] [varchar](15) NOT NULL,
	[regDate] [varchar](100) NOT NULL,
	[activatedPin] [varchar](20) NOT NULL,
	[activated] [int] NOT NULL,
	[ewallet] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[custId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[custEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[QName] [varchar](200) NOT NULL,
	[ErrorMessage] [varchar](max) NOT NULL,
	[eDate] [varchar](100) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FoodItemTable]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FoodItemTable](
	[FoodName] [varchar](100) NOT NULL,
	[catID] [int] NULL,
	[Fdesc] [varchar](1000) NULL,
	[price] [float] NOT NULL,
	[dateAdded] [varchar](100) NOT NULL,
	[foodID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[foodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[FoodName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderTable]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderTable](
	[CustID] [int] NOT NULL,
	[FoodID] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[price] [float] NOT NULL,
	[OrderDate] [varchar](100) NOT NULL,
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_OrderTable] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentTable]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[orderID] [int] NOT NULL,
	[totalPrice] [float] NOT NULL,
	[pdate] [varchar](100) NOT NULL,
	[dispatch] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleTable]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleTable](
	[AdminID] [int] NOT NULL,
	[adminRole] [varchar](50) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CustomerTable] ADD  DEFAULT ((0)) FOR [activated]
GO
ALTER TABLE [dbo].[CustomerTable] ADD  DEFAULT ((1000)) FOR [ewallet]
GO
ALTER TABLE [dbo].[PaymentTable] ADD  DEFAULT ((0)) FOR [dispatch]
GO
ALTER TABLE [dbo].[CartTable]  WITH CHECK ADD FOREIGN KEY([CustID])
REFERENCES [dbo].[CustomerTable] ([custId])
GO
ALTER TABLE [dbo].[CartTable]  WITH CHECK ADD FOREIGN KEY([FoodID])
REFERENCES [dbo].[FoodItemTable] ([foodID])
GO
ALTER TABLE [dbo].[FoodItemTable]  WITH CHECK ADD FOREIGN KEY([catID])
REFERENCES [dbo].[CategoryTable] ([catID])
GO
ALTER TABLE [dbo].[OrderTable]  WITH CHECK ADD FOREIGN KEY([CustID])
REFERENCES [dbo].[CustomerTable] ([custId])
GO
ALTER TABLE [dbo].[OrderTable]  WITH CHECK ADD FOREIGN KEY([FoodID])
REFERENCES [dbo].[FoodItemTable] ([foodID])
GO
ALTER TABLE [dbo].[PaymentTable]  WITH CHECK ADD FOREIGN KEY([orderID])
REFERENCES [dbo].[OrderTable] ([OrderID])
GO
ALTER TABLE [dbo].[RoleTable]  WITH CHECK ADD FOREIGN KEY([AdminID])
REFERENCES [dbo].[AdminTable] ([AdminId])
GO
/****** Object:  StoredProcedure [dbo].[AddToCart]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddToCart]
    @CustID int,
    @FoodID int,
    @Quantity int,
    @Price float        
AS
BEGIN
    -- Start a transaction
    BEGIN TRANSACTION;
     
    BEGIN TRY       

        -- If everything is successful, insert the customer data
        INSERT INTO CartTable (CustID,FoodID,quantity,price,CartDate) 
        VALUES (@CustID, @FoodID, @Quantity, @Price,  FORMAT(GETDATE(), 'dd/MM/yyyy @ HH:mm:ss'));       

        -- Commit the transaction
        COMMIT TRANSACTION;

		-- Return 1 to indicate successful update
		SELECT 1 AS Response;
    END TRY
    BEGIN CATCH
        -- If an error occurs, rollback the transaction
        ROLLBACK TRANSACTION;

        -- Log or handle the error
        INSERT INTO ErrorLog (QName, ErrorMessage, eDate) 
        VALUES ('AddToCart', ERROR_MESSAGE(), FORMAT(GETDATE(), 'dd/MM/yyyy @ HH:mm:ss'));

        -- Re-throw the error to the calling code
        THROW;
    END CATCH;
	
END;
GO
/****** Object:  StoredProcedure [dbo].[CheckOut]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckOut]
    @CustId INT
AS
BEGIN
    SELECT 
        c.CustId,
        c.CustName,
        c.CustAddress,
        c.CustPhone,
        c.CustEmail,
        c.Ewallet
    FROM 
        CustomerTable c
    WHERE 
        c.CustId = @CustId;

    SELECT 
        t.CartDate,
        t.FoodID,
        f.FoodName,
        t.Price AS FoodPrice,
        t.Quantity
    FROM 
        CartTable t
    INNER JOIN 
        FoodItemTable f ON t.FoodID = f.FoodID
    WHERE 
        t.CustId = @CustId;
END
GO
/****** Object:  StoredProcedure [dbo].[ConfirmAccount]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConfirmAccount]
@CustID int,
@ActivatedPin varchar(15)
AS
BEGIN
     DECLARE @RecordCount INT
     BEGIN TRY
		-- Initialize the output parameter
		SET @RecordCount = 0;

		-- Check if there are matching records
		SELECT @RecordCount = COUNT(*) FROM CustomerTable WHERE custId = @CustID AND activatedPin = @ActivatedPin;

		-- If there are matching records, update the activated status
		IF @RecordCount > 0
		BEGIN
			UPDATE CustomerTable SET activated = 1 WHERE custId = @CustID AND activatedPin = @ActivatedPin;
		END

		-- Return the @RecordCount
		SELECT @RecordCount;
	END TRY
	BEGIN CATCH
	        -- Log or handle the error
        INSERT INTO ErrorLog (QName, ErrorMessage, eDate) 
        VALUES ('ConfirmAccount Procedure', ERROR_MESSAGE(), FORMAT(GETDATE(), 'dd/MM/yyyy @ HH:mm:ss'));
        -- Re-throw the error to the calling code
        THROW;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[CustomerLogin]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CustomerLogin]
@CustEmail varchar(200)
AS
BEGIN
    SELECT  custId,custPassword,activatedPin,activated FROM CustomerTable WHERE custEmail = @CustEmail
END;
GO
/****** Object:  StoredProcedure [dbo].[DoesCustEmailExist]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DoesCustEmailExist]
@email varchar(200)
AS
BEGIN
    SELECT COUNT(*) FROM CustomerTable WHERE custEmail = @email
END;
GO
/****** Object:  StoredProcedure [dbo].[getcart]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getcart]
@custid int
AS
BEGIN
     SELECT CT.foodID, FIT.foodName, CT.price, CT.quantity FROM CartTable CT
     INNER JOIN FoodItemTable FIT ON FIT.foodID = CT.FoodID
     WHERE CT.custID = @custid  ORDER BY FIT.foodName ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[getcategories]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[getcategories]
AS
BEGIN
    select catName,catID from categoryTable order by catname asc;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerAccount]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCustomerAccount]
    @CustId INT
AS
BEGIN
    SELECT 
        c.CustId,
        c.CustName,
        c.CustAddress,
        c.CustPhone,
        c.CustEmail,
        c.CustPassword,
        c.Ewallet
    FROM 
        CustomerTable c
    WHERE 
        c.CustId = @CustId;

    SELECT 
        o.OrderDate,
        o.FoodID,
        f.FoodName,
        f.Price AS FoodPrice,
        o.Quantity
    FROM 
        OrderTable o
    INNER JOIN 
        FoodItemTable f ON o.FoodID = f.FoodID
    WHERE 
        o.CustId = @CustId;
END
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerAccount2]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCustomerAccount2]
@CustId int
AS
BEGIN
     SELECT custId,custName,custAddress,custPhone,custEmail,custPassword FROM CustomerTable 
     WHERE custId = @CustId  
END;
GO
/****** Object:  StoredProcedure [dbo].[GetCustProfileName]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCustProfileName]
@custid int
AS
BEGIN
     SELECT custid,custName FROM CustomerTable 
     WHERE custid = @custid  
END;
GO
/****** Object:  StoredProcedure [dbo].[getOrder]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getOrder]
@custid int
AS
BEGIN
     SELECT OT.foodID, FIT.foodName, OT.price, OT.quantity FROM OrderTable OT
     INNER JOIN FoodItemTable FIT ON FIT.foodid = OT.foodid 
     LEFT JOIN PaymentTable PT ON OT.orderID = PT.orderID
     WHERE OT.custID = @custid   AND PT.orderID IS NULL 
     ORDER BY FIT.foodName ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[getPassword]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getPassword]
@CustEmail VARCHAR(200)
AS
BEGIN
     SELECT custPassword FROM CustomerTable
     WHERE custEmail = @CustEmail;
END;
GO
/****** Object:  StoredProcedure [dbo].[getProductDetail]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getProductDetail]
@id int
AS
BEGIN
     SELECT foodID, foodName, Fdesc,price,1 as 'Quantity' FROM FoodItemTable 
     WHERE foodID = @id;
END;
GO
/****** Object:  StoredProcedure [dbo].[getrecentitem]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[getrecentitem]
AS
BEGIN
    select top 20 FoodName,foodID,price from FoodItemTable order by foodID desc;
END;
GO
/****** Object:  StoredProcedure [dbo].[PlaceOrder]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PlaceOrder]
@CustID INT
AS
BEGIN
    -- Start a transaction
    BEGIN TRANSACTION;
    
    DECLARE @totalPrice FLOAT;  -- Variable to store the total cart price
    DECLARE @ewallet FLOAT; -- Variable to store the Customer Ewallet Balance
    DECLARE @Response INT; -- Variable to store the response (1 for success, 0 for failure)

    BEGIN TRY
        SELECT @totalPrice = SUM(price * quantity) FROM CartTable WHERE CustID = @CustID;
        SELECT @ewallet = ewallet FROM CustomerTable WHERE CustID = @CustID;

        IF @ewallet >= @totalPrice
        BEGIN
            INSERT INTO OrderTable (CustID, FoodID, quantity, price, OrderDate) 
            SELECT CustID, FoodID, quantity, price, FORMAT(GETDATE(), 'dd/MM/yyyy @ HH:mm:ss') 
            FROM CartTable WHERE CustID = @CustID;
            
            DECLARE @orderID INT;
            SELECT @orderID = SCOPE_IDENTITY();

            DELETE FROM CartTable WHERE CustID = @CustID;

            INSERT INTO Paymenttable (orderID, totalPrice, pdate) 
            VALUES (@orderID, @totalPrice, FORMAT(GETDATE(), 'dd/MM/yyyy @ HH:mm:ss'));

            UPDATE CustomerTable SET ewallet = ewallet - @totalPrice WHERE CustID = @CustID;

            SET @Response = 1; -- Set response to success

        END
        ELSE 
        BEGIN
            SET @Response = 0; -- Set response to failure
        END;

        -- Commit the transaction
        COMMIT TRANSACTION;
        
        -- Return the response as a result set
        SELECT @Response AS Response;

    END TRY
    BEGIN CATCH
        -- If an error occurs, rollback the transaction
        ROLLBACK TRANSACTION;

        -- Log or handle the error
        INSERT INTO ErrorLog (QName, ErrorMessage, eDate) 
        VALUES ('PlaceOrder', ERROR_MESSAGE(), FORMAT(GETDATE(), 'dd/MM/yyyy @ HH:mm:ss'));

        -- Re-throw the error to the calling code
        THROW;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[removecart]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[removecart]
@CustID int,
@FoodID int
AS
BEGIN  

     DELETE FROM CartTable WHERE FoodID=@FoodID;
     SELECT 1 AS Response;
END;
GO
/****** Object:  StoredProcedure [dbo].[shop]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[shop]
@SearchString VARCHAR(100)
AS
BEGIN
    IF @SearchString = ''
    BEGIN
        SELECT fi.FoodID, fi.FoodName, fi.Price, c.CatName, fi.Fdesc AS FoodDescription 
        FROM FoodItemTable fi
        INNER JOIN CategoryTable c ON fi.catID = c.catID  
        ORDER BY fi.FoodName ASC;
    END
    ELSE 
    BEGIN
        SELECT fi.FoodID, fi.FoodName, fi.Price, c.CatName, fi.Fdesc AS FoodDescription 
        FROM FoodItemTable fi
        INNER JOIN CategoryTable c ON fi.catID = c.catID 
        WHERE fi.FoodName LIKE '%' + @SearchString + '%' OR c.CatName LIKE '%' + @SearchString + '%' 
        ORDER BY fi.FoodName ASC;
    END;
END;
GO
/****** Object:  StoredProcedure [dbo].[signup]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[signup]
    @CustName VARCHAR(200),
    @CustAddress VARCHAR(500),
    @CustPhone VARCHAR(50),
    @CustEmail VARCHAR(100),
    @CustPassword VARCHAR(500)     
AS
BEGIN
    -- Start a transaction
    BEGIN TRANSACTION;
    
    DECLARE @PinExists INT;  -- Variable to store the result of PIN existence check
	DECLARE @ActivatedtedPin VARCHAR(10); -- Variable to store the Activated PIN
    BEGIN TRY
        -- Loop until a unique PIN is generated
        WHILE 1 = 1
        BEGIN

            -- Generate a random 10-digit number within the range [1000000000,9000000000]
			SET @ActivatedtedPin = CAST((1000000000 + (ABS(CAST(CAST(NEWID() AS VARBINARY) AS INT)) % 8000000001)) AS VARCHAR(10));

            -- Check if the generated PIN already exists
            SELECT @PinExists = COUNT(*) FROM CustomerTable WHERE activatedPin = @ActivatedtedPin;

            -- If the PIN doesn't exist break the loop
            IF @PinExists = 0
            BEGIN
                BREAK;            
            END; 
			
        END;

        -- If everything is successful, insert the customer data
        INSERT INTO CustomerTable (custName, custAddress, custPhone, custEmail, custPassword, regDate, activatedPin) 
        VALUES (@CustName, @CustAddress, @CustPhone, @CustEmail, @CustPassword, FORMAT(GETDATE(), 'dd/MM/yyyy @ HH:mm:ss'), @ActivatedtedPin);

        -- Return the customer ID and the Activated pin of the newly inserted record
        SELECT SCOPE_IDENTITY() AS custId,@ActivatedtedPin as ActivatedPin;

        -- Commit the transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- If an error occurs, rollback the transaction
        ROLLBACK TRANSACTION;

        -- Log or handle the error
        INSERT INTO ErrorLog (QName, ErrorMessage, eDate) 
        VALUES ('SignUp Procedure', ERROR_MESSAGE(), FORMAT(GETDATE(), 'dd/MM/yyyy @ HH:mm:ss'));

        -- Re-throw the error to the calling code
        THROW;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[UpdateProfile]    Script Date: 02/05/2024 21:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateProfile]
    @CustName VARCHAR(200),
    @CustAddress VARCHAR(500),
    @CustPhone VARCHAR(50),
    @CustEmail VARCHAR(100),
    @CustPassword VARCHAR(500),  
    @CustId INT
AS
BEGIN
    -- Start a transaction
    BEGIN TRANSACTION;
      
    BEGIN TRY
        -- Check if the new email already exists
        IF NOT EXISTS (SELECT 1 FROM CustomerTable WHERE custEmail = @CustEmail AND custId != @CustId)
        BEGIN
            -- Update the customer data
            UPDATE CustomerTable 
            SET custName = @CustName, 
                custAddress = @CustAddress, 
                custPhone = @CustPhone, 
                custEmail = @CustEmail, 
                custPassword = @CustPassword 
            WHERE custId = @CustId;

            -- Commit the transaction
            COMMIT TRANSACTION;

            -- Return 1 to indicate successful update
            SELECT 1 AS Result;
        END
        ELSE
        BEGIN
            -- Commit the transaction (since no updates were made)
            COMMIT TRANSACTION;

            -- Return 0 to indicate failure due to existing email
            SELECT 0 AS Result;
        END
    END TRY
    BEGIN CATCH
        -- If an error occurs, rollback the transaction
        ROLLBACK TRANSACTION;

        -- Log or handle the error
        INSERT INTO ErrorLog (QName, ErrorMessage, eDate) 
        VALUES ('UpdateProfile Procedure', ERROR_MESSAGE(), FORMAT(GETDATE(), 'dd/MM/yyyy @ HH:mm:ss'));

        -- Re-throw the error to the calling code
        THROW;
    END CATCH;
END;
GO

```

5. Configure the application settings:

Modify the `app.json` file to include your database connection string, email sender credentials, authentication cookie name, and AES encryption key:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your connection string"
  },
  "AllowedHosts": "*",
  "Serilog": {
    "Using": [ "Serilog.Sinks.File" ],
    "MinimumLevel": {
      "Default": "Warning",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "Logs/FastFoodLog-.txt",
          "rollOnFileSizeLimit": true,
          "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter,Serilog.Formatting.Compact",
          "rollingInterval": "Day",
          "outputTemplate": "[ {Timestamp:dd/MM/yy HH:mm:ss} [{Level}]: {SourceContext} {Message}  Exception: {Exception} ]{NewLine}"
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithThreadId", "WithMachineName" ]
  },
  "Email": {
    "SenderEmail": "yourmail@gmail.com",
    "SenderPassword": "Password"
  },
  "CookieAuth": {
    "Name": "FastFoodCookieAuth"
  },
  "AES": {
    "Key": "@*FastFoodKey24#"
  }
}
```

6. Build the project:

```bash
dotnet build
```

7. Run the application:

```bash
dotnet run
```

## Usage

### Customer Controller

The `CustomerController` provides various actions related to customer management, including:

- `MyAccount`: View customer account details.
- `UpdateProfile`: Update customer profile information.
- `UploadImage`: Upload a profile image for the customer.
- `AddToCart`: Add items to the shopping cart.
- `ViewCart`: View the contents of the shopping cart.
- `RemoveCart`: Remove items from the shopping cart.
- `CheckOut`: Proceed to checkout and place an order.

### Account Controller

The `AccountController` manages user authentication and account-related actions, such as sign up, login, and password recovery.

### Home Controller

The `HomeController` handles home page rendering, about page, contact us page, and product details page.

## Contributing

Contributions are welcome! If you'd like to contribute to this project, please follow these steps:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -am 'Add some feature'`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Create a new Pull Request.

## License

This project is licensed under the [MIT License](LICENSE).
```
