using System.ComponentModel;

namespace Domain.Enums
{
    public enum AppStatus
    {
        [Description("InProgress")]
        Inprogress,
        [Description("Pending")]
        Pending,
        [Description("Approved")]
        Approved,
        [Description("Completed")]
        Completed,
        [Description("Initiated")]
        Initiated,
        [Description("Rejected")]
        Rejected,
    }
}
