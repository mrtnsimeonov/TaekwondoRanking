using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Category
    {
        public Category()
        {
            Subcmpt2s = new HashSet<Subcmpt2>();
        }

        public string Idctgr { get; set; } = null!;
        public string? Ageclass { get; set; }
        public string? Mf { get; set; }
        public string? Namectgr { get; set; }
        public byte[] SsmaTimeStamp { get; set; } = null!;

        public virtual Agekl? AgeclassNavigation { get; set; }
        public virtual ICollection<Subcmpt2> Subcmpt2s { get; set; }
    }
}
