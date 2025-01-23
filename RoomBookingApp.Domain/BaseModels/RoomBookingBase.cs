using System.ComponentModel.DataAnnotations;

namespace RoomBookingApp.Core.Domain.BaseModels
{
    public abstract class RoomBookingBase : IValidatableObject
    {
        [Required]
        [StringLength(80)]
        public string? FullName { get; set; }

        [Required]
        [StringLength(80)]
        [EmailAddress]
        public string? Email { get; set; }


        public DateTime Date { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (Date < DateTime.UtcNow.Date) yield return new ValidationResult("Date must be in the future.", new[] { nameof(Date) });
        }
    }
}