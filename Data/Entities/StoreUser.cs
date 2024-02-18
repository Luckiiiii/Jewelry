using Microsoft.AspNetCore.Identity;

namespace Jewelry.Data.Entities
{
    public class StoreUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber {  get; set; }
    } 
}