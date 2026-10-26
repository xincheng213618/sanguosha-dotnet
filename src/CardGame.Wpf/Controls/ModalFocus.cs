using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CardGame.Wpf.Controls;

internal static class ModalFocus
{
    public static bool HasOpenDropDown(DependencyObject root)
    {
        if (root is ComboBox { IsDropDownOpen: true }) return true;
        for (var index = 0; index < VisualTreeHelper.GetChildrenCount(root); index++)
            if (HasOpenDropDown(VisualTreeHelper.GetChild(root, index))) return true;
        return false;
    }
}
