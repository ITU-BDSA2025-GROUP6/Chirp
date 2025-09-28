# Creates the database locally in temp
sqlite3 /tmp/chirp.db < data/schema.sql
sqlite3 /tmp/chirp.db < data/dump.sql
