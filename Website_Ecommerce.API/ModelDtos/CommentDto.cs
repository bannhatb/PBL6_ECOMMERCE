namespace Website_Ecommerce.API.ModelDtos
{
    public class CommentDto
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; }
        public int Rate { get; set; } 
    }
}