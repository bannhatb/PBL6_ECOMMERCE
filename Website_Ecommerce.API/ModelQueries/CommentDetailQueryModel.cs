namespace Website_Ecommerce.API.ModelQueries
{
    public class CommentDetailQueryModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public int Rate { get; set; }
        public int ProductId { get; set; }
    }
}