namespace Website_Ecommerce.API.ModelDtos
{
    public class ShipperDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UrlAvatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsBlock { get; set; }
        public int? WareHouseId { get; set; }

    }
}