using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniATM.Infracstructure.SqlServer.Repositories.SqlServer.DataContext
{
    public class Customer
    {
        public Guid Id { get; set; }
        [MaxLength(50)]//này nói với cột name alf mày đc 50 ký tự thôi
        public required string Name { get; set; }
    }
}
