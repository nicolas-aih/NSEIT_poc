namespace Database
{
    public static class Databases
    {
        public static dynamic GetDatabase(string connectionString)
        {
            // Basic implementation - replace with actual database logic
            return new System.Data.SqlClient.SqlConnection(connectionString);
        }
    }
}