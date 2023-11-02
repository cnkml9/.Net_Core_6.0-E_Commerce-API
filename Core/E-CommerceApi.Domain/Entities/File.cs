using E_CommerceApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Domain.Entities
{
    public class File:BaseEntity
    {
        public string? FileName { get; set; }
        public string? Path { get; set; }
        public string? Storage { get; set; }

        //Updated data nin base entityden gelmesini istemediğimiz de bu yapıyı kullanabiliriz.
        //[NotMapped]
        //public override DateTime UpdatedDate { get => base.UpdatedDate; set => base.UpdatedDate = value; }
    }
}
