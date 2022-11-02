using System;
using System.Collections.Generic;

namespace authentication.Models
{
    public partial class UserTb
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class Jwt
    {
        //internal char[] key;
  public string Key { get; set; }
        public string Issuer { get; set; }
       public string Audience { get; set; }
        public string Subject { get; set; }

   }

}
