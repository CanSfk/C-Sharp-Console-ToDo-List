using System.Data.SqlClient;

namespace ToDoList_Projesi
{
    public class DatabaseOperations
    {
        private readonly SqlConnection _connection;
        private readonly string tableName = "ToDoList";

        public DatabaseOperations(SqlConnection connection)
        {
            _connection = connection;
        }

        public void CrateToDoListTable()
        {
            string createTableQuery = $@"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}')
            BEGIN
            CREATE TABLE {tableName}(
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name VARCHAR(200),
                State BIT DEFAULT 0
            );
            END
            ";

            using SqlCommand command = new(createTableQuery, _connection);
            command.ExecuteNonQuery();
        }

        public void GetAllList()
        {
            string listQuery = $"Select * from {tableName}";

            using SqlCommand command = new(listQuery, _connection);

            using SqlDataReader reader = command.ExecuteReader();

            Console.Clear();
            System.Console.WriteLine("\t\t\t\t\t GOREV LISTESI");
            System.Console.WriteLine("----------------------------------------------------------------------------------------------------------");
            if (reader.HasRows)
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    bool state = reader.GetBoolean(2);

                    Console.WriteLine($"Gorev Id :{id}\t\t  -  Gorev :{name}\t\t  -  Durum :{(state ? "Tamamlandi" : "Tamamlanmadi")}");
                }
            else
                System.Console.WriteLine("Herhangi bir gorev bulunamadi");
            System.Console.WriteLine("----------------------------------------------------------------------------------------------------------");
        }

        public void AddToDo()
        {
            Console.Write("Eklemek istediginiz gorevi yaziniz :");
            string name = Console.ReadLine() ?? "";

            string insertQuery = $"INSERT INTO {tableName} (Name) VALUES (@Name)";
            using SqlCommand command = new(insertQuery, _connection);
            command.Parameters.AddWithValue("@Name", name);
            int status = command.ExecuteNonQuery();

            HandleMessage(status, "Ekleme islemi basarili", "Ekleme islemi basarisiz");
        }

        public void ComplateToDo()
        {
            System.Console.Write("Tamamlamak istediginiz gorevin Id'sini giriniz :");
            string id = Console.ReadLine() ?? "-1";

            string complateQuery = $"Update {tableName} set State=1 WHERE Id=@Id";
            using SqlCommand command = new(complateQuery, _connection);
            command.Parameters.AddWithValue("@Id", id);

            int status = command.ExecuteNonQuery();

            HandleMessage(status, "Gorev tamamlandi", "Gorev tamamlama islemi basarisiz");
        }

        public void RemoveToDo()
        {
            System.Console.Write("Silmek istediginiz gorevin id'sini giriniz :");
            string id = Console.ReadLine() ?? "-1";

            string removeQuery = $"Delete {tableName} WHERE Id=@Id";
            using SqlCommand command = new(removeQuery, _connection);
            command.Parameters.AddWithValue("@Id", id);

            int status = command.ExecuteNonQuery();

            HandleMessage(status, "Silme islemi basarili", "Silme islemi basarisiz");
        }

        public void TruncateToDoListTable()
        {
            System.Console.Write("Sifre :");
            string password = Console.ReadLine() ?? "-1";

            if (password == "1234")
            {
                string truncateQuery = "TRUNCATE TABLE ToDoList";
                using SqlCommand command = new(truncateQuery, _connection);
                command.ExecuteNonQuery();

                using SqlDataReader reader = command.ExecuteReader();

                HandleMessage(reader.HasRows, "Tablo temizleme islemi basarisiz", "Tablo basariyla temizlendi");
            }
            else
            {
                Console.Clear();
                System.Console.WriteLine("!!! Hatali sifre girdiniz !!!");
            }
        }

        private static void HandleMessage(object status, string success, string error)
        {
            Console.Clear();
            if (status is int intStatus)
            {
                if (intStatus > 0)
                    Console.WriteLine($"********* {success} *********");
                else
                    Console.WriteLine($"!!!!!!!!!! {error} !!!!!!!!!!");
            }
            else if (status is bool boolStatus)
            {
                if (boolStatus)
                    Console.WriteLine($"********* {success} *********");
                else
                    Console.WriteLine($"!!!!!!!!!! {error} !!!!!!!!!!");
            }
        }
    }
}