using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheCodexUiChecks
{
    private static readonly Lazy<ContentRegistry> FreeRefreshRegistry = new(() => ContentRegistry.Build(new StandardContentPackage()));
}
