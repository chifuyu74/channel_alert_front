using channel_alert_front.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace channel_alert_front.Shared.DataTransferObject;

public class AlertHistoryDto
{
    public int AlertHistoryId { get; set; }
    public string Text { get; set; }
    public DateTime OnAlerted { get; set; }
    public EPlatform Platform { get; set; }
}
