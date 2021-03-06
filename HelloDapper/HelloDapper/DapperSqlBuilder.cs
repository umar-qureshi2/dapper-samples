﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDapper
{
    public class DapperSqlBuilder
    {
        public void SelectById()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                conn.Open();
                SqlBuilder builder = new SqlBuilder();
                var template = builder.AddTemplate("SELECT /**select**/ from Suppliers /**where**/");
                builder.Select("*");
                builder.Where("SupplierID = @SupplierID", new { SupplierID = 2 });
                var sql = template.RawSql;
                var supplier = conn.Query<Supplier>(sql, template.Parameters);
                ObjectDumper.Write(supplier);
            }
            Console.WriteLine("Please enter any key to exit");
            Console.ReadKey();
        }

        public void BuilderAndTVP()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                conn.Open();
                DataTable dt = new DataTable();
                dt.Columns.Add("CompanyName");
                dt.Columns.Add("City");
                dt.Rows.Add("New Washing Company", "Albuquerque");
                dt.Rows.Add("Walter White Inc.", "Albuquerque");
                conn.Execute("ImportBasicSuppliers", new { suppliers = dt.AsTableValuedParameter("SupplierBasic") }, commandType: CommandType.StoredProcedure);

                var builder = new SqlBuilder();
                var template = builder.AddTemplate("select /**select**/ from Suppliers /**where**/ /**orderby**/");
                builder.Select("TOP 2 CompanyName, City");
                builder.OrderBy("SupplierID DESC");
                var sql = template.RawSql;
                var suppliers = conn.Query<Supplier>(sql);
                foreach (var supplier in suppliers)
                {
                    ObjectDumper.Write(supplier);
                }

            }
            Console.WriteLine("Please enter any key to exit");
            Console.ReadKey();
        }
    }
}
