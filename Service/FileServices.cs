using ConsultasTSC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasTSC.Services
{
    public class FileServices
    {
 
        public static UploadServiceResponse SaveFileData(string constring, int FileId, string FileName, int FileSize, string Flexfields)
        {
            UploadServiceResponse response = new UploadServiceResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SP_SaveFileData ", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pnFileId", SqlDbType.Int).Value = FileId;
                    cmd.Parameters.Add("@pvFileName", SqlDbType.VarChar).Value = FileName;
                    cmd.Parameters.Add("@pnFileSizeBytes", SqlDbType.Int).Value = FileSize;
                    cmd.Parameters.Add("@pvFlexfields", SqlDbType.VarChar).Value =  Flexfields ?? (object)DBNull.Value;

                    SqlParameter pcMessageParam = new SqlParameter();
                    pcMessageParam.ParameterName = "@pcMessage";
                    pcMessageParam.SqlDbType = SqlDbType.VarChar;
                    pcMessageParam.Size = int.MaxValue;
                    pcMessageParam.Direction = ParameterDirection.Output;

                    SqlParameter pnCodeResultParam = new SqlParameter();
                    pnCodeResultParam.ParameterName = "@pnCodeResult";
                    pnCodeResultParam.SqlDbType = SqlDbType.Int;
                    pnCodeResultParam.Size = int.MaxValue;
                    pnCodeResultParam.Direction = ParameterDirection.Output;

                    SqlParameter pnTypeResultParam = new SqlParameter();
                    pnTypeResultParam.ParameterName = "@pnTypeResult";
                    pnTypeResultParam.SqlDbType = SqlDbType.Int;
                    pnTypeResultParam.Size = int.MaxValue;
                    pnTypeResultParam.Direction = ParameterDirection.Output;

                    SqlParameter pnReferenceIdParam = new SqlParameter();
                    pnReferenceIdParam.ParameterName = "@pnReferenceId";
                    pnReferenceIdParam.SqlDbType = SqlDbType.Int;
                    pnReferenceIdParam.Size = int.MaxValue;
                    pnReferenceIdParam.Direction = ParameterDirection.Output;

                    SqlParameter pcStorageDirectoryParam = new SqlParameter();
                    pcStorageDirectoryParam.ParameterName = "@pcStorageDirectory";
                    pcStorageDirectoryParam.SqlDbType = SqlDbType.VarChar;
                    pcStorageDirectoryParam.Size = int.MaxValue;
                    pcStorageDirectoryParam.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(pcMessageParam);
                    cmd.Parameters.Add(pnTypeResultParam);
                    cmd.Parameters.Add(pnCodeResultParam);
                    cmd.Parameters.Add(pnReferenceIdParam);
                    cmd.Parameters.Add(pcStorageDirectoryParam);

                    SqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();

                    String pcMessage = (cmd.Parameters["@pcMessage"].Value.ToString() == "null") ? null : cmd.Parameters["@pcMessage"].Value.ToString();
                    String pnTypeResult = (cmd.Parameters["@pnTypeResult"].Value.ToString() == "null") ? null : cmd.Parameters["@pnTypeResult"].Value.ToString();
                    String pnCodeResult = (cmd.Parameters["@pnCodeResult"].Value.ToString() == "null") ? null : cmd.Parameters["@pnCodeResult"].Value.ToString();
                    String pnReferenceId = (cmd.Parameters["@pnReferenceId"].Value.ToString() == "null") ? null : cmd.Parameters["@pnReferenceId"].Value.ToString();
                    String pcStorageDirectory = (cmd.Parameters["@pcStorageDirectory"].Value.ToString() == "null") ? null : cmd.Parameters["@pcStorageDirectory"].Value.ToString();

                    if(pnTypeResult != "0")
                    {
                        response.error = pcMessage;
                    } else
                    {
                        response.ReferenceId = Int32.Parse(pnReferenceId);
                        response.directory = pcStorageDirectory;
                    }

                    con.Close();
                }



                return response;
            } catch(Exception ex)
            {
                response.error = ex.ToString();
                return response;
            }
        }

        public static GetFileDataResponse GetFileData(string constring, int uploadId)
        {
            GetFileDataResponse response = new GetFileDataResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SP_GetFileData", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pnUploadId", SqlDbType.Int).Value = uploadId;

                    SqlParameter pcMessageParam = new SqlParameter();
                    pcMessageParam.ParameterName = "@pcMessage";
                    pcMessageParam.SqlDbType = SqlDbType.VarChar;
                    pcMessageParam.Size = int.MaxValue;
                    pcMessageParam.Direction = ParameterDirection.Output;

                    SqlParameter pnCodeResultParam = new SqlParameter();
                    pnCodeResultParam.ParameterName = "@pnCodeResult";
                    pnCodeResultParam.SqlDbType = SqlDbType.Int;
                    pnCodeResultParam.Size = int.MaxValue;
                    pnCodeResultParam.Direction = ParameterDirection.Output;

                    SqlParameter pnTypeResultParam = new SqlParameter();
                    pnTypeResultParam.ParameterName = "@pnTypeResult";
                    pnTypeResultParam.SqlDbType = SqlDbType.Int;
                    pnTypeResultParam.Size = int.MaxValue;
                    pnTypeResultParam.Direction = ParameterDirection.Output;

                    SqlParameter pcPathParam = new SqlParameter();
                    pcPathParam.ParameterName = "@pcPath";
                    pcPathParam.SqlDbType = SqlDbType.VarChar ;
                    pcPathParam.Size = int.MaxValue;
                    pcPathParam.Direction = ParameterDirection.Output;

                    SqlParameter pcDataParam = new SqlParameter();
                    pcDataParam.ParameterName = "@pcData";
                    pcDataParam.SqlDbType = SqlDbType.VarChar;
                    pcDataParam.Size = int.MaxValue;
                    pcDataParam.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(pcMessageParam);
                    cmd.Parameters.Add(pnTypeResultParam);
                    cmd.Parameters.Add(pnCodeResultParam);
                    cmd.Parameters.Add(pcPathParam);
                    cmd.Parameters.Add(pcDataParam);

                    SqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();

                    String pcMessage = (cmd.Parameters["@pcMessage"].Value.ToString() == "null") ? null : cmd.Parameters["@pcMessage"].Value.ToString();
                    String pnTypeResult = (cmd.Parameters["@pnTypeResult"].Value.ToString() == "null") ? null : cmd.Parameters["@pnTypeResult"].Value.ToString();
                    String pnCodeResult = (cmd.Parameters["@pnCodeResult"].Value.ToString() == "null") ? null : cmd.Parameters["@pnCodeResult"].Value.ToString();
                    String pcPath = (cmd.Parameters["@pcPath"].Value.ToString() == "null") ? null : cmd.Parameters["@pcPath"].Value.ToString();
                    String pcData = (cmd.Parameters["@pcData"].Value.ToString() == "null") ? null : cmd.Parameters["@pcData"].Value.ToString();

                    if (pnTypeResult != "0")
                    {
                        response.Error = pcMessage;
                    }
                    else
                    {
                        response.FileData = pcData;
                        response.FileUrl = pcPath;
                    }

                    con.Close();
                }



                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex.ToString();
                return response;
            }
        }

        public static void UpdateAfterError(string constring, int uploadId, string comments)
        {
            GetFileDataResponse response = new GetFileDataResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SP_Update_After_Error ", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pnUploadId", SqlDbType.Int).Value = uploadId;
                    cmd.Parameters.Add("@pcComment", SqlDbType.VarChar).Value = comments;

                    SqlParameter pcMessageParam = new SqlParameter();
                    pcMessageParam.ParameterName = "@pcMessage";
                    pcMessageParam.SqlDbType = SqlDbType.VarChar;
                    pcMessageParam.Size = int.MaxValue;
                    pcMessageParam.Direction = ParameterDirection.Output;

                    SqlParameter pnCodeResultParam = new SqlParameter();
                    pnCodeResultParam.ParameterName = "@pnCodeResult";
                    pnCodeResultParam.SqlDbType = SqlDbType.Int;
                    pnCodeResultParam.Size = int.MaxValue;
                    pnCodeResultParam.Direction = ParameterDirection.Output;

                    SqlParameter pnTypeResultParam = new SqlParameter();
                    pnTypeResultParam.ParameterName = "@pnTypeResult";
                    pnTypeResultParam.SqlDbType = SqlDbType.Int;
                    pnTypeResultParam.Size = int.MaxValue;
                    pnTypeResultParam.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(pcMessageParam);
                    cmd.Parameters.Add(pnTypeResultParam);
                    cmd.Parameters.Add(pnCodeResultParam);


                    SqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();

                    String pcMessage = (cmd.Parameters["@pcMessage"].Value.ToString() == "null") ? null : cmd.Parameters["@pcMessage"].Value.ToString();
                    String pnTypeResult = (cmd.Parameters["@pnTypeResult"].Value.ToString() == "null") ? null : cmd.Parameters["@pnTypeResult"].Value.ToString();
                    String pnCodeResult = (cmd.Parameters["@pnCodeResult"].Value.ToString() == "null") ? null : cmd.Parameters["@pnCodeResult"].Value.ToString();


                    con.Close();
                }

            }
            catch (Exception ex)
            {
                response.Error = ex.ToString();
            }
        }



    }
    
}
