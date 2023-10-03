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
using System.Threading.Tasks;

namespace CourseRequest__.Net_Framework_
{
    public partial class _Request : Page
    {
        protected int requestCount;

        [AllowAnonymous]
        protected void Page_Load(object sender, EventArgs e)
        {
            int role =  GetRoleByUsername();
            if (role != 1)
            {
                Response.Redirect($"~/ListRequest.aspx");

            }

            if (!IsPostBack)
            {

                // получение и вывод всех НОВЫХ заявок в таблицу
                List<Request> requests =  GetRequestsFromDatabase();
                RepeaterRequests.DataSource = requests;
                RepeaterRequests.DataBind();


                // получение количества НОВЫХ заявок
                requestCount = requests.Count;

                // получение ФИО сотрудников
                GetFioDropDownList();

                // получение названий курсов
                GetCourseNameDropDownList();

                // получение типов курсов
                GetCourseTypeDropDownList();

                // получение статусов заявок
                GetCourseStatusDropDownList();
            }
        }


        protected void GetCourseStatusDropDownList()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Выполните SQL-запрос для получения данных для выпадающего списка
                    string query = "SELECT id, status FROM public.status ORDER BY id ASC"; // Замените на ваш запрос

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        // Очистите существующие элементы в списке
                        Status.Items.Clear();

                        // Добавьте элементы в список на основе данных из базы данных
                        while (reader.Read())
                        {
                            string id = reader.GetInt32(0).ToString();
                            string status = reader.GetString(1);
                            ListItem listItem = new ListItem(status, id);
                            Status.Items.Add(listItem);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

            }
        }

        protected void GetCourseTypeDropDownList()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Выполните SQL-запрос для получения данных для выпадающего списка
                    string query = "SELECT id, type FROM public.type ORDER BY id ASC"; // Замените на ваш запрос

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        // Очистите существующие элементы в списке
                        Course_Type.Items.Clear();

                        // Добавьте пустой элемент (первый элемент списка)
                        Course_Type.Items.Add(new ListItem("", ""));

                        // Добавьте элементы в список на основе данных из базы данных
                        while (reader.Read())
                        {
                            string id = reader.GetInt32(0).ToString();
                            string type = reader.GetString(1);
                            ListItem listItem = new ListItem(type, id);
                            Course_Type.Items.Add(listItem);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

            }
        }


        protected void GetCourseNameDropDownList()
        {

            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Выполните SQL-запрос для получения данных для выпадающего списка
                    string query = "SELECT id, name FROM public.course ORDER BY id DESC"; // Замените на ваш запрос

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        // Очистите существующие элементы в списке
                        Course_Name.Items.Clear();

                        // Добавьте пустой элемент (первый элемент списка)
                        Course_Name.Items.Add(new ListItem("", ""));

                        // Добавьте элементы в список на основе данных из базы данных
                        while (reader.Read())
                        {
                            string id = reader.GetInt32(0).ToString();
                            string course = reader.GetString(1);
                            ListItem listItem = new ListItem(course, id);
                            Course_Name.Items.Add(listItem);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

            }

        }


        protected void AddNewCourse_Click(object sender, EventArgs e)
        {
            try
            {

                string newCourseName = newCourseNameInput.Value;

                // Строка подключения к базе данных PostgreSQL
                string connectionString = ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос для вставки данных в таблицу 'request'
                    string sql = @"INSERT INTO public.course (name) VALUES (@Name)";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", newCourseName);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (FormatException ex)
            {
                // Обработка ошибки парсинга
                Console.WriteLine("Неправильно введены значения в поля: " + ex.Message);
                // Здесь вы можете предпринять необходимые действия, например, вывести сообщение об ошибке пользователю или установить значение по умолчанию.
            }
            catch (Exception) { }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "$('#addCourseModal').modal('hide');", true);

        }


        protected void GetFioDropDownList()
        {
            string username = GetUserName();

            string department = GetDepartmentByUsername(username);

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString_MSSQL"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Выполните SQL-запрос для получения данных для выпадающего списка
                string query = "SELECT DISTINCT FIO, Dept, Position FROM custom_view_employees where Dept LIKE @Department ORDER BY FIO ASC"; // 

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Department", "%" + department + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
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
                    }
                }

                connection.Close();
            }
        }


        protected string GetDepartmentByUsername(string username)
        {
            string department = "";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString_MSSQL"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Выполните SQL-запрос для получения данных для выпадающего списка
                string query = "SELECT Dept FROM custom_view_employees where LoginName = @Username"; // Замените на ваш запрос
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    SqlDataReader reader = command.ExecuteReader();
                    // Добавьте элементы в список на основе данных из базы данных
                    if (reader.Read())
                    {
                        department = reader["Dept"].ToString();
                    }
                }



                connection.Close();
            }

            return department;
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

                    string query = "SELECT role_id FROM users WHERE lower(username) = lower(@Username)";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (NpgsqlDataReader reader =  command.ExecuteReader())
                        {
                            if ( reader.Read())
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
                int courseNameId = Convert.ToInt32(Course_Name.Value);
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
                    string sql = @"INSERT INTO public.request (full_name, department, position, course_name_id, course_type_id, notation, status_id, course_start, course_end, year, creator) 
                       VALUES (@FullName, @Department, @Position, @CourseNameId, @CourseType, @Notation, @Status, @CourseStart, @CourseEnd, @Year, @User)";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@Department", department);
                        command.Parameters.AddWithValue("@Position", position);
                        command.Parameters.AddWithValue("@CourseNameId", courseNameId);
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
                     connection.OpenAsync();

                    // SQL-запрос для извлечения данных из таблицы 'Request'
                    string sql = "SELECT r.id, full_name, department, position, c.name, t.type, notation, s.status, course_start, course_end, year, creator " +
                        "FROM public.request r " +
                        "JOIN public.type t ON t.id = r.course_type_id " +
                        "JOIN public.status s ON s.id = r.status_id " +
                        "JOIN public.course c ON c.id = r.course_name_id " +
                        "WHERE s.status = 'Новая' AND creator = @User " +
                        "ORDER BY id DESC";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        string userName = GetUserName();
                        command.Parameters.AddWithValue("@User", userName);

                        using (NpgsqlDataReader reader =  command.ExecuteReader())
                        {
                            while ( reader.Read())
                            {
                                Request request = new Request
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Full_Name = reader.GetString(reader.GetOrdinal("full_name")),
                                    Department = reader.GetString(reader.GetOrdinal("department")),
                                    Position = reader.GetString(reader.GetOrdinal("position")),
                                    Course_Name = reader.GetString(reader.GetOrdinal("name")),
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