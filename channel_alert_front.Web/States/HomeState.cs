using Microsoft.AspNetCore.Components.Web;
using System;
using System.Diagnostics;

namespace channel_alert_front.Web.States;

public enum EDocumentType
{
    PDF,
    PNG,
    JPG,
}

public class HomeState
{
    public HomeState()
    {
        Init();
    }

    public void Init()
    {
        foreach (string type in Enum.GetNames<EDocumentType>())
            DocumentTypeItems.Add(type, type);
    }

    private bool overlayed = false;
    public bool Overlayed
    {
        get => overlayed;
        set
        {
            overlayed = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    EDocumentType _documentType = EDocumentType.PDF;
    public EDocumentType DocumentType
    {
        get => _documentType;
        set
        {
            if (value == _documentType)
                return;

            _documentType = value;
            NotifyStateChanged();
        }
    }

    public Dictionary<string, string> DocumentTypeItems = [];
}
