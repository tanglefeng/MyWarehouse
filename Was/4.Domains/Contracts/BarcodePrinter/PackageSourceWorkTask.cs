using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Domain.Entity.Package
{
    public class PackageSourceWorkTask : SourceWorkTask<string>
    {
        public string Executor { get; set; }
        public string CustomField01 { get; set; }
        public string CustomField02 { get; set; }
        public string CustomField03 { get; set; }
        public string CustomField04 { get; set; }
        public string CustomField05 { get; set; }
        public string CustomField06 { get; set; }
        public string CustomField07 { get; set; }
        public string CustomField08 { get; set; }
        public string CustomField09 { get; set; }
        public string CustomField10 { get; set; }
    }
}