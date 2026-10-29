using System.Windows.Input;
using System.Windows.Media;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Resources;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private IReadOnlyList<ShanheRestChoiceCard> _shanheRestChoices = [];
    private ShanheRestChoiceCard? _selectedShanheRestChoice;
    private long _shanheRestChoiceRevision = -1;
    private RelayCommand? _confirmShanheRestChoiceCommand;

    public IReadOnlyList<ShanheRestChoiceCard> ShanheRestChoices => _shanheRestChoices;
    public bool IsShanheRestChoiceOpen => _shanheRestChoices.Count == 2;
    public ShanheRestChoiceCard? SelectedShanheRestChoice
    {
        get => _selectedShanheRestChoice;
        set
        {
            if (value is not null && !_shanheRestChoices.Contains(value)) return;
            if (SetProperty(ref _selectedShanheRestChoice, value)) _confirmShanheRestChoiceCommand?.NotifyCanExecuteChanged();
        }
    }
    public ICommand ConfirmShanheRestChoiceCommand => _confirmShanheRestChoiceCommand ??= new RelayCommand(() =>
    {
        if (SelectedShanheRestChoice is { } choice && IsCurrentShanheChoice(choice.Choice))
            ChooseShanheOption(choice.Choice);
    }, () => IsShanheRestChoiceOpen && SelectedShanheRestChoice is { Choice.IsEnabled: true } choice && IsCurrentShanheChoice(choice.Choice));

    private void RefreshShanheRestChoices()
    {
        var session = _shanheSession;
        var state = session?.Snapshot;
        var node = state?.Phase == ShanheCampaignPhase.NodeChoice
            ? session!.Definition.Nodes.Single(node => node.Id == state.CurrentNodeId) : null;
        var selectedId = state?.Revision == _shanheRestChoiceRevision ? _selectedShanheRestChoice?.Choice.Id : null;
        _shanheRestChoices = node?.ChapterRole == ShanheChapterNodeRole.Rest
            ? ShanheChoices.Select(choice =>
            {
                var rewardId = node.Choices?.FirstOrDefault(option => option.Id == choice.Id)?.RewardId;
                var reward = session!.Definition.Rewards.FirstOrDefault(reward => reward.Id == rewardId);
                return reward?.Bonuses switch
                {
                    { MaxHp: > 0 } => new ShanheRestChoiceCard(choice, "体", $"当前体力上限：{state!.MaxHp}"),
                    { Draw: > 0 } => new ShanheRestChoiceCard(choice, "摸", $"当前基础摸牌：{2 + state!.Bonuses.Draw}"),
                    _ => null
                };
            }).OfType<ShanheRestChoiceCard>().ToArray() : [];
        _shanheRestChoiceRevision = state?.Revision ?? -1;
        _selectedShanheRestChoice = _shanheRestChoices.FirstOrDefault(choice => choice.Choice.Id == selectedId);
        RaisePropertyChanged(nameof(ShanheRestChoices));
        RaisePropertyChanged(nameof(SelectedShanheRestChoice));
        RaisePropertyChanged(nameof(IsShanheRestChoiceOpen));
        _confirmShanheRestChoiceCommand?.NotifyCanExecuteChanged();
    }
}

public sealed record ShanheRestChoiceCard(ShanheCampaignChoice Choice, string Glyph, string CurrentValue)
{
    public string Name => Choice.Name;
    public string Description => Choice.Description;
    public string AccessibleName => $"{Choice.AccessibleName}，{CurrentValue}";
    public bool IsEnabled => Choice.IsEnabled;
    public ImageSource? Artwork => AssetImages.Load($"Assets/Shanhe/reward_{(Glyph == "摸" ? "mopai" : "tili")}.png");
}
