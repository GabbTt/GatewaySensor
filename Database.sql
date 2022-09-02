

CREATE TABLE RequestLog
  (   
     [id]         [INT] IDENTITY(1, 1) NOT NULL primary key,   
     [timestamp]  [DATETIME] NOT NULL,   
     [level]      [VARCHAR](100) NOT NULL,   
     [logger]     [VARCHAR](1000) NOT NULL,   
     [message]    [VARCHAR](3600) NOT NULL,   
     [userid]     [INT] NULL,   
     [exception]  [VARCHAR](3600) NULL,   
     [stacktrace] [VARCHAR](3600) NULL,   
     
  )   