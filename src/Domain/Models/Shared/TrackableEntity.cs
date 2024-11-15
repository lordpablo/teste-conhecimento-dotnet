using System.ComponentModel.DataAnnotations.Schema;

namespace SampleTest.Domain.Models.Shared
{
    public abstract class TrackableEntity : Entity
    {
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }
        public int? CreateUserId { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; }
        public int? UpdateUserId { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "timestamp")]
        public DateTime? DeletedAt { get; set; }
        public int? DeleteUserId { get; set; }

        public void Created(int createdBy)
        {
            CreatedAt = DateTime.Now;
            CreateUserId = createdBy;
        }

        public void Updated(int updatedBy)
        {
            UpdatedAt = DateTime.Now;
            UpdateUserId = updatedBy;
        }

        public void Deleted(int deletedBy)
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
            DeleteUserId = deletedBy;
        }
    }
}