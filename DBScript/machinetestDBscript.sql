USE [machine_test]
GO
/****** Object:  Table [dbo].[tbl_CategoryMaster]    Script Date: 3/8/2025 6:32:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CategoryMaster](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](50) NULL,
	[Inserted_Date] [datetime] NULL,
 CONSTRAINT [PK_Category_Master] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ProductMaster]    Script Date: 3/8/2025 6:32:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ProductMaster](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](50) NULL,
	[CategoryId] [int] NOT NULL,
	[Inserted_Date] [datetime] NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tbl_CategoryMaster] ON 

INSERT [dbo].[tbl_CategoryMaster] ([CategoryId], [CategoryName], [Inserted_Date]) VALUES (1, N'Software', CAST(N'2025-03-08T18:11:11.887' AS DateTime))
INSERT [dbo].[tbl_CategoryMaster] ([CategoryId], [CategoryName], [Inserted_Date]) VALUES (2, N'Hardware', CAST(N'2025-03-08T18:11:20.213' AS DateTime))
SET IDENTITY_INSERT [dbo].[tbl_CategoryMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_ProductMaster] ON 

INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (1, N'Keybord', 2, CAST(N'2025-03-08T18:13:45.683' AS DateTime))
INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (2, N'Mouse', 2, CAST(N'2025-03-08T18:13:58.163' AS DateTime))
INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (3, N'Monitor', 2, CAST(N'2025-03-08T18:14:22.783' AS DateTime))
INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (4, N'Laptop', 2, CAST(N'2025-03-08T18:14:34.727' AS DateTime))
INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (5, N'Pendrive', 2, CAST(N'2025-03-08T18:14:44.883' AS DateTime))
INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (6, N'DataCable', 2, CAST(N'2025-03-08T18:15:13.180' AS DateTime))
INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (7, N'Antivirus', 1, CAST(N'2025-03-08T18:15:31.510' AS DateTime))
INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (8, N'VLC', 1, CAST(N'2025-03-08T18:15:47.697' AS DateTime))
INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (9, N'Microsoft windows 10', 1, CAST(N'2025-03-08T18:16:22.460' AS DateTime))
INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (10, N'VPN', 1, CAST(N'2025-03-08T18:16:46.940' AS DateTime))
INSERT [dbo].[tbl_ProductMaster] ([ProductId], [ProductName], [CategoryId], [Inserted_Date]) VALUES (11, N'Anydesk', 1, CAST(N'2025-03-08T18:17:04.520' AS DateTime))
SET IDENTITY_INSERT [dbo].[tbl_ProductMaster] OFF
GO
ALTER TABLE [dbo].[tbl_CategoryMaster] ADD  CONSTRAINT [DF_Category_Master_Inserted_Date]  DEFAULT (getdate()) FOR [Inserted_Date]
GO
ALTER TABLE [dbo].[tbl_ProductMaster] ADD  CONSTRAINT [DF_Product_Master_Inserted_Date]  DEFAULT (getdate()) FOR [Inserted_Date]
GO
ALTER TABLE [dbo].[tbl_ProductMaster]  WITH CHECK ADD  CONSTRAINT [FK_Product_Master_Category_Master] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[tbl_CategoryMaster] ([CategoryId])
GO
ALTER TABLE [dbo].[tbl_ProductMaster] CHECK CONSTRAINT [FK_Product_Master_Category_Master]
GO
/****** Object:  StoredProcedure [dbo].[sp_getproductpages]    Script Date: 3/8/2025 6:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[sp_getproductpages]
	@Pagenumber int,
	@Pagesize int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    declare @Offset int = (@Pagenumber-1)*@Pagesize;

	select p.ProductId, p.ProductName, p.CategoryId,c.CategoryName from tbl_ProductMaster p
	left join tbl_CategoryMaster c on c.CategoryId = p.CategoryId
	order by p.ProductId
	OFFSET @offset Rows fetch next @Pagesize Rows only;

END
GO
