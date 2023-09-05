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

namespace CourseRequest__.Net_Framework_
{
    public partial class _Request : Page
    {
        protected int requestCount; // Объявление переменной на уровне класса

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                requestCount = GetRequestCount();

                List<Request> requests = GetRequestsFromDatabase();
                RepeaterRequests.DataSource = requests;
                RepeaterRequests.DataBind();

            }
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
                    string sql = "SELECT r.id, full_name, department, position, course_name, t.type, notation, " +
                                 "s.status, course_start, course_end, year, r.user " +
                                 "FROM public.request r " +
                                 "JOIN public.type t ON t.id = r.course_type " +
                                 "JOIN public.status s ON s.id = r.status " +
                                 "WHERE s.status = 'Новая' AND r.user = @User";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        string userName = $"{Dns.GetHostName()}\\{Environment.UserName}";
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
                                    User = reader.GetString(reader.GetOrdinal("user"))
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
                    string sql = "SELECT COUNT(*) " +
                        "FROM public.request r" +
                        "JOIN public.status s" +
                        "WHERE s.status = 'Новая' AND r.user = @User";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        string userName = $"{Dns.GetHostName()}\\{Environment.UserName}";
                        command.Parameters.AddWithValue("@User", userName);

                        int count = (int)command.ExecuteScalar();

                        return count;
                    }
                    
                }
                catch (Exception ex)
                {
                    // Обработка ошибок, логирование
                    Console.WriteLine("Произошла ошибка при получении количества заявок: " + ex.Message);
                    return 0; // Возвращаем 0 в случае ошибки
                }
            }
        }








    }
}