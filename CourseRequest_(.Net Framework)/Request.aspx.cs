using CourseRequest__.Net_Framework_.Models;
using CourseRequest__;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Web.Services;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Profile;
using System.Web.Mvc;

namespace CourseRequest__.Net_Framework_
{
    public partial class _Request : Page
    {
        protected int requestCount;

        [AllowAnonymous]
        protected void Page_Load(object sender, EventArgs e)
        {
            int role = GetRoleByUsername();
            if (role != 1)
            {
                Response.Redirect($"~/ListRequest.aspx");

            }

            if (!IsPostBack)
            {

                // получение и вывод всех НОВЫХ заявок в таблицу
                List<Request> requests = GetRequestsFromDatabase();
                RepeaterRequests.DataSource = requests;
                RepeaterRequests.DataBind();


                // получение количества НОВЫХ заявок
                requestCount = requests.Count;

                // получение ФИО сотрудников
                GetFioDropDownList();
            }
        }

        protected void GetFioDropDownList()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString_MSSQL"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Выполните SQL-запрос для получения данных для выпадающего списка
                string query = "SELECT FIO, Dept, Position FROM custom_view_employees"; // Замените на ваш запрос
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                // Очистите существующие элементы в списке
                Full_Name.Items.Clear();

                // Добавьте пустой элемент (первый элемент списка)
                Full_Name.Items.Add(new ListItem("", ""));

                // Добавьте элементы в список на основе данных из базы данных
                while (reader.Read())
                {
                    string fio = reader["FIO"].ToString();
                    string dept = reader["Dept"].ToString();
                    string position = reader["Position"].ToString();

                    // Создайте элемент списка с атрибутами data-dept и data-position
                    ListItem listItem = new ListItem(fio, fio);
                    listItem.Attributes["data-dept"] = dept;
                    listItem.Attributes["data-position"] = position;

                    Full_Name.Items.Add(listItem);
                }

                connection.Close();
            }
        }







        protected int GetRoleByUsername()
        {
            string username = GetUserName();
            int role = -1;

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT role_id FROM users WHERE username = @Username";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                role = reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении роли пользователя: " + ex.Message);
            }

            return role;
        }


        protected void CreateRequestButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем значения из элементов формы
                string fullName = Full_Name.Value;

                string department = Department.Value;
                string position = Position.Value;
                string courseName = Course_Name.Value;
                int courseType = Convert.ToInt32(Course_Type.Value);
                string notation = Notation.Value;
                int status = Convert.ToInt32(Status.Value);
                DateTime courseStart = DateTime.Parse(Course_Start.Value);
                DateTime courseEnd = DateTime.Parse(Course_End.Value);
                int year = courseStart.Year;
                string user = GetUserName(); // Имя пользователя Windows

                // Строка подключения к базе данных PostgreSQL
                string connectionString = ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос для вставки данных в таблицу 'request'
                    string sql = @"INSERT INTO public.request (full_name, department, position, course_name, course_type_id, notation, status_id, course_start, course_end, year, creator) 
                       VALUES (@FullName, @Department, @Position, @CourseName, @CourseType, @Notation, @Status, @CourseStart, @CourseEnd, @Year, @User)";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@Department", department);
                        command.Parameters.AddWithValue("@Position", position);
                        command.Parameters.AddWithValue("@CourseName", courseName);
                        command.Parameters.AddWithValue("@CourseType", courseType);
                        command.Parameters.AddWithValue("@Notation", notation);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@CourseStart", courseStart);
                        command.Parameters.AddWithValue("@CourseEnd", courseEnd);
                        command.Parameters.AddWithValue("@Year", year);
                        command.Parameters.AddWithValue("@User", user);

                        command.ExecuteNonQuery();
                    }
                }

                // После вставки данных, вы можете перезагрузить страницу или выполнить другие необходимые действия.
                Response.Redirect(Request.RawUrl); // Этот код перезагрузит текущую страницу.
            }
            catch (FormatException ex)
            {
                // Обработка ошибки парсинга
                Console.WriteLine("Неправильно введены значения в поля: " + ex.Message);
                // Здесь вы можете предпринять необходимые действия, например, вывести сообщение об ошибке пользователю или установить значение по умолчанию.
            }
            catch (Exception) { }
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

        protected string GetUserName()
        {
            string userName = Page.User.Identity.Name;
            return userName;
        }

        public int GetUserId()
        {
            int userId = -1; // Значение по умолчанию, если пользователь не найден

            // Строка подключения к базе данных PostgreSQL
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL-запрос для получения id пользователя по его имени
                    string sql = "SELECT id FROM public.users WHERE username = @Username";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        string username = GetUserName();
                        command.Parameters.AddWithValue("@Username", username);

                        var result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            userId = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок, логирование
                    Console.WriteLine("Произошла ошибка при получении id пользователя: " + ex.Message);
                }
            }

            return userId;
        }


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
                    string sql = "SELECT r.id, full_name, department, position, course_name, t.type, notation, s.status, course_start, course_end, year, creator " +
                        "FROM public.request r " +
                        "JOIN public.type t ON t.id = r.course_type_id " +
                        "JOIN public.status s ON s.id = r.status_id " +
                        "WHERE s.status = 'Новая' AND creator = @User " +
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
                                    User = reader.GetString(reader.GetOrdinal("creator"))

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



        public int GetRequestCount()
        {
            // Строка подключения к базе данных PostgreSQL
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL-запрос для подсчета количества заявок со статусом "Новая"
                    string sql = "SELECT CAST(COUNT(*) AS INT) " +
                        "FROM public.request r " +
                        "JOIN public.status s ON s.id = r.status_id " +
                        "WHERE s.status = 'Новая' AND r.creator = @User";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        string userName = GetUserName();
                        command.Parameters.AddWithValue("@User", userName);
                        //Response.Write(userName);

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








    }
}