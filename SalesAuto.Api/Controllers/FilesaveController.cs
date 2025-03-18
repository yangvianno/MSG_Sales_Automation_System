using LeadLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesAuto.Api.Repositories;
using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FilesaveController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<FilesaveController> logger;
        private readonly ILeadsRepo _leadsRepository;
        private readonly IChiTieuSoLuongRepo chiTieuSoLuongRepository;

        public FilesaveController(IWebHostEnvironment env,
            ILogger<FilesaveController> logger
            , ILeadsRepo leadsRepository
            , IChiTieuSoLuongRepo chiTieuSoLuongRepository)
        {
            this.env = env;
            this.logger = logger;
            _leadsRepository = leadsRepository;
            this.chiTieuSoLuongRepository = chiTieuSoLuongRepository;
        }

        [HttpPost]
        public async Task<ActionResult<IList<UploadResult>>> PostFile(
            [FromForm] IEnumerable<IFormFile> files, string MaBenhVien="")
        {
            var maxAllowedFiles = 3;
            long maxFileSize = 1024 * 1024 * 15;
            var filesProcessed = 0;
            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
            List<UploadResult> uploadResults = new();
            string MaBenhVienNguon = MaBenhVien;
            if (MaBenhVien == "")
            {
                MaBenhVienNguon = "O";
                try
                {
                    if (Request != null)
                    {
                        var HeaderMaBenhVienNguon = Request.Headers["MaBenhVienNguon"];
                        if (HeaderMaBenhVienNguon.Count >= 0)
                        {
                            MaBenhVienNguon = HeaderMaBenhVienNguon[0].ToString();
                        }
                    }
                }
                catch
                {
                }
            }

            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                string trustedFileNameForFileStorage;
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay =
                    WebUtility.HtmlEncode(untrustedFileName);

                if (filesProcessed < maxAllowedFiles)
                {
                    if (file.Length == 0)
                    {
                        logger.LogInformation("{FileName} length is 0 (Err: 1)",
                            trustedFileNameForDisplay);
                        uploadResult.ErrorCode = 1;
                    }
                    else if (file.Length > maxFileSize)
                    {
                        logger.LogInformation("{FileName} of {Length} bytes is " +
                            "larger than the limit of {Limit} bytes (Err: 2)",
                            trustedFileNameForDisplay, file.Length, maxFileSize);
                        uploadResult.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            trustedFileNameForFileStorage =DateTime.Now.ToString("yyyyMMddHHmmss")+ untrustedFileName;
                            var path = Path.Combine(env.ContentRootPath,
                                env.EnvironmentName, "unsafe_uploads",
                                trustedFileNameForFileStorage);
                            if(!Directory.Exists($"{env.ContentRootPath}\\{env.EnvironmentName}\\unsafe_uploads"))
                            {
                                Directory.CreateDirectory($"{env.ContentRootPath}\\{env.EnvironmentName}\\unsafe_uploads");
                            }    

                            await using FileStream fs = new(path, FileMode.Create);
                            await file.CopyToAsync(fs);
                            fs.Close();

                            //LeadRepo.SaveLeadFileToDataBase(path);
                            await _leadsRepository.SaveLeadFileToDataBase(path, MaBenhVienNguon);

                            logger.LogInformation("{FileName} saved at {Path}",
                                trustedFileNameForDisplay, path);
                            uploadResult.Uploaded = true;
                            uploadResult.StoredFileName = trustedFileNameForFileStorage;
                        }
                        catch (IOException ex)
                        {
                            logger.LogError("{FileName} error on upload (Err: 3): {Message}",
                                trustedFileNameForDisplay, ex.Message);
                            uploadResult.ErrorCode = 3;
                        }
                    }

                    filesProcessed++;
                }
                else
                {
                    logger.LogInformation("{FileName} not uploaded because the " +
                        "request exceeded the allowed {Count} of files (Err: 4)",
                        trustedFileNameForDisplay, maxAllowedFiles);
                    uploadResult.ErrorCode = 4;
                }

                uploadResults.Add(uploadResult);
            }

            return new CreatedResult(resourcePath, uploadResults);
        }

        [HttpPost]
        [Route("chitieu")]
        public async Task<ActionResult<IList<UploadResult>>> PostFileChiTieu(
            [FromForm] IEnumerable<IFormFile> files)
        {
            var maxAllowedFiles = 3;
            long maxFileSize = 1024 * 1024 * 15;
            var filesProcessed = 0;
            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
            List<UploadResult> uploadResults = new();

            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                string trustedFileNameForFileStorage;
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay =
                    WebUtility.HtmlEncode(untrustedFileName);

                if (filesProcessed < maxAllowedFiles)
                {
                    if (file.Length == 0)
                    {
                        logger.LogInformation("{FileName} length is 0 (Err: 1)",
                            trustedFileNameForDisplay);
                        uploadResult.ErrorCode = 1;
                    }
                    else if (file.Length > maxFileSize)
                    {
                        logger.LogInformation("{FileName} of {Length} bytes is " +
                            "larger than the limit of {Limit} bytes (Err: 2)",
                            trustedFileNameForDisplay, file.Length, maxFileSize);
                        uploadResult.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            trustedFileNameForFileStorage = DateTime.Now.ToString("yyyyMMddHHmmss") + untrustedFileName;
                            var path = Path.Combine(env.ContentRootPath,
                                env.EnvironmentName, "unsafe_uploads",
                                trustedFileNameForFileStorage);
                            if (!Directory.Exists($"{env.ContentRootPath}\\{env.EnvironmentName}\\unsafe_uploads"))
                            {
                                Directory.CreateDirectory($"{env.ContentRootPath}\\{env.EnvironmentName}\\unsafe_uploads");
                            }

                            await using FileStream fs = new(path, FileMode.Create);
                            await file.CopyToAsync(fs);
                            fs.Close();

                            //LeadRepo.SaveLeadFileToDataBase(path);
                            await chiTieuSoLuongRepository.SaveChiTieuLasikFileToDataBase(path);

                            logger.LogInformation("{FileName} saved at {Path}",
                                trustedFileNameForDisplay, path);
                            uploadResult.Uploaded = true;
                            uploadResult.StoredFileName = trustedFileNameForFileStorage;
                        }
                        catch (IOException ex)
                        {
                            logger.LogError("{FileName} error on upload (Err: 3): {Message}",
                                trustedFileNameForDisplay, ex.Message);
                            uploadResult.ErrorCode = 3;
                        }
                    }

                    filesProcessed++;
                }
                else
                {
                    logger.LogInformation("{FileName} not uploaded because the " +
                        "request exceeded the allowed {Count} of files (Err: 4)",
                        trustedFileNameForDisplay, maxAllowedFiles);
                    uploadResult.ErrorCode = 4;
                }

                uploadResults.Add(uploadResult);
            }

            return new CreatedResult(resourcePath, uploadResults);
        }
    }
}
