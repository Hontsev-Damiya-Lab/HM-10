using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace HM_10.ServiceMods
{
    public class DB
    {
        private string connStr = "";

        public DB()
        {
            //自文件中读取连接Access的字符串。
            FileStream fs;
            StreamReader sr;
            try
            {
                fs = new FileStream("D:\\ConnectionString.txt", FileMode.Open);
                sr = new StreamReader(fs);
                connStr = sr.ReadToEnd().Trim();
                sr.Close();
                fs.Close();
            }
            catch
            {
            }
        }
        /// <summary>
        /// 查询access数据库，会返回datatable形式查询结果
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataTable getResult(string sql)
        {
            OleDbConnection conn = new OleDbConnection(connStr);
            DataTable dt = new DataTable();
            //try
            //{
            conn.Open();
            OleDbDataAdapter myadapter = new OleDbDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            myadapter.Fill(ds);
            if (ds.Tables.Count > 0) dt = ds.Tables[0];
            myadapter.Dispose();
            conn.Close();
            conn.Dispose();
            //}
            //catch
            //{
            //    conn.Close();
            //    conn.Dispose();
            //}
            return dt;
        }

        /// <summary>
        /// 查询access数据库，会返回datatable形式查询结果
        /// </summary>
        /// <param name="comm"></param>
        /// <returns></returns>
        public DataTable getResult(OleDbCommand comm)
        {
            OleDbConnection conn = new OleDbConnection(connStr);
            DataTable dt = new DataTable();
            //try
            //{
            conn.Open();
            comm.Connection = conn;
            OleDbDataAdapter myadapter = new OleDbDataAdapter(comm);
            DataSet ds = new DataSet();
            myadapter.Fill(ds);
            if (ds.Tables.Count > 0) dt = ds.Tables[0];
            myadapter.Dispose();
            conn.Close();
            conn.Dispose();
            //}
            //catch
            //{
            //    conn.Close();
            //    conn.Dispose();
            //}
            return dt;
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        public void excute(string sql)
        {
            OleDbConnection conn = new OleDbConnection(connStr);
            //try
            //{
            conn.Open();
            OleDbCommand comm = new OleDbCommand(sql, conn);
            comm.ExecuteNonQuery();
            comm.Dispose();
            conn.Close();
            conn.Dispose();
            //}
            //catch
            //{
            //    conn.Close();
            //    conn.Dispose();
            //}
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="comm"></param>
        public void excute(OleDbCommand comm)
        {
            OleDbConnection conn = new OleDbConnection(connStr);
            //try
            //{
            conn.Open();
            comm.Connection = conn;
            comm.ExecuteNonQuery();
            comm.Dispose();
            conn.Close();
            conn.Dispose();
            //}
            //catch
            //{
            //    conn.Close();
            //    conn.Dispose();
            //}
        }
    }
}