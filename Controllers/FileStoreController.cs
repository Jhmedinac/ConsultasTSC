using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ConsultasTSC.Swagger;
using ConsultasTSC.Filter;
using ConsultasTSC.Models;
using ConsultasTSC.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;

namespace ConsultasTSC.Controllers
{
    //[Route("api/[controller]")]
    //[Route("[controller]")]
    [Route("FileStore/[controller]")]

    [ApiController]
    public class FileStoreController : Controller
    {
        private readonly IConfiguration _configuration;

        public FileStoreController(IConfiguration config)
        {
            _configuration = config;
        }
        /// <summary>
        /// Service to upload a file.
        /// </summary>       
        /// <param name="value">
        /// <b>IdFile(string):</b>Id of the file <br/>
        /// <b>Flexfields(string):</b> Flexfields key-value of the file <br/>
        /// The flexfields included must have the following format: [{"FlexfieldKey": "string", "FlexfieldValue":"string"}].
        /// </param>
        /// <returns>ServiceResponse</returns>
        /// /// <remarks>
        /// The flexfields included must have the following format:
        ///
        ///  [
        ///     {
        ///         "FlexfieldKey":"string",
        ///         "FlexfieldValue":"string"
        ///     } 
        /// ]
        ///
        /// </remarks>
        /// <response code="200">OK. The service was executed with success.</response>
        /// <response code="400">BadRequest. An error on the service execution happened.</response>
        /// <response code="401">Unauthorized. The credential provided are invalid.</response>
        /// 

        [HttpPost, DisableRequestSizeLimit]
        [Route("/Upload_File")]
        [AuthFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ServiceResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ServiceResponse<object>))]
        [Produces("application/json")]
        public IActionResult Upload_File([FromForm] Upload value)
        //public IActionResult UploadFile([FromForm] Upload value, [FromHeader(Name = "Path")] string path)
        //public IActionResult Upload_File([FromForm] Upload value, [FromHeader(Name = "Path")] string path)
        //public IActionResult UploadFile([FromHeader(Name = "Path")] string path, IFormFile file)
        {
            try
            {

                var file = value.File;

                if (file.Length > 0)
                {
                    ServiceResponse<int> response = new ServiceResponse<int>();
                    var fileSize = file.Length;
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    byte[] bytes = Encoding.Default.GetBytes(fileName);
                    fileName = Encoding.UTF8.GetString(bytes);

                    UploadServiceResponse uploadResponse = new UploadServiceResponse();
                    //string constring = _configuration.GetSection("ConnectionStrings").GetSection("catalog2ConnectionString").Value;
                    string constring = _configuration.GetSection("ConnectionStrings").GetSection("Catalog1").Value;
                    uploadResponse = FileServices.SaveFileData(constring, value.IdFile, fileName, Int32.Parse(fileSize.ToString()), value.Flexfields);

                    if (!string.IsNullOrEmpty(uploadResponse.error))
                    {
                        response.Success = false;
                        response.Message = uploadResponse.error;
                        return Ok(response);
                    }
                    try
                    {
                        string pathToSave;
                        var headers = HttpContext.Request.Headers;


                        if (SystemParameters.FolderIsRelative)
                        {
                            pathToSave = Path.Combine(Directory.GetCurrentDirectory(), SystemParameters.AppBasePath);
                        }
                        else
                        {
                            pathToSave = SystemParameters.AppBasePath;
                            // aca viene cambio 11 junio
                            if (headers.TryGetValue("File_Path", out var File_Path))
                            {
                                //pathToSave = ($"Header value: {Path}");
                                pathToSave = ($"{File_Path}");
                            }
                        }

                        
                          

                        pathToSave = Path.Combine(pathToSave, uploadResponse.directory);
                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }

                        fileName = uploadResponse.ReferenceId.ToString() + '_' + fileName;

                        var fullPath = Path.Combine(pathToSave, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        response.Result = uploadResponse.ReferenceId;
                        response.Success = true;
                        response.Message = "File uploaded successfully";
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        FileServices.UpdateAfterError(constring, uploadResponse.ReferenceId, ex.ToString());
                        response.Success = false;
                        response.Message = "An error ocurred at upload the file: " + ex.ToString();
                        return Ok(response);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Service to get data of a file.
        /// </summary>       
        /// <param name="idFile1"> Id of the file to retrieve data. </param>
        /// <returns>File</returns>
        /// <response code="200">OK.The service was executed with success.</response>
        /// <response code="400">BadRequest. An error on the service execution happened.</response>
        /// <response code="401">Unauthorized. The credential provided are invalid.</response>
        /// 
        [HttpGet("/DataFile/{idFile1}")]
        [AuthFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ServiceResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ServiceResponse<object>))]
        [Produces("application/json")]
        public async Task<IActionResult> GetFile_U (string idFile1)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                GetFileDataResponse retrieveResponse = new GetFileDataResponse();
                string constring = _configuration.GetSection("ConnectionStrings").GetSection("Catalog1").Value;
                retrieveResponse = FileServices.GetFileData(constring, Int32.Parse(idFile1));

                if (!string.IsNullOrEmpty(retrieveResponse.Error))
                {
                    response.Success = false;
                    response.Message = retrieveResponse.Error;
                    return Ok(response);
                }

                response.Result = retrieveResponse.FileData;
                response.Success = true;
                response.Message = "Data retieved succesfully";

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
                return Problem(ex.ToString(), null, 500, "An error ocurred.", null);
            }
        }

        /// <summary>
        /// Service to get a file.
        /// </summary>       
        /// <param name="idFile1"> Id of the file to retrieve </param>
        /// <returns>File</returns>
        /// <response code="200">OK. Returns the requested file.</response>
        /// <response code="400">BadRequest. An error on the service execution happened.</response>
        /// <response code="401">Unauthorized. The credential provided are invalid.</response>
        /// 
        [HttpGet("/Download/{idFile1}")]
        [AuthFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ServiceResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ServiceResponse<object>))]
        [Produces("application/json")]
        public async Task<IActionResult> DownloadFile1(string idFile1)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                GetFileDataResponse retrieveResponse = new GetFileDataResponse();
                //string constring = _configuration.GetSection("ConnectionStrings").GetSection("catalog2ConnectionString").Value;
                string constring = _configuration.GetSection("ConnectionStrings").GetSection("Catalog1").Value;
                retrieveResponse = FileServices.GetFileData(constring, Int32.Parse(idFile1));

                if (!string.IsNullOrEmpty(retrieveResponse.Error))
                {
                    response.Success = false;
                    response.Message = retrieveResponse.Error;
                    return Ok(response);
                }

                string path;
                var headers = HttpContext.Request.Headers;
                if (SystemParameters.FolderIsRelative)
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(),SystemParameters.AppBasePath);
                    //path = cust
                }
                else
                {
                    string pathToSave = "";
                    // aca viene cambio 11 junio
                    if (headers.TryGetValue("File_Path", out var File_Path))
                    {
                        //pathToSave = ($"Header value: {Path}");
                        pathToSave = ($"{File_Path}");
                    }
                    path = Path.Combine(pathToSave, retrieveResponse.FileUrl);
                }

                var fileName = Path.GetFileName(path);
                var content = await System.IO.File.ReadAllBytesAsync(path);

                new FileExtensionContentTypeProvider()
                    .TryGetContentType(fileName, out string contentType);
                return File(content, contentType, fileName);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
                return BadRequest();
            }
        }

        //[HttpGet("files")]
        //public IActionResult GetFiles([FromQuery] string path)
        //{
        //    if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
        //    {
        //        return BadRequest("Invalid path.");
        //    }
        //    var files = Directory.GetFiles(path);
        //    return Ok(files);
        //}

        //[HttpPost, DisableRequestSizeLimit]
        //[Route("/Upload_File_Path")]
        //[AuthFilter]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<object>))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ServiceResponse<object>))]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ServiceResponse<object>))]
        //[Produces("application/json")]
        //public IActionResult Upload_File_Path([FromForm] Upload value, [FromHeader(Name = "Path")] string path)
        //{
        //    try
        //    {
        //        var file = value.File;

        //        if (file.Length > 0)
        //        {
        //            ServiceResponse<int> response = new ServiceResponse<int>();
        //            var fileSize = file.Length;
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            byte[] bytes = Encoding.Default.GetBytes(fileName);
        //            fileName = Encoding.UTF8.GetString(bytes);

        //            UploadServiceResponse uploadResponse = new UploadServiceResponse();
        //            string constring = _configuration.GetSection("ConnectionStrings").GetSection("Catalog1").Value;
        //            uploadResponse = FileServices.SaveFileData(constring, value.IdFile, fileName, Int32.Parse(fileSize.ToString()), value.Flexfields);

        //            if (!string.IsNullOrEmpty(uploadResponse.error))
        //            {
        //                response.Success = false;
        //                response.Message = uploadResponse.error;
        //                return Ok(response);
        //            }
        //            try
        //            {
        //                string pathToSave;

        //                if (SystemParameters.FolderIsRelative)
        //                {
        //                    pathToSave = Path.Combine(Directory.GetCurrentDirectory(), SystemParameters.AppBasePath);
        //                }
        //                else
        //                {
        //                    pathToSave = SystemParameters.AppBasePath;
        //                }

        //                // Aquí se agrega el path recibido del encabezado
        //                pathToSave = Path.Combine(pathToSave, path, uploadResponse.directory);
        //                if (!Directory.Exists(pathToSave))
        //                {
        //                    Directory.CreateDirectory(pathToSave);
        //                }

        //                fileName = uploadResponse.ReferenceId.ToString() + '_' + fileName;

        //                var fullPath = Path.Combine(pathToSave, fileName);
        //                using (var stream = new FileStream(fullPath, FileMode.Create))
        //                {
        //                    file.CopyTo(stream);
        //                }
        //                response.Result = uploadResponse.ReferenceId;
        //                response.Success = true;
        //                response.Message = "File uploaded successfully";
        //                return Ok(response);
        //            }
        //            catch (Exception ex)
        //            {
        //                FileServices.UpdateAfterError(constring, uploadResponse.ReferenceId, ex.ToString());
        //                response.Success = false;
        //                response.Message = "An error occurred at upload the file: " + ex.ToString();
        //                return Ok(response);
        //            }
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.ToString());
        //    }
        //}


    }
}


//**** Endpoint cliente *****

//var httpClient = new HttpClient();
//var response = await httpClient.GetAsync($"https://tuapi.com/files?path=C:\\example\\path");
//if (response.IsSuccessStatusCode)
//{
//    var files = await response.Content.ReadAsAsync<string[]>();
//    // Procesar los archivos...
//}