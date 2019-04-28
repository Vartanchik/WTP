using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.DAL.DomainModels
{
    public class Admin: IdentityUser<int>, IEntity
    {
        public override int Id { get { return base.Id; } set { base.Id = value; } }
        public override string UserName { get { return base.UserName; } set { base.UserName = value; } }
        public override string Email { get { return base.Email; } set { base.Email = value; } }

        public List<History> Histories { get; set; }
    }
}
