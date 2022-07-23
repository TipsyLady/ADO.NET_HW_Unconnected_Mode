using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace ADO.NET_HW_Unconnected_Mode
{
    class Program
    {
        public static void Employees (DataTable employee)
        {
            foreach (DataRow emRow in employee.Rows)
            {
                var EmployeeID = emRow["EmployeeID"];
                var EmployeeName = emRow["FirstName"] + " " + emRow["LastName"];
                var EmployeeBD = emRow["BirthDate"];
                var EmployeeP = emRow["PositionID"];
                var EmployeeS = emRow["Salary"];
                WriteLine($" {EmployeeID}  {EmployeeName}  {DateTime.Parse(EmployeeBD.ToString()).ToShortDateString()}  {EmployeeP}  {EmployeeS}");
            }
        }
        static void Main(string[] args)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Company_DB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlDataAdapter employeeAapter = new SqlDataAdapter("Select *from [dbo].[Employee]", conn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(employeeAapter);
                DataSet dataSet = new DataSet();
                employeeAapter.Fill(dataSet, "Employee");
                DataTable employees = dataSet.Tables["Employee"];
                //Select

                //Employees(employees);

                //Insert
                employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };
                employeeAapter.InsertCommand = new SqlCommand("stp_EmployeeAdd", conn);
                employeeAapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                employeeAapter.InsertCommand.Parameters.AddWithValue("@FirstName", "Maksim");
                employeeAapter.InsertCommand.Parameters.AddWithValue("@LastName", "Selivanov");
                employeeAapter.InsertCommand.Parameters.AddWithValue("@BirthDate", "1992-03-13");
                employeeAapter.InsertCommand.Parameters.AddWithValue("@PositionID", 2);
                employeeAapter.InsertCommand.Parameters.AddWithValue("@Salary", 4590);
                employeeAapter.InsertCommand.Parameters.Add("@EmployeeID", SqlDbType.Int).Direction = ParameterDirection.Output;

                //employeeAapter.InsertCommand.ExecuteNonQuery();
                employees.Clear();
                employeeAapter.Fill(dataSet, "Employee");

                //Employees(employees);

                //Delete
                employeeAapter.DeleteCommand = new SqlCommand("stp_EmployeeDelete_1", conn);
                employeeAapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
                employeeAapter.DeleteCommand.Parameters.AddWithValue("@EmployeeID", 17);
                //employeeAapter.DeleteCommand.ExecuteNonQuery();
                employees.Clear();
                employeeAapter.Fill(dataSet, "Employee");
                //Employees(employees);

                //Update
                employeeAapter.UpdateCommand = new SqlCommand("stp_EmployeeUpdate", conn);
                employeeAapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
                employeeAapter.UpdateCommand.Parameters.AddWithValue("@FirstName", "Maksim");
                employeeAapter.UpdateCommand.Parameters.AddWithValue("@LastName", "Fomin");
                employeeAapter.UpdateCommand.Parameters.AddWithValue("@BirthDate", "1997-05-12");
                employeeAapter.UpdateCommand.Parameters.AddWithValue("@PositionID", 2);
                employeeAapter.UpdateCommand.Parameters.AddWithValue("@Salary", 4590);
                employeeAapter.UpdateCommand.Parameters.AddWithValue("@EmployeeID", 15);
                employeeAapter.UpdateCommand.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;
                employeeAapter.UpdateCommand.ExecuteNonQuery();
                employees.Clear();
                employeeAapter.Fill(dataSet, "Employee");

                Employees(employees);
            }
        }
    }
}
