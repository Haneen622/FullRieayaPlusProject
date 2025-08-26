using System.ComponentModel.DataAnnotations;

public class EmergencyContact
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = "";

    [Required, MaxLength(50)]
    public string Phone { get; set; } = "";

    [Required, MaxLength(100)]
    public string Relationship { get; set; } = "";
}
