GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'90F449E3-5371-43F8-A651-C432AD56C35C', N'User', N'USER', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'BC8709EE-634E-45AA-A6E1-A21FA4C92788', N'Admin', N'ADMIN', NULL)
GO
INSERT [dbo].[AspNetUsers] ([Id], [Discriminator], [CreateDate], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'7cd738cc-acec-44ff-a692-f0987a9e2354', N'ApplicationUser', CAST(N'2025-06-04T00:54:47.1265931' AS DateTime2), N'admin', N'ADMIN', N'admin@local', N'ADMIN@LOCAL', 0, N'AQAAAAEAACcQAAAAEEF3nSetZDimUWHnRpfVf5BRPALsDAm7PLcW5OAjKk2vrncxjEvqlz0ZZ0b3f6PEqg==', N'OMFNNPDGFAANJGHQKIKG3WSEMUFIV64X', N'6878e88d-3583-41bb-a924-dc53a327938c', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'7cd738cc-acec-44ff-a692-f0987a9e2354', N'BC8709EE-634E-45AA-A6E1-A21FA4C92788')
GO
INSERT [dbo].[BlogPosts] ([PostId], [Title], [PostDate], [AuthorId], [Content], [PostImgSrc]) VALUES (N'420153b0-d029-4bc6-ba09-808d4b5ffc51', N'Service Start!', CAST(N'2025-12-04T20:20:45.1318605' AS DateTime2), N'7cd738cc-acec-44ff-a692-f0987a9e2354', N'Welcome! This post is only a test.
Wish u a good day', N'420153b0-d029-4bc6-ba09-808d4b5ffc51_welcome.gif')
GO
SET IDENTITY_INSERT [dbo].[ProgressStates] ON 

INSERT [dbo].[ProgressStates] ([ProgressId], [State]) VALUES (1, N'All')
INSERT [dbo].[ProgressStates] ([ProgressId], [State]) VALUES (2, N'Playing')
INSERT [dbo].[ProgressStates] ([ProgressId], [State]) VALUES (3, N'Complete')
INSERT [dbo].[ProgressStates] ([ProgressId], [State]) VALUES (4, N'Planning')
INSERT [dbo].[ProgressStates] ([ProgressId], [State]) VALUES (5, N'Awaiting')
INSERT [dbo].[ProgressStates] ([ProgressId], [State]) VALUES (6, N'Dropped')
SET IDENTITY_INSERT [dbo].[ProgressStates] OFF
GO
SET IDENTITY_INSERT [dbo].[Producents] ON 

INSERT [dbo].[Producents] ([ProdId], [CompanyName]) VALUES (1, N'Riot Games')
INSERT [dbo].[Producents] ([ProdId], [CompanyName]) VALUES (2, N'Valve')
SET IDENTITY_INSERT [dbo].[Producents] OFF
GO
INSERT [dbo].[GameDatas] ([GameId], [Name], [ProducentId], [ReleaseDate], [PegiAge], [ImgSrc]) VALUES (N'05795fe4-dbbd-4179-808b-c24693080733', N'League of Legends', 1, CAST(N'2009-09-27T00:00:00.0000000' AS DateTime2), 16, N'05795fe4-dbbd-4179-808b-c24693080733_league-of-legends-pc-game-cover.jpg')
INSERT [dbo].[GameDatas] ([GameId], [Name], [ProducentId], [ReleaseDate], [PegiAge], [ImgSrc]) VALUES (N'908c118e-d09c-44bf-9a0a-1e513f08cc0b', N'Counter-Strike: Global Offensive', 2, CAST(N'2014-08-23T00:00:00.0000000' AS DateTime2), 16, N'908c118e-d09c-44bf-9a0a-1e513f08cc0b_fb_image.png')
GO
SET IDENTITY_INSERT [dbo].[UsersGamesDatas] ON 

INSERT [dbo].[UsersGamesDatas] ([DataId], [UserId], [GameId], [StateId], [AddDate]) VALUES (3, N'7cd738cc-acec-44ff-a692-f0987a9e2354', N'05795fe4-dbbd-4179-808b-c24693080733', 5, CAST(N'2025-06-04T01:24:48.5794953' AS DateTime2))
INSERT [dbo].[UsersGamesDatas] ([DataId], [UserId], [GameId], [StateId], [AddDate]) VALUES (7, N'7cd738cc-acec-44ff-a692-f0987a9e2354', N'908c118e-d09c-44bf-9a0a-1e513f08cc0b', 5, CAST(N'2025-12-04T20:14:33.4985141' AS DateTime2))
SET IDENTITY_INSERT [dbo].[UsersGamesDatas] OFF
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250603224547_InitalMigration', N'6.0.36')
GO
