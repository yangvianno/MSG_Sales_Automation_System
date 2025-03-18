using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using OfficeOpenXml;
using System.Data;
using System.Globalization;

namespace LeadLib
{
    public class LeadRepo
    {
        public static void SaveLeadFileToDataBase(string file, string connectionString = "")
        {
            if (connectionString == "")
            {
                connectionString = "Data source=10.120.2.40; Initial Catalog=FIN_REPORT; User Id=thanhIT;Password=P@ssw0rd!23";
            }
            if (File.Exists(file))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                var cultureInfo = new CultureInfo("fr - FR");
                try
                {
                    string sqlstr = "";
                    using (var pck = new OfficeOpenXml.ExcelPackage())
                    {
                        using (var stream = System.IO.File.OpenRead(file))
                        {
                            pck.Load(stream);
                        }
                        var ws = pck.Workbook.Worksheets.First();
                        int startRow = 3;
                        sqlstr = @"proc_LeadSave";
                        command.Parameters.Add("@STT", SqlDbType.Int);
                        command.Parameters.Add("@TenKhachHang", SqlDbType.NVarChar);
                        command.Parameters.Add("@SoPhu", SqlDbType.NVarChar);
                        command.Parameters.Add("@Phone", SqlDbType.NVarChar);
                        command.Parameters.Add("@Ngay", SqlDbType.DateTime);
                        command.Parameters.Add("@Nguon", SqlDbType.NVarChar);
                        command.Parameters.Add("@TinhThanh", SqlDbType.NVarChar);
                        command.Parameters.Add("@file", SqlDbType.NVarChar);

                        command.CommandText = sqlstr;
                        command.CommandType = CommandType.StoredProcedure;
                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            int stt=0;
                            string TenKhachHang="";
                            string SoPhu="";
                            string Phone="";
                            string Nguon="";
                            string TinhThanh="";
                            DateTime Ngay= DateTime.Now.Date;
                            try
                            {
                                
                                var wsRow = ws.Cells[rowNum, 1, rowNum, 7];
                                if (wsRow[rowNum, 1].Text != "")
                                {
                                    stt = int.Parse(wsRow[rowNum, 1].Text);
                                    TenKhachHang = wsRow[rowNum, 2].Text;
                                    SoPhu = wsRow[rowNum, 3].Text;
                                    Phone = wsRow[rowNum, 4].Text;                                   
                                    Nguon = wsRow[rowNum, 6].Text;
                                    TinhThanh = wsRow[rowNum, 7].Text;
                                    Ngay = (wsRow[rowNum, 5].Text == "" ? DateTime.Now.Date : DateTime.Parse(wsRow[rowNum, 5].Text, cultureInfo));
                                    if (TenKhachHang != "" && Phone != "")
                                    {

                                        //" + stt + @"
                                        //, N'" + TenKhachHang + @"'
                                        //, N'" + SoPhu + @"'
                                        //, N'" + Phone + @"'
                                        //, '" + Ngay.ToString("yyyy-MM-dd") + @"'
                                        //, N'" + Nguon + @"'
                                        //, N'" + TinhThanh + @"'
                                        //, N'" + file + "'";

                                        command.Parameters["@STT"].Value = stt;
                                        command.Parameters["@TenKhachHang"].Value = TenKhachHang;
                                        command.Parameters["@SoPhu"].Value = SoPhu;
                                        command.Parameters["@Phone"].Value = Phone;
                                        command.Parameters["@Ngay"].Value = Ngay;
                                        command.Parameters["@Nguon"].Value = Nguon;
                                        command.Parameters["@TinhThanh"].Value = TinhThanh;
                                        command.Parameters["@file"].Value = file;

                                        command.ExecuteNonQuery();

                                    }
                                }
                            } catch (Exception ex)
                            {
                                WriteLog(ex.Message + ";" + stt + ";" + TenKhachHang + ";" + SoPhu + ";" + TenKhachHang + ";" + Ngay + ";" + Nguon + ";" + TinhThanh + ";" + file);
                            }
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                command.Connection.Close();


            }

        }
        public static void WriteLog(string log)
        {
            try
            {

                if (!Directory.Exists("ErrorLogs"))
                {
                    Directory.CreateDirectory("ErrorLogs");
                }

                string path = "ErrorLogs\\" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".log";

                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(DateTime.Now + " : " + log);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(DateTime.Now + " : " + log);
                        sw.Close();
                    }
                }
            }
            catch
            {
            }
        }
    }
}
