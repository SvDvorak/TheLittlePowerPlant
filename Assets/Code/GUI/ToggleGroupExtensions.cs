using System.Linq;
using UnityEngine.UI;

public static class ToggleGroupExtension
{
    public static Toggle GetActive(this ToggleGroup aGroup)
    {
        return aGroup.ActiveToggles().FirstOrDefault();
    }
}