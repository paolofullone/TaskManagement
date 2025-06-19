CREATE DATABASE TaskManagementDB;
GO

USE TaskManagementDB;
GO

-- Create USERS table if not exists
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'USERS')
BEGIN
    CREATE TABLE USERS (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE
    );
    PRINT 'Table USERS created successfully.';
END
GO

-- Create PROJECTS table if not exists
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PROJECTS')
BEGIN
    CREATE TABLE PROJECTS (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(255) NOT NULL
    );
    PRINT 'Table PROJECTS created successfully.';
END
GO

-- Create WORKTASKS table if not exists
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'WORKTASKS')
BEGIN
    CREATE TABLE WORKTASKS (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Title NVARCHAR(100) NOT NULL,
        Description NVARCHAR(100) NOT NULL,
        IsCompleted BIT NOT NULL DEFAULT 0,
        UserId INT NOT NULL,
        ProjectId INT NOT NULL,
        FOREIGN KEY (UserId) REFERENCES USERS(Id),
        FOREIGN KEY (ProjectId) REFERENCES PROJECTS(Id)    
    );
    PRINT 'Table WORKTASKS created successfully.';
END
GO

-- -- Clear existing data only if tables exist
-- BEGIN TRY
--     IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WORKTASKS')
--     BEGIN
--         DELETE FROM WORKTASKS;
--         PRINT 'Cleared WORKTASKS data.';
--     END
    
--     IF EXISTS (SELECT * FROM sys.tables WHERE name = 'USERS')
--     BEGIN
--         DELETE FROM USERS;
--         PRINT 'Cleared USERS data.';
--     END
    
--     IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PROJECTS')
--     BEGIN
--         DELETE FROM PROJECTS;
--         PRINT 'Cleared PROJECTS data.';
--     END
-- END TRY
-- BEGIN CATCH
--     PRINT 'Error clearing tables: ' + ERROR_MESSAGE();
-- END CATCH
-- GO

-- Insert Users with conflict handling
BEGIN TRY
    INSERT INTO USERS (Name, Email) VALUES
    ('Paolo', 'paolo@example.com'),
    ('Kely', 'kely@example.com'),
    ('Luca', 'luca@example.com'),
    ('Manu', 'manu@example.com');
    PRINT 'Inserted user data successfully.';
END TRY
BEGIN CATCH
    PRINT 'Error inserting users: ' + ERROR_MESSAGE();
END CATCH
GO

-- Insert Projects with conflict handling
BEGIN TRY
    INSERT INTO PROJECTS (Name, Description) VALUES
    ('Website Redesign', 'Complete overhaul of company website'),
    ('Mobile App', 'Development of new cross-platform mobile application');
    PRINT 'Inserted project data successfully.';
END TRY
BEGIN CATCH
    PRINT 'Error inserting projects: ' + ERROR_MESSAGE();
END CATCH
GO

-- Insert Tasks with transaction and error handling
BEGIN TRANSACTION;
BEGIN TRY
    -- Paolo's Tasks
    INSERT INTO WORKTASKS (Title, Description, IsCompleted, UserId, ProjectId) VALUES
    ('Design Homepage', 'Create new homepage mockup', 0, 1, 1),
    ('Implement Auth', 'Setup JWT authentication', 1, 1, 2),
    ('Database Schema', 'Design new database structure', 0, 1, 1),
    ('API Endpoints', 'Create REST API endpoints', 0, 1, 2),
    ('Testing', 'Write unit tests', 1, 1, 1);

    -- Kely's Tasks
    INSERT INTO WORKTASKS (Title, Description, IsCompleted, UserId, ProjectId) VALUES
    ('UI Components', 'Build reusable React components', 0, 2, 1),
    ('App Navigation', 'Implement bottom tab navigation', 1, 2, 2),
    ('State Management', 'Configure Redux store', 0, 2, 2),
    ('Performance Audit', 'Identify performance bottlenecks', 0, 2, 1),
    ('Accessibility Check', 'Ensure WCAG compliance', 1, 2, 1);

    -- Luca's Tasks
    INSERT INTO WORKTASKS (Title, Description, IsCompleted, UserId, ProjectId) VALUES
    ('CI/CD Pipeline', 'Setup GitHub Actions', 0, 3, 2),
    ('Dockerize App', 'Create Docker containers', 1, 3, 2),
    ('Load Testing', 'Conduct stress tests', 0, 3, 1),
    ('Security Scan', 'Run vulnerability scan', 0, 3, 1),
    ('Deployment', 'Deploy to production', 1, 3, 2);

    -- Manu's Tasks
    INSERT INTO WORKTASKS (Title, Description, IsCompleted, UserId, ProjectId) VALUES
    ('Content Migration', 'Move old content to new CMS', 0, 4, 1),
    ('Analytics', 'Implement Google Analytics', 1, 4, 1),
    ('SEO Optimization', 'Improve search rankings', 0, 4, 1),
    ('Email Templates', 'Design transactional emails', 0, 4, 2),
    ('Documentation', 'Write technical docs', 1, 4, 2);

    COMMIT TRANSACTION;
    PRINT 'All tasks inserted successfully.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error inserting tasks: ' + ERROR_MESSAGE();
END CATCH
GO
