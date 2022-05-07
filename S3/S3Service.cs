using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Service.S3
{
    public class S3Service : IS3Service
    {
        private AmazonS3Client Client { get; set; }
        private string bucket { get; set; }
        private string UrlDownload { get; set; }

        public S3Service(IConfiguration config)
        {
            string AccesskeyID = config.GetSection("AWSS3")["AccesskeyID"];
            string SecretAccessKey = config.GetSection("AWSS3")["SecretAccessKey"];
            UrlDownload = config.GetSection("AWSS3")["UrlDownload"];

            bucket = config.GetSection("AWSS3")["Bucket"];

            RegionEndpoint region = RegionEndpoint.USEast1;

            BasicAWSCredentials credentials = new BasicAWSCredentials(AccesskeyID, SecretAccessKey);
            Client = new AmazonS3Client(credentials, region);
        }

        public async Task<FileResponse> ListFiles(DownloadCategory category)
        {
            FileResponse response = new FileResponse();
            response.Files = new List<Domain.Response.File>();

            var S3 = await Client.ListObjectsAsync(new ListObjectsRequest { BucketName = bucket });

            foreach (S3Object obj in S3.S3Objects.Where(obj => obj.Key.Contains(DownloadCategoryExtensions.GetString(category))))
            {
                if (DownloadCategoryExtensions.GetString(category).Equals(obj.Key))
                    continue;

                List<string> path = obj.Key.Split("/").ToList();
                if (!string.IsNullOrEmpty(obj.Key) && path[2].Contains('.'))
                {
                    Domain.Response.File file = MapFile(obj);
                    response.Files.Add(file);
                }
                else
                {
                    if (response.SubCategorys == null)
                        response.SubCategorys = new List<SubCategory>();

                    MapSubCategory(obj, response);
                }
            }

            return response;
        }

        public async Task<FileResponse> ListSubCategoryFiles(DownloadCategory category, string subCategory, string adicionalPath)
        {
            FileResponse response = new FileResponse();
            response.Files = new List<Domain.Response.File>();
            var S3 = await Client.ListObjectsAsync(new ListObjectsRequest { BucketName = bucket });

            string pathSubCategory = CreatePathS3(category, subCategory, adicionalPath);

            foreach (S3Object obj in S3.S3Objects.Where(obj => obj.Key.Contains(pathSubCategory)))
            {
                if (obj.Key.Equals(pathSubCategory))
                    continue;

                if (!string.IsNullOrEmpty(adicionalPath))
                {
                    Domain.Response.File file = MapFile(obj);
                    response.Files.Add(file);
                }
                else
                {
                    List<string> path = obj.Key.Split("/").ToList();
                    if (!string.IsNullOrEmpty(obj.Key) && path[3].Contains('.'))
                    {
                        Domain.Response.File file = MapFile(obj);
                        response.Files.Add(file);
                    }
                }
            }

            return response;
        }

        public async Task<Domain.Response.File> GetFile(DownloadCategory category, string name)
        {
            Domain.Response.File file = new Domain.Response.File();
            var S3 = await Client.ListObjectsAsync(new ListObjectsRequest { BucketName = bucket });

            foreach (S3Object obj in S3.S3Objects.Where(obj => obj.Key.Contains(DownloadCategoryExtensions.GetString(category))))
            {
                if (string.Format("{0}{1}", DownloadCategoryExtensions.GetString(category), name).Equals(obj.Key))
                {
                    file = MapFile(obj);
                }
            }

            return file;
        }

        public async Task<Domain.Response.File> GetSubCategoryFile(DownloadCategory category, string subCategory, string adicionalPath, string name)
        {
            Domain.Response.File file = new Domain.Response.File();
            var S3 = await Client.ListObjectsAsync(new ListObjectsRequest { BucketName = bucket });

            string path = CreatePathS3(category, subCategory, adicionalPath);

            foreach (S3Object obj in S3.S3Objects.Where(obj => obj.Key.Contains(path)))
            {
                if (string.Format("{0}{1}", path, name).Equals(obj.Key))
                {
                    file = MapFile(obj);
                }
            }

            return file;
        }

        private void MapSubCategory(S3Object obj, FileResponse response)
        {
            List<string> path = obj.Key.Split("/").ToList();

            var subCategorys = response.SubCategorys.Where(s => s.Name.Equals(path[2])).FirstOrDefault();
            if (subCategorys != null)
            {
                mapAdicionalPath(path, subCategorys);
            }
            else
            {
                Domain.Response.SubCategory subCategory = new Domain.Response.SubCategory();
                subCategory.Name = path[2];

                mapAdicionalPath(path, subCategory);

                response.SubCategorys.Add(subCategory);
            }
        }

        private static void mapAdicionalPath(List<string> path, SubCategory subCategory)
        {
            if (!string.IsNullOrEmpty(path[3]) && !path[3].Contains('.'))
            {
                if (subCategory.AdicionalPaths == null)
                    subCategory.AdicionalPaths = new List<string>();

                string adicionaPath = string.Empty;
                for (int i = 0; i < path.Count; i++)
                {
                    adicionaPath += i > 2 ? string.Concat("/", path[i]) : "";
                }

                if (!adicionaPath.Contains('.'))
                    subCategory.AdicionalPaths.Add(adicionaPath);

            }
        }

        private Domain.Response.File MapFile(S3Object obj)
        {
            Domain.Response.File file = new Domain.Response.File();

            List<string> path = obj.Key.Split("/").ToList();

            string name = path.LastOrDefault();
            file.Name = name;
            file.Extension = name.Substring(name.LastIndexOf('.') + 1);
            file.Link = UrlDownload + obj.Key;
            return file;
        }

        private string CreatePathS3(DownloadCategory category, string subCategory, string adicionalPath)
        {
            return !string.IsNullOrEmpty(adicionalPath) ? DownloadCategoryExtensions.GetString(category) + subCategory + adicionalPath : DownloadCategoryExtensions.GetString(category) + subCategory + "/";
        }

    }
}
