using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Response
{
    public class FileResponse
    {
        public List<File> Files { get; set; }

        public List<SubCategory> SubCategorys { get; set; }
        
        public Links Links { get; set; }
        public Meta Meta { get; set; }
    }


    public class File
    {
        public string Name { get; set; }

        public string Extension { get; set; }

        public string Link { get; set; }
    }

    public class SubCategory
    {
        public string Name { get; set; }

        public List<string> AdicionalPaths { get; set; }

    }
}
