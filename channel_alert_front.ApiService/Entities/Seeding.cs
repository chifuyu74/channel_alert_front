using channel_alert_front.Shared.Enums;

namespace channel_alert_front.ApiService.Entities;

public interface ISeeding<T>
{
    public List<T> Seeding();
}

#region User
public class UserSeeding : ISeeding<User>
{
    public List<User> Seeding()
    {
        string name1 = $"name_{1}";
        User user1 = new()
        {
            FirstName = name1,
            LastName = name1,
            UserName = name1,
        };

        string name2 = $"name_{2}";
        User user2 = new()
        {
            FirstName = name2,
            LastName = name2,
            UserName = name2,
        };

        List<User> users = [user1, user2];

        return users;
    }
}
#endregion

#region AlertHistory
public class AlertHistorySeeding : ISeeding<AlertHistory>
{
    public List<AlertHistory> Seeding()
    {
        return [
            new()
            {
                UserId = "dff4afe1-f111-4370-9371-25d7e2018a27",
                OnAlerted = DateTime.Now,
                Platform = EPlatform.Discord,
                Text = "discord alert!",
            },
            new()
            {
                UserId = "3207822c-c7b3-4e36-9f0e-c177915db697",
                OnAlerted = DateTime.Now,
                Platform = EPlatform.Github,
                Text = "github alert!",
            },
        ];
    }
}
#endregion