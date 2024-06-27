using AssociationEntities.Models;

namespace AssociationBusiness.Association.Interface
{
    public interface IBlogBusiness
    {
        List<Blog> GetAllBlogs();

        Task<int> CreateBlogImages(List<BlogImage> blogImages);
    }
}
