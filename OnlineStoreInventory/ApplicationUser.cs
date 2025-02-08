using Microsoft.AspNetCore.Identity;

namespace OnlineStoreInventory.DataBase
{
    public class ApplicationUser : IdentityUser
    {
        // Дополнительные свойства для пользователя
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}