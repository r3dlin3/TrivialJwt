using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TrivialJwt.Models
{
    public class RefreshTokenModel
    {
        [Required]
#pragma warning disable IDE1006 // Styles d'affectation de noms
        public string refresh_token { get; set; }
#pragma warning restore IDE1006 // Styles d'affectation de noms


    }
}
