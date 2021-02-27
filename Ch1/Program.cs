using System;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;

namespace Ch1
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadDataFromSQLServer();
            Console.WriteLine("Hello World!");
        }

        static void PrintMsg()
        {
            string msg=$"Dive into the C#!";
            Console.WriteLine(msg);
        }

        static void ReadDataFromSQLServer()
        {
            string connString=@"Server=WIN-6Q0BTM94V7Q\MSSQLSERVER01;Database=Adventureworks2019;Integrated Security=SSPI;";
            using(SqlConnection conn=new SqlConnection(connString))
            {
                conn.Open();
                string selectSQL="select * from INFORMATION_SCHEMA.TABLES;";
                var result=conn.Query(selectSQL);
                string jsonValue=JsonConvert.SerializeObject(result,Formatting.Indented);
                Console.WriteLine(jsonValue);
            }
        }
 
        static void AnnoymousSerializeObject()
        {
            var obj=new 
            {
                Id=1,
                Name="Fred",
                IsMale=true,
                NetWorth=99999999999999999999999.88
            };
            string jsonValue=JsonConvert.SerializeObject(obj,Formatting.Indented);
            Console.WriteLine(jsonValue);
        } 
    }
}
