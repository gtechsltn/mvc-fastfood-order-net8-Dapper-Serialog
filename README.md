```markdown
# FastFood Service

This repository contains the source code for the FastFood service. It includes functionality for managing customer accounts, shopping carts, and orders in a fast food delivery system.

## Table of Contents
- [Features](#features)
- [Setup](#setup)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Features

- Authentication and Authorization: Utilizes cookie authentication for customer login with authorization policies.
- Customer Account Management: Allows customers to view and update their profile information.
- Shopping Cart Management: Enables customers to add items to their shopping carts, view the cart contents, and remove items.
- Order Placement: Facilitates the process of placing orders securely.

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
/****** Object:  Table [dbo].[AdminTable]    Script Date: 02/05/2024 08:49:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminTable](
    [AdminId] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [AdminId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartTable]    Script Date: 02/05/2024 08:49:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartTable](
    [CustID] [int] NOT NULL,
    [FoodID] [int] NOT NULL,
    [quantity] [int] NOT NULL,
    [price] [float] NOT NULL,
      NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryTable]    Script Date: 02/05/2024 08:49:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryTable](
      NOT NULL,
      NOT NULL,
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
/****** Object:  Table [dbo].[CustomerTable]    Script Date: 02/05/2024 08:49:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerTable](
    [custId] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
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
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 02/05/2024 08:49:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorLog](
      NOT NULL,
    [ErrorMessage] [varchar](max) NOT NULL,
      NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FoodItemTable]    Script Date: 02/05/2024 08:49:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FoodItemTable](
      NOT NULL,
    [catID] [int] NULL,
      NULL,
    [price] [float] NOT NULL,
      NOT NULL,
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
/****** Object:  Table [dbo].[OrderTable]    Script Date: 02/05/2024 08:49:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderTable](
    [CustID] [int] NOT NULL,
    [FoodID] [int] NOT NULL,
    [quantity] [int] NOT NULL,
    [price] [float] NOT NULL,
      NOT NULL,
    [OrderID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_OrderTable] PRIMARY KEY CLUSTERED 
(
    [OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentTable]    Script Date: 02/05/2024 08:49:42 ******/
SET ANSI_NULLS ON
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

5. Build the project:

```bash
dotnet build
```

6. Run the application:

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
