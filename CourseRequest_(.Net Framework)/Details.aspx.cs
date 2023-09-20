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
                Status.Disabled = false;
            }

            if (Request.QueryString["requestId"] != null)
            {
                requestId = Convert.ToInt32(Request.QueryString["requestId"]);
            }

            if (!IsPostBack)
            {

                // Загрузите данные о заявке по requestId из базы данных
                RequestDB request = GetRequestById(requestId);

                //Response.Write(requestId);

                if (request != null)
                {
                    // Заполните контролы на странице значениями из объекта Request
                    Full_Name.Value = request.Full_Name;
                    Department.Value = request.Department;
                    Position.Value = request.Position;
                    Course_Name.Value = request.Course_Name;
                    Course_Type.Value = request.Course_Type_id.ToString();
                    Notation.Value = request.Notation;
                    Status.Value = request.Status_id.ToString();

                    Course_Start.Value = request.Course_Start.ToString("dd.MM.yyyy");
                    Course_End.Value = request.Course_End.ToString("dd.MM.yyyy");

                    Year = request.Year.ToString();
                    UserName = request.User;

                }

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
            // Получите значения полей из формы
            string full_Name = Full_Name.Value;
            string department = Department.Value;
            string position = Position.Value;
            string course_Name = Course_Name.Value;
            int course_Type_id = int.Parse(Course_Type.Value); // Предполагается, что Course_Type - это DropDownList
            string notation = Notation.Value;
            int status_id = int.Parse(Status.Value); // Предполагается, что Status - это DropDownList
            DateTime course_Start = DateTime.ParseExact(Course_Start.Value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime course_End = DateTime.ParseExact(Course_End.Value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            int year = course_Start.Year;
            // В этом месте также получите другие значения полей, если необходимо.

            // Обновите запись в базе данных, используя полученные значения
            UpdateRequestInDatabase(requestId, full_Name, department, position, course_Name, course_Type_id, notation, status_id, course_Start, course_End, year);

            // Опционально: перенаправьте пользователя на другую страницу после обновления заявки
            Response.Redirect("~/ListRequest.aspx");
        }


        private void UpdateRequestInDatabase(int requestId, string full_Name, string department, string position, string course_Name, int course_Type_id, string notation, int status_id, DateTime course_Start, DateTime course_End, int year)
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
                                 "SET full_name = @full_Name, department = @department, position = @position, course_name = @course_Name, course_type_id = @course_Type_id, " +
                                 "notation = @notation, status_id = @status_id, course_start = @course_Start, course_end = @course_End, year = @year " +
                                 "WHERE id = @requestId";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@full_Name", full_Name);
                        command.Parameters.AddWithValue("@department", department);
                        command.Parameters.AddWithValue("@position", position);
                        command.Parameters.AddWithValue("@course_Name", course_Name);
                        command.Parameters.AddWithValue("@course_Type_id", course_Type_id);
                        command.Parameters.AddWithValue("@notation", notation);
                        command.Parameters.AddWithValue("@status_id", status_id);
                        command.Parameters.AddWithValue("@course_Start", course_Start);
                        command.Parameters.AddWithValue("@course_End", course_End);
                        command.Parameters.AddWithValue("@year", year);
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
        private RequestDB GetRequestById(int requestId)
        {
            // Строка подключения к базе данных PostgreSQL
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CourseRequestConnectionString"].ConnectionString;

            RequestDB request = new RequestDB();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL-запрос для извлечения данных из таблицы 'Request'
                    string sql = "SELECT r.id, r.full_name, r.department, r.position, r.course_name, r.course_type_id, r.notation, r.status_id, r.course_start, r.course_end, r.year, creator FROM public.request r " +
                        "Where r.Id = @reqId";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@reqId", requestId);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                request = new RequestDB
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Full_Name = reader.GetString(reader.GetOrdinal("full_name")),
                                    Department = reader.GetString(reader.GetOrdinal("department")),
                                    Position = reader.GetString(reader.GetOrdinal("position")),
                                    Course_Name = reader.GetString(reader.GetOrdinal("course_name")),
                                    Course_Type_id = reader.GetInt32(reader.GetOrdinal("course_type_id")),
                                    Notation = reader.GetString(reader.GetOrdinal("notation")),
                                    Status_id = reader.GetInt32(reader.GetOrdinal("status_id")),
                                    Course_Start = reader.GetDateTime(reader.GetOrdinal("course_start")),
                                    Course_End = reader.GetDateTime(reader.GetOrdinal("course_end")),
                                    Year = reader.GetInt32(reader.GetOrdinal("year")),
                                    User = reader.GetString(reader.GetOrdinal("creator"))

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