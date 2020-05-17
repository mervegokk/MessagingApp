namespace MessaginApp.API.Models
{
    public class User
    {
   public int id { get; set; }
   public string userName { get; set; }
   public byte[] passwordHash { get; set; }

   public byte[] passworSalt { get; set; }
   
   
    }
}