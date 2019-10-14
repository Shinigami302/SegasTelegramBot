CREATE TABLE [dbo].[BotReactions]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Reactions] VARCHAR(MAX) NULL, 
    [Users] VARCHAR(MAX) NULL, 
    [HowAreYou] VARCHAR(MAX) NULL, 
    [WhatYouThink] VARCHAR(MAX) NULL
)
