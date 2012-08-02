using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bootstrap.AutoMapper;
using RavenCms.Controllers;

namespace RavenCms
{
    public class MapCreator : IMapCreator
    {
        public void CreateMap(IProfileExpression mapper)
        {
            mapper.CreateMap<Content.Content, ContentViewModel>();
            mapper.CreateMap<ContentViewModel, Content.Content>();
        }
    }
}
