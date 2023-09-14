using CourseRequest__.Net_Framework_.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseRequest__.Net_Framework_
{
    public partial class _ListRequest : Page
    {
        protected int requestCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InsertUsernameInDB();

                // получение количества ВСЕХ заявок
                requestCount = GetRequestCount();

                // получение и вывод ВСЕХ заявок в таблицу
                List<Request> requests = GetRequestsFromDatabase();
                RepeaterRequests.DataSource = requests;
                RepeaterRequests.DataBind();


                // вывод списка лет
                int currentYear = DateTime.Now.Year;
                int endYear = currentYear - 4;
                int startYear = currentYear;
                List<int> years = new List<int>();
                for (int year = startYear; year >= endYear; year--)
                {
                    years.Add(year);
                }
                yearSelect.DataSource = years;
                yearSelect.DataBind();
                yearSelect.Items.Insert(0, new ListItem("Весь период", "0"));

            }
        }

        protected void RepeaterRequests_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ShowDetails")
            {
                int requestId = Convert.ToInt32(e.CommandArgument);
                // Здесь вы можете получить данные о выбранной заявке по requestId

                Response.Write(requestId);
                // Перенаправление на страницу Detail.aspx с передачей параметров
                Response.Redirect($"~/Details.aspx?requestId={requestId}");
            }
        }

        protected void YearSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Обработка выбора года
        }

        //// Фильтры 
        //[System.Web.Services.WebMethod]
        //protected List<Request> ApplyFilters(string year, string status, string department, string courseBegin, string courseEnd, string fullName, string requestNumber)
        //{
        //    // Здесь выполните логику применения фильтров к данным из базы данных
        //    // И верните список данных, удовлетворяющих фильтрам
        //
        //    List<Request> filteredData = Request.FilterData(year, status, department, courseBegin, courseEnd, fullName, requestNumber);
        //
        //    return filteredData;
        //}

        protected List<Request> GetRequestsFromDatabase()
        {
            // Строка подключения к базе данных PostgreSQL
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

            List<Request> requests = new List<Request>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL-запрос для извлечения данных из таблицы 'Request'
                    string sql = "SELECT r.id, full_name, department, position, course_name, t.type, notation, s.status, course_start, course_end, year, u.username " +
                        "FROM public.request r " +
                        "JOIN public.type t ON t.id = r.course_type_id " +
                        "JOIN public.status s ON s.id = r.status_id JOIN public.users u ON u.id = r.user_id " +
                        "ORDER BY id DESC";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        string userName = GetUserName();
                        command.Parameters.AddWithValue("@User", userName);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Request request = new Request
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Full_Name = reader.GetString(reader.GetOrdinal("full_name")),
                                    Department = reader.GetString(reader.GetOrdinal("department")),
                                    Position = reader.GetString(reader.GetOrdinal("position")),
                                    Course_Name = reader.GetString(reader.GetOrdinal("course_name")),
                                    Course_Type = reader.GetString(reader.GetOrdinal("type")),
                                    Notation = reader.GetString(reader.GetOrdinal("notation")),
                                    Status = reader.GetString(reader.GetOrdinal("status")),
                                    Course_Start = reader.GetDateTime(reader.GetOrdinal("course_start")),
                                    Course_End = reader.GetDateTime(reader.GetOrdinal("course_end")),
                                    Year = reader.GetInt32(reader.GetOrdinal("year")),
                                    User = reader.GetString(reader.GetOrdinal("username"))

                                };

                                requests.Add(request);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок, логирование
                    Console.WriteLine("Произошла ошибка при получении заявок: " + ex.Message);
                    //Response.Write(ex.Message);
                }
            }
            
            return requests;
        }

        protected void InsertUsernameInDB()
        {
            // Получите значение из элемента ввода на вашей веб-странице (например, TextBox)
            string username = GetUserName(); // Замените "YourUsernameTextBox" на реальное имя элемента ввода

            // Проверьте, существует ли уже пользователь с таким именем в таблице users
            if (!UserExistsInDB(username))
            {
                // Если пользователь не существует, выполните вставку в таблицу users

                // Строка подключения к базе данных PostgreSQL
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // SQL-запрос для вставки нового пользователя
                        string sql = "INSERT INTO users (username, role_id) VALUES (@Username, @RoleId)";

                        using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                        {
                            // Задайте значения параметров для вставки
                            command.Parameters.AddWithValue("@Username", username);
                            command.Parameters.AddWithValue("@RoleId", 3); // Замените "YourRoleId" на соответствующий идентификатор роли

                            // Выполните запрос на вставку
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Обработка ошибок, логирование и т. д.
                        Console.WriteLine("Произошла ошибка при вставке пользователя: " + ex.Message);
                    }
                }
            }
        }

        // Функция для проверки существования пользователя в таблице users
        private bool UserExistsInDB(string username)
        {
            // Строка подключения к базе данных PostgreSQL
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL-запрос для проверки существования пользователя
                    string sql = "SELECT COUNT(*) FROM users WHERE username = @Username";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        int count = (int)command.ExecuteScalar();

                        // Если count больше нуля, пользователь уже существует
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок, логирование и т. д.
                    Console.WriteLine("Произошла ошибка при проверке существования пользователя: " + ex.Message);
                    return false; // По умолчанию считаем, что пользователь не существует
                }
            }
        }

        public int GetRequestCount()
        {
            // Строка подключения к базе данных PostgreSQL
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL-запрос для подсчета количества ВСЕХ заявок
                    string sql = "SELECT CAST(COUNT(*) AS INT) " +
                        "FROM public.request";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        int count = (int)command.ExecuteScalar();

                        return count;
                    }

                }
                catch (Exception ex)
                {
                    // Обработка ошибок, логирование
                    Console.WriteLine("Произошла ошибка при получении количества заявок: " + ex.Message);
                    Response.Write(ex);
                    return 0; // Возвращаем 0 в случае ошибки
                }
            }
        }

        protected string GetUserName()
        {
            string userName = $"{Dns.GetHostName()}\\{Environment.UserName}";
            return userName;
        }








    }
}