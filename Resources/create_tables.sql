/****** Object:  Table [dbo].[Users]    Script Date: 07/06/2017 11:33:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[EmailID] [nvarchar](255) NOT NULL,
	[DateBirth] [datetime2](7) NULL,
	[Password] [nvarchar](max) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[ActivationCode] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [EmailID], [DateBirth], [Password], [IsEmailVerified], [ActivationCode]) VALUES (2, N'Paul', N'Brust', N'ab.paul4@gmail.com', CAST(0x070000000000D6040B AS DateTime2), N'XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=', 1, N'e8c8f675-6f09-407f-bd79-65b5ea4dacca')
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [EmailID], [DateBirth], [Password], [IsEmailVerified], [ActivationCode]) VALUES (3, N'ккк', N'ккк', N'ккк@dd.com', CAST(0x070000000000303D0B AS DateTime2), N'aEh9wpUFKqecUw4oPOaYuMa7G0L/CUQlLhkQ2+zcVCU=', 0, N'18cadf5a-4530-4adf-9295-c13805c8b80f')
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [EmailID], [DateBirth], [Password], [IsEmailVerified], [ActivationCode]) VALUES (4, N'ккк', N'ккк', N'rrr@dd.com', CAST(0x070000000000303D0B AS DateTime2), N'ciOeiyHFsNFDW2cs4WNArLPZZyvPqJChUXpJWFPGE2Y=', 0, N'480a0cc8-15e5-4580-b1b4-5acd9297ff1a')
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [EmailID], [DateBirth], [Password], [IsEmailVerified], [ActivationCode]) VALUES (5, N'Paul', N'Brust', N'b.paul4@gmail.com', NULL, N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 1, N'913c72f3-e73b-4fe4-a008-e5e5de66b7ed')
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Default [DF__Users__FirstName__239E4DCF]    Script Date: 07/06/2017 11:33:29 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [FirstName]
GO
/****** Object:  Default [DF__Users__LastName__24927208]    Script Date: 07/06/2017 11:33:29 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [LastName]
GO
/****** Object:  Default [DF__Users__EmailID__25869641]    Script Date: 07/06/2017 11:33:29 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [EmailID]
GO
/****** Object:  Default [DF__Users__IsEmailVe__267ABA7A]    Script Date: 07/06/2017 11:33:29 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsEmailVerified]
GO
