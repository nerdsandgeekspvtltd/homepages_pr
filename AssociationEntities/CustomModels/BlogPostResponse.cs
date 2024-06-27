using AssociationEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociationEntities.CustomModels
{
    public class BlogPostResponse
    {
        public List<BlogPost> blogPosts { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int MaximumCount { get; set; }
    }



}
