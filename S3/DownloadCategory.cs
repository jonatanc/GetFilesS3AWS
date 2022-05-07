using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAIConnect.Domain.SeedWork.Enums
{
    public enum DownloadCategory
    {
        EstudosBiblicos,
        Lideranca,
        Ministerios,
        Pastoral,
        Site
    }

    public static class DownloadCategoryExtensions
    {
        public static string GetString(this DownloadCategory downloadCategory)
        {
            switch (downloadCategory)
            {
                case DownloadCategory.EstudosBiblicos:
                    return "connect/estudos-biblicos/";
                case DownloadCategory.Lideranca:
                    return "connect/lideranca/";
                case DownloadCategory.Ministerios:
                    return "connect/ministerios/";
                case DownloadCategory.Pastoral:
                    return "connect/pastoral/";
                case DownloadCategory.Site:
                    return "connect/site/";
                default:
                    return "NO VALUE GIVEN";
            }
        }

        public static string GetNameEnum(this DownloadCategory downloadCategory)
        {
            switch (downloadCategory)
            {
                case DownloadCategory.EstudosBiblicos:
                    return "EstudosBiblicos";
                case DownloadCategory.Lideranca:
                    return "Lideranca";
                case DownloadCategory.Ministerios:
                    return "Ministerios";
                case DownloadCategory.Pastoral:
                    return "Pastoral";
                case DownloadCategory.Site:
                    return "Site";
                default:
                    return "NO VALUE CATEGORY";
            }
        }
    }
}
