using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Classes
{
    class Client
    {
        public string Token { get; set; }
        public DateTime DateConnect { get; set; }

        public Client()
        {
            Random random = new Random();
            string Chars = "iuwgeiugkgdgiugowerugh98y24w9u8948h";

            Token = new string(Enumerable.Repeat(Chars, 15).Select(x => x[random.Next(Chars.Length)]).ToArray());
            DateConnect = DateTime.Now;
        }
    }
}
