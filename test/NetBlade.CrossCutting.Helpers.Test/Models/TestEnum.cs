using System.ComponentModel.DataAnnotations;

namespace NetBlade.Core.Test.Helper.Models
{
    public enum TestEnum
    {
        [Display(Name = "Test AA", Order = 1)]
        AA = 0,

        [Display(Name = "Test CC", Order = 3)]
        CC = 1,

        [Display(Name = "Test BB", Order = 2)]
        BB = 2
    }
}
