using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.DAL.Entities
{
    public class Game : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
