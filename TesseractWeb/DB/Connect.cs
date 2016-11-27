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
        public string UserConnect(string Name,string Password )
        {
            //Web.config файлаас өгөгдлийн сантай холбогдох String-ийг авч байна.
            string dsn = (string)ConfigurationManager.AppSettings["dsn"];
            //Авсан тэмдэгтийн цуваагаар өгөгдлийн сантай холбох обьектыг үүсгэж байна.
            //Класс System.Data.SqlClient-д байгаа
            IDbConnection conn = new SqlConnection(dsn);
            //SQL комманд гүйцэтгэх обьектыг дээрх холбоосоос үүсгэж байна.
            IDbCommand cmd = conn.CreateCommand();
            //Коммандыг зааж өгж байна
            cmd.CommandText = "SELECT * FROM Sailor where sailorId=@Name AND password=@Password ";
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
                    return "1";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                conn.Dispose();
            }
            return "2";
        }

        public string UserAdd(UserModel UM)
            {
                string dsn = (string)ConfigurationManager.AppSettings["dsn"];
                SqlConnection conn = new SqlConnection(dsn);
                //Коммандын төрлийг сонгох тохиолдолд дараах байдлаар үүсгэнэ.
                SqlCommand cmd = new SqlCommand("AddCompany", conn);
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
                return "Хэрэглэгч амжилттай нэмэгдлээ";
            }
            public string CompanyUpdate(UserModel UM)
            {
                string dsn = (string)ConfigurationManager.AppSettings["dsn"];
                SqlConnection conn = new SqlConnection(dsn);
                //Коммандын төрлийг сонгох тохиолдолд дараах байдлаар үүсгэнэ.
                SqlCommand cmd = new SqlCommand("UpdateCompany", conn);
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
            public string CompanyDelete(int id)
            {
                string dsn = (string)ConfigurationManager.AppSettings["dsn"];
                SqlConnection conn = new SqlConnection(dsn);
                //Коммандын төрлийг сонгох тохиолдолд дараах байдлаар үүсгэнэ.
                SqlCommand cmd = new SqlCommand("DeleteCompany", conn);
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

    }
}