using RuS.Domain.Contracts;

namespace RuS.Domain.Entities.Projects
{
    public class Discussion : AuditableEntity<int>
    {
        public string Comment { get; set; }
        public int ProjectId { get; set; }
        public int? TaskId { get; set; }

        public Project Project { get; set; }
        public Task Task { get; set; }
    }
}
