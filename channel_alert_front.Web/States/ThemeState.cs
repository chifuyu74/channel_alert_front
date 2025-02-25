using Microsoft.FluentUI.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace channel_alert_front.Web.States;

public class ThemeState
{
    public event Action? OnChange = null;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public string storageName = "theme";

    public ThemeState()
    {
        InitCollections();
    }

    void InitCollections()
    {
        foreach (string color in Enum.GetNames<OfficeColor>())
        {
            string colorName = color.ToString();
            OfficeColors.Add(colorName, colorName);
        }

        foreach (string mode in Enum.GetNames<DesignThemeModes>())
        {
            string modeName = mode.ToString();
            DesignThemes.Add(modeName, modeName);
        }
    }

    public Dictionary<string, string> DesignThemes = [];
    private DesignThemeModes _mode = DesignThemeModes.System;
    public DesignThemeModes Mode
    {
        get => _mode;
        set
        {
            if (_mode == value)
                return;

            _mode = value;
            NotifyStateChanged();
        }
    }

    public Dictionary<string, string> OfficeColors = [];


    private OfficeColor _officeColor = OfficeColor.Default;
    public OfficeColor Color
    {
        get => _officeColor;
        set
        {
            if (_officeColor == value)
                return;

            _officeColor = value;
            NotifyStateChanged();
        }
    }
}