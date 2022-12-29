using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _dataContext;

        public CommentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Comment> Comments => _dataContext.Comments;

        public IUnitOfWork UnitOfWork => _dataContext;

        public void Add(Comment comment)
        {
            _dataContext.Comments.Add(comment);
        }

        public void Delete(Comment comment)
        {
            _dataContext.Entry(comment).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }

        public void Update(Comment comment)
        {
            _dataContext.Entry(comment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        /// <summary>
        /// Get list comment detail
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommentDetailQueryModel>> GetCommentDetails()
        {
            var listCommentDetails = await _dataContext.Comments.Where(x => x.State == 1).Join(_dataContext.Users, c => c.UserId, u => u.Id,
                                                                        (c, u) => new CommentDetailQueryModel
                                                                        {
                                                                            Id = c.Id,
                                                                            ProductId = c.ProductId,
                                                                            Content = c.Content,
                                                                            Username = u.Username,
                                                                            Avatar = u.UrlAvatar,
                                                                            Rate = c.Rate
                                                                        }).ToListAsync();
            return listCommentDetails;

        }

    }
}