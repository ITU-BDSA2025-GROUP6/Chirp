-- Ensure the two users exist (won't error if already there)
INSERT OR IGNORE INTO user (user_id, username, email) VALUES (1, 'Helge',  'ropf@itu.dk');
INSERT OR IGNORE INTO user (user_id, username, email) VALUES (2, 'Adrian', 'adho@itu.dk');

-- Insert 100 messages alternating between user 1 (Helge) and user 2 (Adrian)
WITH RECURSIVE seq(n) AS (
    SELECT 1
    UNION ALL
    SELECT n + 1 FROM seq WHERE n < 100
)
INSERT INTO message (author_id, text, pub_date)
SELECT
    CASE WHEN (n % 2) = 1 THEN 1 ELSE 2 END AS author_id,
    'Auto message #' || n || ' from ' ||
    CASE WHEN (n % 2) = 1 THEN 'Helge' ELSE 'Adrian' END AS text,
    1690892208 + (n - 1) * 300  -- bump timestamp by 5 minutes each message
FROM seq;
