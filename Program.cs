using System.Data.SqlClient;
using ToDoList_Projesi;

internal class Program
{
    private static void Main(string[] args)
    {
        string connectionString = "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;";

        SqlConnection connection = new(connectionString);
        connection.Open();

        ConsoleOperations co = new(connection);
        co.ShowMenu();
    }
}