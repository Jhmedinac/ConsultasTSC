
using ConsultasTSC.Filter;
using ConsultasTSC.Models;
using ConsultasTSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace ConsultasTSC.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class FileController : Controller
    {

        private readonly IConfiguration _configuration;

        public FileController(IConfiguration config)
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
        [Route("/Upload")]
        [AuthFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ServiceResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ServiceResponse<object>))]
        [Produces("application/json")]
        public IActionResult Upload([FromForm]Upload value)
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
                    string constring = _configuration.GetSection("ConnectionStrings").GetSection("catalog2ConnectionString").Value;
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
                        if (SystemParameters.FolderIsRelative)
                        {
                            pathToSave = Path.Combine(Directory.GetCurrentDirectory(), SystemParameters.AppBasePath);
                        }else
                        {
                            pathToSave =  SystemParameters.AppBasePath;
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
                    catch(Exception ex)
                    {
                        FileServices.UpdateAfterError(constring, uploadResponse.ReferenceId, ex.ToString());
                        response.Success = false;
                        response.Message = "An error ocurred at upload the file: "+ex.ToString();
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
        /// <param name="idFile"> Id of the file to retrieve data. </param>
        /// <returns>File</returns>
        /// <response code="200">OK.The service was executed with success.</response>
        /// <response code="400">BadRequest. An error on the service execution happened.</response>
        /// <response code="401">Unauthorized. The credential provided are invalid.</response>
        /// 
        [HttpGet("/Data/{idFile}")]
        [AuthFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ServiceResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ServiceResponse<object>))]
        [Produces("application/json")]
        public async Task<IActionResult> GetFileData(string idFile)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                GetFileDataResponse retrieveResponse = new GetFileDataResponse();
                string constring = _configuration.GetSection("ConnectionStrings").GetSection("Catalog2").Value;
                retrieveResponse = FileServices.GetFileData(constring, Int32.Parse(idFile));

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
        /// <param name="idFile"> Id of the file to retrieve </param>
        /// <returns>File</returns>
        /// <response code="200">OK. Returns the requested file.</response>
        /// <response code="400">BadRequest. An error on the service execution happened.</response>
        /// <response code="401">Unauthorized. The credential provided are invalid.</response>
        /// 
        [HttpGet("/Download/{idFile}")]
        [AuthFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ServiceResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ServiceResponse<object>))]
        [Produces("application/json")]
        public async Task<IActionResult> DownloadFile(string idFile)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
                {
                GetFileDataResponse retrieveResponse = new GetFileDataResponse();
                string constring = _configuration.GetSection("ConnectionStrings").GetSection("catalog2ConnectionString").Value;
                retrieveResponse = FileServices.GetFileData(constring, Int32.Parse(idFile));

                if (!string.IsNullOrEmpty(retrieveResponse.Error))
                {
                    response.Success = false;
                    response.Message = retrieveResponse.Error;
                    return Ok(response);
                }

                string path;
                if (SystemParameters.FolderIsRelative)
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), SystemParameters.AppBasePath);
                } else
                {
                    path = Path.Combine(SystemParameters.AppBasePath, retrieveResponse.FileUrl);
                }

                    var fileName = Path.GetFileName(path);
                    var content = await System.IO.File.ReadAllBytesAsync(path);

                    new FileExtensionContentTypeProvider()
                        .TryGetContentType(fileName, out string contentType);
                    return File(content, contentType, fileName);

                }
                catch(Exception ex)
                {
                response.Success = false;
                response.Message = ex.ToString();
                return BadRequest() ;
                }
        }

    }
}
