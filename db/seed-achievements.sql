-- Seeds the default achievements (safe to re-run: existing names are skipped).
-- Run: sqlcmd -S localhost -d MindTrackDB -E -C -i db/seed-achievements.sql
SET NOCOUNT ON;
BEGIN TRANSACTION;

-- Remove swagger placeholder rows (only if nobody has unlocked them).
DELETE a
FROM dbo.Achievement a
WHERE a.Name = N'string'
  AND NOT EXISTS (SELECT 1 FROM dbo.UserAchievement ua WHERE ua.AchievementId = a.Id);

;WITH Seed (Name, Description, PointsReward) AS (
    SELECT * FROM (VALUES
        -- Journaling milestones
        (N'First Words',          N'Record your very first video journal entry.',                                  10),
        (N'Getting Comfortable',  N'Record 5 journal entries and get used to talking to the camera.',              25),
        (N'Finding Your Voice',   N'Record 10 journal entries. Your own words are getting easier to find.',        50),
        (N'Habit Builder',        N'Record 25 journal entries and make reflection part of your routine.',         100),
        (N'Storyteller',          N'Record 50 journal entries. Your journal is now a story worth rereading.',     200),
        (N'Century Club',         N'Record 100 journal entries. A true commitment to knowing yourself.',          400),
        -- Streaks
        (N'Three-Day Streak',     N'Journal 3 days in a row.',                                                      30),
        (N'Week Warrior',         N'Journal every day for a full week.',                                            75),
        (N'Fortnight Focus',      N'Keep your journaling streak alive for 14 days.',                               150),
        (N'Monthly Mindset',      N'Journal every day for 30 days in a row.',                                      300),
        (N'Unbreakable',          N'Reach a 100-day journaling streak. Nothing stops you.',                        500),
        -- Emotional awareness
        (N'Emotion Explorer',     N'Open the emotion analysis of one of your entries for the first time.',          15),
        (N'Emotional Range',      N'Record entries that show 5 different emotions.',                                60),
        (N'Mood Mapper',          N'Review your emotional trends across a full week of entries.',                   40),
        (N'Self-Aware',           N'Spot a recurring emotional pattern across 10 of your entries.',                 80),
        -- Reflection
        (N'Deep Thinker',         N'Answer your first reflection question.',                                        15),
        (N'Reflective Mind',      N'Answer 25 reflection questions.',                                              100),
        (N'Honest Moment',        N'Complete a reflection after an entry where you felt overwhelmed or low.',       50),
        -- Practices
        (N'First Breath',         N'Complete your first suggested practice.',                                       15),
        (N'Calm Seeker',          N'Complete 5 breathing or grounding exercises.',                                  50),
        (N'Practice Makes Progress', N'Complete 10 practices recommended for you.',                                 75),
        (N'Mindful Master',       N'Complete 50 practices. Calm is becoming a skill.',                             250),
        -- Fun and timing
        (N'Night Owl',            N'Record an entry after 10 PM to wind down your day.',                            20),
        (N'Early Bird',           N'Record an entry before 8 AM to start the day with intention.',                  20),
        (N'Full Circle',          N'Journal, reflect and complete a practice, all in one day.',                    120)
    ) AS v (Name, Description, PointsReward)
)
INSERT INTO dbo.Achievement (Id, Name, Description, PointsReward)
SELECT NEWID(), s.Name, s.Description, s.PointsReward
FROM Seed s
WHERE NOT EXISTS (SELECT 1 FROM dbo.Achievement a WHERE a.Name = s.Name);

COMMIT TRANSACTION;

SELECT COUNT(*) AS AchievementCount FROM dbo.Achievement;
