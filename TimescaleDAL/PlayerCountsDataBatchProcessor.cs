using Npgsql;
using PostgreSQLCopyHelper;
using System.Collections.Generic;

namespace TimescaleDAL
{
    public class PlayerCountsDataBatchProcessor
    {
        private readonly string _connectionString;

        public PlayerCountsDataBatchProcessor(string connString)
        {
            _connectionString = connString;
        }

        public bool TestConnection()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using (var cmd = new NpgsqlCommand("SELECT EXISTS(SELECT * FROM information_schema.tables WHERE table_schema = 'battlefield' AND table_name = 'player_counts');", connection))
            {
                var result = (bool)cmd.ExecuteScalar();
                if (!result) return false;
            }
            connection.Close();
            return true;
        }

        public ulong WriteToDatabase(IEnumerable<PlayerCountsData> entities)
        {
            var copyHelper = new PostgreSQLCopyHelper<PlayerCountsData>("battlefield", "player_counts")
                .MapVarchar("server_guid", x => x.ServerGuid)
                .MapTimeStamp("timestamp", x => x.TimeStamp)
                .MapVarchar("map", x => x.Map)
                .MapBigInt("mapmode", x => x.MapMode)
                .MapInteger("battlelog_queue", x => x.BattlelogQueue)
                .MapInteger("battlelog_players", x => x.BattlelogPlayers)
                .MapInteger("battlelog_spectators", x => x.BattlelogSpectators)
                .MapInteger("companion_queue", x => x.CompanionQueue)
                .MapInteger("companion_players", x => x.CompanionPlayers)
                .MapInteger("companion_spectators", x => x.CompanionSpectators)
                .MapInteger("snapshot_queue", x => x.SnapshotQueue)
                .MapInteger("snapshot_players", x => x.SnapshotPlayers);

            return WriteToDatabase(copyHelper, entities);
        }

        private ulong WriteToDatabase(PostgreSQLCopyHelper<PlayerCountsData> copyHelper, IEnumerable<PlayerCountsData> entities)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            // Returns count of rows written 
            return copyHelper.SaveAll(connection, entities);
        }
    }
}
