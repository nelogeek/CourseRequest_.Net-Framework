﻿using CourseRequest__.Net_Framework_.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseRequest__.Net_Framework_
{
    public partial class _ListRequest : Page
    {
        protected int requestCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            int role = GetRoleByUsername();
            if (role != 1)
            {
                NewRequestBtn.Visible = false;
            }
           
            if (!IsPostBack)
            {
           
                // получение и вывод заявок в таблицу
                List<Request> requests = GetFilteredRequests();
                RepeaterRequests.DataSource = requests;
                RepeaterRequests.DataBind();
           
                // получение количества заявок
                requestCount = requests.Count;
           
                // Добавление текущего года и трех предыдущих значений в <select>
                int currentYear = DateTime.Now.Year;
                yearSelect.Items.Add(new ListItem(currentYear.ToString(), currentYear.ToString()));
                for (int i = 1; i <= 3; i++)
                {
                    int previousYear = currentYear - i;
                    yearSelect.Items.Add(new ListItem(previousYear.ToString(), previousYear.ToString()));
                }

                // получение ФИО сотрудников из заявок
                GetFioDropDownList();
                
                // получение отделов из заявок
                GetDepDropDownList();

                // получение всех годов
                GetYearDropDownList();

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
                        Stat.Items.Clear();

                        // Добавьте пустой элемент (первый элемент списка)
                        Stat.Items.Add(new ListItem("", ""));

                        // Добавьте элементы в список на основе данных из базы данных
                        while (reader.Read())
                        {
                            string id = reader.GetInt32(0).ToString();
                            string status = reader.GetString(1);
                            ListItem listItem = new ListItem(status, id);
                            Stat.Items.Add(listItem);
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
                        Course_Type_list.Items.Clear();

                        // Добавьте пустой элемент (первый элемент списка)
                        Course_Type_list.Items.Add(new ListItem("", ""));

                        // Добавьте элементы в список на основе данных из базы данных
                        while (reader.Read())
                        {
                            string id = reader.GetInt32(0).ToString();
                            string type = reader.GetString(1);
                            ListItem listItem = new ListItem(type, id);
                            Course_Type_list.Items.Add(listItem);
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
                        Course_Name_list.Items.Clear();

                        // Добавьте пустой элемент (первый элемент списка)
                        Course_Name_list.Items.Add(new ListItem("", ""));

                        // Добавьте элементы в список на основе данных из базы данных
                        while (reader.Read())
                        {
                            string id = reader.GetInt32(0).ToString();
                            string course = reader.GetString(1);
                            ListItem listItem = new ListItem(course, id);
                            Course_Name_list.Items.Add(listItem);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

            }

        }


        protected void GetYearDropDownList()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Выполните SQL-запрос для получения данных для выпадающего списка
                    string query = "SELECT year FROM (SELECT EXTRACT(YEAR FROM course_start) AS year FROM public.request UNION SELECT EXTRACT(YEAR FROM course_end) AS year FROM public.request) AS years ORDER BY year"; // Замените на ваш запрос

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        // Очистите существующие элементы в списке
                        yearSelect.Items.Clear();

                        // Добавьте пустой элемент (первый элемент списка)
                        yearSelect.Items.Add(new ListItem("Весь период", ""));

                        // Добавьте элементы в список на основе данных из базы данных
                        while (reader.Read())
                        {
                            string year = reader.GetInt32(0).ToString();
                            ListItem listItem = new ListItem(year, year);
                            yearSelect.Items.Add(listItem);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

            }
        }



        protected void GetDepDropDownList()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Выполните SQL-запрос для получения данных для выпадающего списка
                    string query = "SELECT DISTINCT department FROM public.request ORDER BY department ASC"; // Замените на ваш запрос

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        // Очистите существующие элементы в списке
                        Department.Items.Clear();

                        // Добавьте пустой элемент (первый элемент списка)
                        Department.Items.Add(new ListItem("", ""));

                        // Добавьте элементы в список на основе данных из базы данных
                        while (reader.Read())
                        {
                            string dep = reader.GetString(0);
                            ListItem listItem = new ListItem(dep, dep);
                            Department.Items.Add(listItem);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

            }
        }


        protected void GetFioDropDownList()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Выполните SQL-запрос для получения данных для выпадающего списка
                    string query = "SELECT DISTINCT full_name FROM public.request ORDER BY full_name ASC"; // Замените на ваш запрос

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        // Очистите существующие элементы в списке
                        Full_Name.Items.Clear();

                        // Добавьте пустой элемент (первый элемент списка)
                        Full_Name.Items.Add(new ListItem("", ""));

                        // Добавьте элементы в список на основе данных из базы данных
                        while (reader.Read())
                        {
                            string fio = reader.GetString(0);
                            ListItem listItem = new ListItem(fio, fio);
                            Full_Name.Items.Add(listItem);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception )
            {
                
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

        protected void RepeaterRequests_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ShowDetails")
            {
                int requestId = Convert.ToInt32(e.CommandArgument);
                // Здесь вы можете получить данные о выбранной заявке по requestId

                //Response.Write(requestId);
                // Перенаправление на страницу Detail.aspx с передачей параметров
                Response.Redirect($"~/Details.aspx?requestId={requestId}");
            }
        }




        protected void FilterRequestButton_Click(object sender, EventArgs e)
        {
            string year = yearSelect.Value;
            string stat = Stat.Value.ToString();
            string dep = Department.Value.ToString();
            string courseBegin = CourseBegin_list.Value.ToString();
            string courseEnd = CourseEnd_list.Value.ToString();
            string fullName = Full_Name.Value.ToString();
            string courseType = Course_Type_list.Value.ToString();
            string courseName = Course_Name_list.Value.ToString();
            string onlyMyReq = OnlyMyReq.Value.ToString();

            List<Request> filteredRequests = GetFilteredRequests(year, stat, dep, courseBegin, courseEnd, fullName, courseType, courseName, onlyMyReq);

            requestCount = filteredRequests.Count;

            //Response.Write($"{year}, {stat}, {dep}, {courseBegin}, {courseEnd}, {fullName}, {courseType}, {courseName}, {onlyMyReq}");

            //Response.Write(OnlyMyReq.Checked);

            // Обновляем RepeaterRequests данными
            RepeaterRequests.DataSource = filteredRequests;
            RepeaterRequests.DataBind();

            // Обновляем UpdatePanel после обновления данных
            UpdatePanel1.Update();
        }


        protected List<Request> GetFilteredRequests(string year = "", string stat = "", string dep = "", string courseBegin = "", string courseEnd = "", string fullName = "", string courseType = "", string courseName = "", string onlyMyReq = "")
        {
            List<Request> requests = new List<Request>();

            // Строка подключения к базе данных PostgreSQL
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;
            try
            {
                string username = GetUserName();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT r.id, r.full_name, r.department, r.position, c.name, t.type, r.notation, s.status, r.course_start, r.course_end, r.year, r.creator " +
                            "FROM public.request r " +
                            "JOIN public.type t ON t.id = r.course_type_id " +
                            "JOIN public.status s ON s.id = r.status_id " +
                            "JOIN public.course c ON c.id = r.course_name_id " +
                            "WHERE 1=1";


                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        // условия фильтрации
                        if (!string.IsNullOrEmpty(year))
                        {
                            query += " AND (EXTRACT(YEAR FROM course_start) = @Year OR EXTRACT(YEAR FROM course_end) = @Year)";
                            command.Parameters.AddWithValue("@Year", Convert.ToInt32(year));
                        }
                        if (!string.IsNullOrEmpty(stat))
                        {
                            query += " AND r.status_id = @Stat";
                            command.Parameters.AddWithValue("@Stat", Convert.ToInt32(stat));
                        }
                        if (!string.IsNullOrEmpty(dep))
                        {
                            query += " AND department ILIKE @Dep";
                            command.Parameters.AddWithValue("@Dep", "%" + dep + "%");
                        }
                        if (!string.IsNullOrEmpty(courseBegin) && DateTime.TryParse(courseBegin, out DateTime startDate))
                        {
                            query += " AND course_start >= @StartDate";
                            command.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(startDate.ToString("yyyy-MM-dd")));
                        }
                        if (!string.IsNullOrEmpty(courseEnd) && DateTime.TryParse(courseEnd, out DateTime endDate))
                        {
                            query += " AND course_end <= @EndDate";
                            command.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(endDate.ToString("yyyy-MM-dd")));
                        }
                        if (!string.IsNullOrEmpty(fullName))
                        {
                            query += " AND full_name ILIKE @FullName";
                            command.Parameters.AddWithValue("@FullName", "%" + fullName + "%");
                        }
                        if (!string.IsNullOrEmpty(courseType))
                        {
                            query += " AND r.course_type_id = @CourseType";
                            command.Parameters.AddWithValue("@CourseType", Convert.ToInt32(courseType));
                        }
                        if (!string.IsNullOrEmpty(courseName))
                        {
                            query += " AND r.course_name_id = @CourseNameId";
                            command.Parameters.AddWithValue("@CourseNameId", Convert.ToInt32(courseName));
                        }
                        if (!string.IsNullOrEmpty(onlyMyReq))
                        {
                            query += " AND creator = @Username";
                            command.Parameters.AddWithValue("@Username", username);
                        }

                        query += " ORDER BY id DESC";

                        //Response.Write(query);

                        command.CommandText = query;

                        NpgsqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Request request = new Request
                            {
                                Id = reader.GetInt32(0),
                                Full_Name = reader.GetString(1),
                                Department = reader.GetString(2),
                                Position = reader.GetString(3),
                                Course_Name = reader.GetString(4),
                                Course_Type = reader.GetString(5),
                                Notation = reader.GetString(6),
                                Status = reader.GetString(7),
                                Course_Start = reader.GetDateTime(8),
                                Course_End = reader.GetDateTime(9),
                                Year = reader.GetInt32(10),
                                User = reader.GetString(11)
                            };

                            
                            requests.Add(request);
                        }
                        //Response.Write($"{requests[0].Id}, {requests[0].Full_Name}, {requests[0].Department}, {requests[0].Position}, {requests[0].Course_Name}, {requests[0].Course_Type}, {requests[0].Notation}, {requests[0].Status}, {requests[0].Course_Start}, {requests[0].Course_End}, {requests[0].Year}, {requests[0].User}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, логирование и т. д.
                Console.WriteLine("Произошла ошибка при получении фильтрованных заявок: " + ex.Message);
            }

            return requests;
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
                        "ORDER BY id DESC";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
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

                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            int count = Convert.ToInt32(result);
                            return count > 0;
                        }
                        else
                        {
                            return true;
                        }
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
            string userName = Page.User.Identity.Name;
            return userName;
        }








    }
}