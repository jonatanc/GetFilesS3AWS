using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SeedWork.Enums
{
    public enum DownloadCategory
    {
        Category1,
        Category2,
        Category3,
        Category4,
        Site
    }

    public static class DownloadCategoryExtensions
    {
        public static string GetString(this DownloadCategory downloadCategory)
        {
            switch (downloadCategory)
            {
                case DownloadCategory.Category1:
                    return "Bucket/Category1/";
                case DownloadCategory.Category2:
                    return "Bucket/Category2/";
                case DownloadCategory.Category3:
                    return "Bucket/Category3/";
                case DownloadCategory.Category4:
                    return "Bucket/Category4/";
                case DownloadCategory.Site:
                    return "Bucket/site/";
                default:
                    return "NO VALUE GIVEN";
            }
        }

        public static string GetNameEnum(this DownloadCategory downloadCategory)
        {
            switch (downloadCategory)
            {
                case DownloadCategory.Category1:
                    return "Category1";
                case DownloadCategory.Category2:
                    return "Category2";
                case DownloadCategory.Category3:
                    return "Category3";
                case DownloadCategory.Category4:
                    return "Category4";
                case DownloadCategory.Site:
                    return "Site";
                default:
                    return "NO VALUE CATEGORY";
            }
        }
    }
}
