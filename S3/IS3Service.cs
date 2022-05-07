using ADAIConnect.Domain.Response;
using ADAIConnect.Domain.SeedWork.Enums;
using ADAIConnect.Domain.ViewModels;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAIConnect.Infrastructure.Service.S3
{
    public interface IS3Service
    {
        Task<FileResponse> ListFiles(DownloadCategory category);

        Task<File> GetFile(DownloadCategory categoria, string name);

        Task<FileResponse> ListSubCategoryFiles(DownloadCategory category, string subCategory, string adicionalPath);

        Task<Domain.Response.File> GetSubCategoryFile(DownloadCategory category, string subCategory, string adicionalPath, string name);
    }
}
