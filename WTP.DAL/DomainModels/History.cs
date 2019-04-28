using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.DAL.DomainModels
{

    public class History : IEntity
    {
        public int Id { get; set; }
        public DateTime DateOfOperation { get; set; }
        public string Description { get; set; }

        public Admin Admin { get; set; }
        public AppUser AppUser { get; set; }
        public AdminOperation Operation { get; set; }
    }
}
