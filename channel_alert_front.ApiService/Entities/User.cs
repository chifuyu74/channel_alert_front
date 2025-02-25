#pragma warning disable 8618
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace channel_alert_front.ApiService.Entities;

[Table(nameof(User))]
public class User : IdentityUser
{
    [Column(nameof(UserId)), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime? LastLoggedIn { get; set; } = null;

    public DateTime? TokenExpiryTime { get; set; } = null;

    public ICollection<string>? Roles { get; init; } = [];

    public string IPv4 { get; set; } = string.Empty;

    public string IPv6 { get; set; } = string.Empty;

    public bool UseGlobalDiscordChannelId { get; set; } = true;

    public string GlobalDiscordChannelId { get; set; } = string.Empty;

    public ICollection<AlertHistory> AlertHistories { get; set; } = [];

    public ICollection<Subscription> Subscriptions { get; set; } = [];

}
#pragma warning restore