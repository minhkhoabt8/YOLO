using Auth.Core.Entities;
using Auth.Infracstructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infracstructure.Services.Implementations
{
    public class SampleServices : ISampleServices
    {
        public SampleEntities GetSampleInfo()
        {
            return new SampleEntities
            {
                MyAge = "18",
                MyName = "Khoa",
                MyFavourite = "Gaming"
            };
        }
    }
}
