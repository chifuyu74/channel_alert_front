#pragma warning disable 8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using channel_alert_front.Shared.Enums;

namespace channel_alert_front.ApiService.Entities;

[Table(nameof(AlertHistory))]
public class AlertHistory
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AlertHistoryId { get; set; }

    [ForeignKey("Id")]
    public string UserId { get; set; }

    public EPlatform Platform { get; set; } = EPlatform.Unknown;

    public string Text { get; set; } = string.Empty;

    public DateTime OnAlerted { get; set; }

    public User User { get; set; }
}
#pragma warning restore