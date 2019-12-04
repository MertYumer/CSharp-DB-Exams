namespace VaporStore.DataProcessor.Dto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportCardDto
    {
        [Required]
        [RegularExpression(@"^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{3}$")]
        public string Cvc { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
