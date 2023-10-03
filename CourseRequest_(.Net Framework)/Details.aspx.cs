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
using System.Globalization;
using System.Web.Services.Description;

namespace CourseRequest__.Net_Framework_
{
    public partial class _Details : Page
    {
        protected int requestId;
        protected string Year;
        protected string UserName;


        protected void Page_Load(object sender, EventArgs e)
        {
            int role = GetRoleByUsername();
            if (role == 2)
            {
                updateRequestButton.Visible = true;
                Status_det.Disabled = false;
            }

            if (Request.QueryString["requestId"] != null)
            {
                requestId = Convert.ToInt32(Request.QueryString["requestId"]);
            }

            if (!IsPostBack)
            {

                // Загрузите данные о заявке по requestId из базы данных
                Request request = GetRequestById(requestId);

                //Response.Write(requestId);

                if (request != null)
                {
                    // Заполните контролы на странице значениями из объекта Request
                    Full_Name_det.Value = request.Full_Name;
                    Department_det.Value = request.Department;
                    Position_det.Value = request.Position;
                    Course_Name_det.Value = request.Course_Name;
                    Course_Type_det.Value = request.Course_Type;
                    Notation_det.Value = request.Notation;
                    Status_det.Value = request.Status;

                    Course_Start_det.Value = request.Course_Start.ToString("dd.MM.yyyy");
                    Course_End_det.Value = request.Course_End.ToString("dd.MM.yyyy");

                    Year = request.Year.ToString();
                    UserName = request.User;

                }

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
                        Status_det.Items.Clear();

                        // Добавьте элементы в список на основе данных из базы данных
                        while (reader.Read())
                        {
                            string id = reader.GetInt32(0).ToString();
                            string status = reader.GetString(1);
                            ListItem listItem = new ListItem(status, id);
                            Status_det.Items.Add(listItem);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

            }
        }

        protected string GetUserName()
        {
            string userName = Page.User.Identity.Name;
            return userName;
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

        protected void UpdateRequestButton_Click(object sender, EventArgs e)
        {
            
            int status_id = int.Parse(Status_det.Value); // Предполагается, что Status - это DropDownList
            
            // В этом месте также получите другие значения полей, если необходимо.

            // Обновите запись в базе данных, используя полученные значения
            UpdateRequestInDatabase(requestId, status_id);

            // Опционально: перенаправьте пользователя на другую страницу после обновления заявки
            Response.Redirect("~/ListRequest.aspx");
        }


        private void UpdateRequestInDatabase(int requestId, int status_id)
        {
            // Строка подключения к базе данных PostgreSQL
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL-запрос для обновления данных в таблице 'Request'
                    string sql = "UPDATE public.request " +
                                 "SET status_id = @status_id " +
                                 "WHERE id = @requestId";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@status_id", status_id);
                        command.Parameters.AddWithValue("@requestId", requestId);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок, логирование и т. д.
                    Console.WriteLine("Произошла ошибка при обновлении заявки: " + ex.Message);
                }
            }
        }



        // Метод для загрузки данных о заявке по requestId из базы данных
        private Request GetRequestById(int requestId)
        {
            // Строка подключения к базе данных PostgreSQL
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

            Request request = new Request();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL-запрос для извлечения данных из таблицы 'Request'
                    
                    string sql = "SELECT r.id, full_name, department, position, c.name, t.type, notation, s.status, course_start, course_end, year, creator " +
                        "FROM public.request r " +
                        "JOIN public.type t ON t.id = r.course_type_id " +
                        "JOIN public.status s ON s.id = r.status_id " +
                        "JOIN public.course c ON c.id = r.course_name_id " +
                        "WHERE r.Id = @reqId  ";


                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@reqId", requestId);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                request = new Request
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
                                    Course_End =reader.GetDateTime(9),
                                    Year = reader.GetInt32(10),
                                    User = reader.GetString(11)

                                };



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

            return request;
        }





    }
}