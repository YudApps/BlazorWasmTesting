using System.ComponentModel.DataAnnotations;

namespace BlazorWasmTesting.Shared
{
    public record Person(
        [property:Key]
        int Key,

        [property:Required]
        [property:StringLength(255)]
        string FirstName,

        [property:Required]
        [property:StringLength(255)]
        string LastName,

        [property:StringLength(255)]
        string MiddleName
    );
}
