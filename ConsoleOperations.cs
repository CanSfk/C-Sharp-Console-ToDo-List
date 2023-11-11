using System.Data.SqlClient;

namespace ToDoList_Projesi
{
    public class ConsoleOperations
    {
        private readonly SqlConnection _connection;
        private readonly DatabaseOperations db;

        public ConsoleOperations(SqlConnection connection)
        {
            _connection = connection;
            db = new(_connection);
            db.CrateToDoListTable();
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n1. Gorevleri listele");
                Console.WriteLine("2. Gorev Ekle");
                Console.WriteLine("3. Gorevi Tamamla");
                Console.WriteLine("4. Gorevi Sil");
                Console.WriteLine("5. Tabloyu Temizle");
                Console.WriteLine("6. Cikis Yap");

                Console.Write("\nYapmak istediginiz islem nedir :");
                string input = Console.ReadLine() ?? "-1";

                switch (input)
                {
                    case "1":
                        db.GetAllList();
                        break;
                    case "2":
                        db.AddToDo();
                        break;
                    case "3":
                        db.GetAllList();
                        db.ComplateToDo();
                        break;
                    case "4":
                        db.GetAllList();
                        db.RemoveToDo();
                        break;
                    case "5":
                        db.TruncateToDoListTable();
                        break;
                    case "6":
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Hatali istek");
                        break;
                }
            }
        }


    }
}