#pragma warning disable 8618
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using channel_alert_front.Shared.Enums;

namespace channel_alert_front.ApiService.Entities;

[Table(nameof(Subscription))]
public class Subscription
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SubscriptionId { get; set; }

    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; }

    public ESubscriptionState TCP { get; set; } = ESubscriptionState.PAUSED;

    public ESubscriptionState UDP { get; set; } = ESubscriptionState.PAUSED;

    public ESubscriptionState WebSocket { get; set; } = ESubscriptionState.PAUSED;

    public ESubscriptionState Redis { get; set; } = ESubscriptionState.PAUSED;

    public ESubscriptionState Kafka { get; set; } = ESubscriptionState.PAUSED;

    public string IPv4 { get; set; } = string.Empty;

    public string IPv6 { get; set; } = string.Empty;

    [ForeignKey(nameof(PlatformId))]
    public int PlatformId { get; set; }

    public EPlatform Platform { get; set; } = EPlatform.Unknown;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string DiscordChannelId { get; set; } = string.Empty;

    public User User { get; set; }
}
#pragma warning restore