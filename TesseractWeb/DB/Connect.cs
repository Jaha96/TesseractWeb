using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TesseractWeb.Models;

namespace TesseractWeb.DB
{
    public class Connect
    {
        string dsn = (string)ConfigurationManager.AppSettings["dsn"];
        public string UserConnect(string Name,string Password )
        {
            //Web.config файлаас өгөгдлийн сантай холбогдох String-ийг авч байна.
            
            //Авсан тэмдэгтийн цуваагаар өгөгдлийн сантай холбох обьектыг үүсгэж байна.
            //Класс System.Data.SqlClient-д байгаа
            IDbConnection conn = new SqlConnection(dsn);
            //SQL комманд гүйцэтгэх обьектыг дээрх холбоосоос үүсгэж байна.
            IDbCommand cmd = conn.CreateCommand();
            //Коммандыг зааж өгж байна
            cmd.CommandText = "SELECT * FROM UserTable where email=@Name AND password=@Password ";
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@Password", Password));
            //Com.Parameters.Add(new SqlParameter("@ProductID", ProductID));
            try
            {
                //Өгөгдлийн санг нээж байна
                conn.Open();
                //Коммандыг ажиллуулж байна
                IDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    
                    reader.Close();
                    return fillUserData(Name, Password).ToString();
                }
                
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                conn.Dispose();
            }
            return "Error";
        }
        public int fillUserData(string Name, string Password)
        {
            DataTable ds = new DataTable();
            SqlConnection Con = new SqlConnection(dsn);
                string SQL = @"SELECT * FROM UserTable where email=@Name AND password=@Password";
                Con.Open();
                SqlCommand Com = new SqlCommand(SQL, Con);
                
                    Com.Parameters.Add(new SqlParameter("@Name", Name));
                    Com.Parameters.Add(new SqlParameter("@Password", Password));
            SqlDataAdapter adap = new SqlDataAdapter(Com);
                    
                        adap.Fill(ds);
                    
                
                
            ds.Rows[0].SetField("LoginCount", Convert.ToInt32(ds.Rows[0].ItemArray[4]) + 1);
            UserModel UM = new UserModel();
            UM.UserId = Convert.ToInt16(ds.Rows[0].ItemArray[0]);
            UM.UserName = ds.Rows[0].ItemArray[1].ToString();
            UM.Count = Convert.ToInt32(ds.Rows[0].ItemArray[4]);
            UM.Email = Name;
            SQL = "Update UserTable Set LoginCount=@count where UserId=@id";
            Com = new SqlCommand(SQL, Con);
            
            Com.Parameters.Add(new SqlParameter("@count", UM.Count));
            Com.Parameters.Add(new SqlParameter("@id", UM.UserId));
            Com.ExecuteNonQuery();
            Con.Dispose();
            return UM.UserId;
        }

        public string UserAdd(UserModel UM)
            {
                string dsn = (string)ConfigurationManager.AppSettings["dsn"];
                SqlConnection conn = new SqlConnection(dsn);
                //Коммандын төрлийг сонгох тохиолдолд дараах байдлаар үүсгэнэ.
                SqlCommand cmd = new SqlCommand("AddCustomer", conn);
                //Коммандын төрөл профедур болохыг зааж байна
                cmd.CommandType = CommandType.StoredProcedure;
                //Уг процедурыг дуудахад дараах параметрүүдийг дамжуулна
                cmd.Parameters.AddWithValue("@UserName ", UM.UserName);
                cmd.Parameters.AddWithValue("@Password ", UM.Password);
                cmd.Parameters.AddWithValue("@Email ", UM.Email);
                //Процедурын гаралтын параметрийг дараахь байдлаар үүсгэн холбож өгнө
                SqlParameter idParam = new SqlParameter("@UserId ", SqlDbType.Int);
                idParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(idParam);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    return ex.ToString();
                }
                finally
                {
                    conn.Dispose();
                }
                return "1";
            }
            public string UserUpdate(UserModel UM)
            {
                string dsn = (string)ConfigurationManager.AppSettings["dsn"];
                SqlConnection conn = new SqlConnection(dsn);
                //Коммандын төрлийг сонгох тохиолдолд дараах байдлаар үүсгэнэ.
                SqlCommand cmd = new SqlCommand("UpdateCustomer", conn);
                //Коммандын төрөл профедур болохыг зааж байна
                cmd.CommandType = CommandType.StoredProcedure;
                //Уг процедурыг дуудахад дараах параметрүүдийг дамжуулна
                cmd.Parameters.AddWithValue("@UserId ", UM.UserId);
                cmd.Parameters.AddWithValue("@UserName ", UM.UserName);
                cmd.Parameters.AddWithValue("@Password ", UM.Password);
                cmd.Parameters.AddWithValue("@Email ", UM.Email);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    return ex.ToString();
                }
                finally
                {
                    conn.Dispose();
                }
                return "1";
            }
            public string UserDelete(int id)
            {
                string dsn = (string)ConfigurationManager.AppSettings["dsn"];
                SqlConnection conn = new SqlConnection(dsn);
                //Коммандын төрлийг сонгох тохиолдолд дараах байдлаар үүсгэнэ.
                SqlCommand cmd = new SqlCommand("DeleteCustomer", conn);
                //Коммандын төрөл профедур болохыг зааж байна
                cmd.CommandType = CommandType.StoredProcedure;
                //Уг процедурыг дуудахад дараах параметрүүдийг дамжуулна
                cmd.Parameters.AddWithValue("@UserId ", id);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    return ex.ToString();
                }
                finally
                {
                    conn.Dispose();
                }
                return "Хэрэглэгч амжилттай устгагдлаа";
            }

        public string FileHistoryAdd(FileModel FM)
        {
            string dsn = (string)ConfigurationManager.AppSettings["dsn"];
            SqlConnection conn = new SqlConnection(dsn);
            //Коммандын төрлийг сонгох тохиолдолд дараах байдлаар үүсгэнэ.
            SqlCommand cmd = new SqlCommand("AddFileHistory", conn);
            //Коммандын төрөл профедур болохыг зааж байна
            cmd.CommandType = CommandType.StoredProcedure;
            //Уг процедурыг дуудахад дараах параметрүүдийг дамжуулна
            cmd.Parameters.AddWithValue("@OutputFile  ", FM.outputFile);
            cmd.Parameters.AddWithValue("@InputImage  ", FM.inputFile);
            cmd.Parameters.AddWithValue("@Date  ", FM.date);
            cmd.Parameters.AddWithValue("@UserId  ", FM.userId);
            //Процедурын гаралтын параметрийг дараахь байдлаар үүсгэн холбож өгнө
            SqlParameter idParam = new SqlParameter("@HistoryId  ", SqlDbType.Int);
            idParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(idParam);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                return ex.ToString();
            }
            finally
            {
                conn.Dispose();
            }
            return "1";
        }
        public string FileHistoryDelete(int id)
        {
            string dsn = (string)ConfigurationManager.AppSettings["dsn"];
            SqlConnection conn = new SqlConnection(dsn);
            //Коммандын төрлийг сонгох тохиолдолд дараах байдлаар үүсгэнэ.
            SqlCommand cmd = new SqlCommand("DeleteFileHistory", conn);
            //Коммандын төрөл профедур болохыг зааж байна
            cmd.CommandType = CommandType.StoredProcedure;
            //Уг процедурыг дуудахад дараах параметрүүдийг дамжуулна
            cmd.Parameters.AddWithValue("@HistoryId  ", id);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                return ex.ToString();
            }
            finally
            {
                conn.Dispose();
            }
            return "1";
        }
        public string FileHistorySelect(int id)
        {
            string dsn = (string)ConfigurationManager.AppSettings["dsn"];
            SqlConnection conn = new SqlConnection(dsn);
            //Коммандын төрлийг сонгох тохиолдолд дараах байдлаар үүсгэнэ.
            SqlCommand cmd = new SqlCommand("SelectFileHistory", conn);
            //Коммандын төрөл профедур болохыг зааж байна
            cmd.CommandType = CommandType.StoredProcedure;
            //Уг процедурыг дуудахад дараах параметрүүдийг дамжуулна
            cmd.Parameters.AddWithValue("@UserId  ", id);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                return ex.ToString();
            }
            finally
            {
                conn.Dispose();
            }
            return "1";
        }

        public DataTable CompanyDatatable(int userId)
        {
            DataTable ds = new DataTable();
            using (SqlConnection Con = new SqlConnection(ConfigurationManager.AppSettings["dsn"]))
            {
                //string SQL = "select * from products where id = @ProductID";
                string SQL = @"select * from FileHistory f
                        left join UserTable u on f.UserId=u.UserId
                        where f.Userid=@id";
                Con.Open();
                using (SqlCommand Com = new SqlCommand(SQL, Con))
                {
                    Com.Parameters.Add(new SqlParameter("@id", userId));
                    using (SqlDataAdapter adap = new SqlDataAdapter(Com))
                    {
                        adap.Fill(ds);
                    }
                }
                Con.Dispose();
            }
            return ds;
        }
        public int getUserIdByEmail(string email)
        {
            DataTable dt = new DataTable();
            using (SqlConnection Con = new SqlConnection(ConfigurationManager.AppSettings["dsn"]))
            {
                //string SQL = "select * from products where id = @ProductID";
                string SQL = @"Select UserId from UserTable where email=@email";
                Con.Open();
                using (SqlCommand Com = new SqlCommand(SQL, Con))
                {
                    Com.Parameters.Add(new SqlParameter("@email", email));
                    using (SqlDataAdapter adap = new SqlDataAdapter(Com))
                    {
                        adap.Fill(dt);
                    }
                }
                Con.Dispose();
            }

            return Convert.ToInt32(dt.Rows[0]["UserId"]);
        }

    }
}