namespace WebApplication1.Library.DB
{
    public static class SQLConfig
    {
        public static string Connection { 
            get 
            {
                return "Server="+Host+";Database="+Database+";User Id="+Username+";password="+Password;
            } 
        }

        public static string Host { get; set; } = "localhost";

        public static string Database { get; set; } = "test_db";

        public static string Username { get; set; } = "root";
        public static string Password { get; set; } = "";
    }
}
