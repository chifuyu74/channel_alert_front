using Microsoft.AspNetCore.Components.Server.Circuits;

namespace channel_alert_front.Web.States;
public class CustomCircuitHandler : CircuitHandler
{
    public bool IsConnected { get; private set; } = false;

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        IsConnected = true;
        return Task.CompletedTask;
    }

    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        IsConnected = false;
        return Task.CompletedTask;
    }
}
