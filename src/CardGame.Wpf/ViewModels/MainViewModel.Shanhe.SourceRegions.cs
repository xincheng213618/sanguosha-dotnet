using CardGame.Core.Campaigns;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private bool _updatingShanheSourceRegions;
    private ShanheSourceRegionOption? _selectedShanheSourceRegion;
    public ObservableRangeCollection<ShanheSourceRegionOption> ShanheSourceRegions { get; } = [];
    public bool HasShanheSourceRegionChoices => ShanheSourceRegions.Count > 1;
    public ShanheSourceRegionOption? SelectedShanheSourceRegion
    {
        get => _selectedShanheSourceRegion;
        set
        {
            if (value is null || !ShanheSourceRegions.Contains(value) || !SetProperty(ref _selectedShanheSourceRegion, value)) return;
            if (!_updatingShanheSourceRegions) RefreshShanhePresentation();
        }
    }

    private ShanheNodeDefinition[] FilterShanheSourceRegionNodes(ShanheNodeDefinition[] nodes, bool sourceEdition,
        ShanheNodeDefinition? focus, string? currentNodeId, IReadOnlySet<string> available, IReadOnlySet<string> completed)
    {
        var groups = sourceEdition ? nodes.Where(node => !string.IsNullOrWhiteSpace(node.ChapterTitle))
            .GroupBy(node => node.ChapterTitle!, StringComparer.Ordinal).ToArray() : [];
        var offered = groups.Where(group => group.Any(node => available.Contains(node.Id) || node.Id == currentNodeId || completed.Contains(node.Id))).ToArray();
        if (offered.Length == 0) offered = groups;
        var choices = offered.Select(group => new ShanheSourceRegionOption(group.Key, group.Key.Split('·').Last().Trim())).ToArray();
        _updatingShanheSourceRegions = true;
        try
        {
            if (!ShanheSourceRegions.SequenceEqual(choices))
            {
                ShanheSourceRegions.Clear();
                foreach (var option in choices) ShanheSourceRegions.Add(option);
            }
            var selected = choices.FirstOrDefault(option => option.Key == _selectedShanheSourceRegion?.Key)
                ?? choices.FirstOrDefault(option => option.Key == focus?.ChapterTitle) ?? choices.FirstOrDefault();
            SetProperty(ref _selectedShanheSourceRegion, selected, nameof(SelectedShanheSourceRegion));
        }
        finally { _updatingShanheSourceRegions = false; }
        RaisePropertyChanged(nameof(HasShanheSourceRegionChoices));
        return _selectedShanheSourceRegion is { } region ? nodes.Where(node => node.ChapterTitle == region.Key).ToArray() : nodes;
    }
}

public sealed record ShanheSourceRegionOption(string Key, string Name);
