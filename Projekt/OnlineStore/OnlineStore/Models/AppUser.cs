//534
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Models
{
    //public enum Cities
    //{
    //    None, London, Paris, Chicago
    //}

    //public enum QualificationLevels
    //{
    //    None, Basic, Advanced
    //}

    public class AppUser : IdentityUser
    {
        //public Cities City { get; set; }
        //public QualificationLevels Qualifications { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
    }
}