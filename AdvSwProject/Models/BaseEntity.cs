namespace AdvSwProject.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; }

        public DateTime? DeletedDate { get; set; }

        public DateTime? EditDate { get; set; }
        public int? EditId { get; set; }

        public DateTime? createdDate { get; set; }
        public int? createdId { get; set; }

    }
}
