--
-- Schema
--
CREATE SCHEMA IF NOT EXISTS battlefield AUTHORIZATION postgres;

--
-- Tables
--
CREATE TABLE IF NOT EXISTS battlefield.player_counts
(
    server_guid VARCHAR(36) NOT NULL,
    timestamp TIMESTAMPTZ NOT NULL,
    map VARCHAR(255) NULL,
    mapmode bigint NULL,
    battlelog_queue integer NULL,
    battlelog_players integer NULL,
    battlelog_spectators integer NULL,
    companion_queue integer NULL,
    companion_players integer NULL,
    companion_spectators integer NULL,
    snapshot_queue integer NULL,
    snapshot_players integer NULL
);

--
-- Convert to hypertable
--
SELECT create_hypertable('battlefield.player_counts', 'timestamp', if_not_exists => TRUE);
