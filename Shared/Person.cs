using System.ComponentModel.DataAnnotations;

namespace BlazorWasmTesting.Shared
{
    public class Person
    {
        public Person() { }
        public Person(int key, string firstName, string lastName, string middleName)
        {
            Key = key;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }


        [Key]
        public int Key { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string MiddleName { get; set; }
    }
}
