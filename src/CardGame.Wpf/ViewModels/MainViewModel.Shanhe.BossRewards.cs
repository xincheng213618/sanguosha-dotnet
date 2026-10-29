using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Resources;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private string? _shanheBossRewardGroupId;
    private readonly List<ShanheCampaignChoice> _shanheBossRewardChoices = [];
    public ObservableCollection<ShanheBossRewardGroup> ShanheBossRewardGroups { get; } = [];
    private ShanheBossRewardGroupSnapshot? ActiveShanheBossRewardGroup => _shanheSession?.Snapshot.BossRewardSummary?.Groups
        .FirstOrDefault(group => group.Id == _shanheBossRewardGroupId && !group.IsResolved);
    public bool HasShanheBossRewardGroup => ActiveShanheBossRewardGroup is not null;
    public bool IsShanheBossRewardOpen => _shanheSession?.Snapshot.BossRewardSummary is not null && !HasShanheBossRewardGroup;
    public bool IsShanheBonusRewardOnly => _shanheSession?.Snapshot.BossRewardSummary?.IsBonusOnly == true;
    public bool HasShanheBossFixedRewards => _shanheSession?.Snapshot.BossRewardSummary is { IsBonusOnly: false };
    public GridLength ShanheBossFixedRewardColumnWidth => HasShanheBossFixedRewards ? new(1, GridUnitType.Star) : new(0);
    public GridLength ShanheBossRewardGroupColumnWidth => new(
        ShanheBossRewardGroups.Count > 2 ? ShanheBossRewardGroups.Count + 1 : 2, GridUnitType.Star);
    public string ShanheBossRewardTitle => IsShanheBonusRewardOnly ? "追 加 奖 励" :
        (_shanheSession?.Snapshot.BossRewardSummary?.Title ?? "章节首领") + " · 奖 励";
    public string ShanheBossRewardNotice
    {
        get
        {
            if (_shanheSession?.Snapshot.BossRewardSummary is not { } summary) return string.Empty;
            var next = summary.IsBonusOnly ? "继续行军" : summary.ChapterNumber >= 3 ? "完成征程" : "进入下一章";
            var remaining = summary.Groups.Count(group => !group.IsResolved);
            return remaining > 0
                ? $"还有{remaining}组{(summary.IsBonusOnly ? "追加" : "")}奖励待处理，完成后{next}"
                : $"本次奖励已处理，确认后{next}";
        }
    }
    public string ShanheBossFixedGoldText => $"铜币 +{_shanheSession?.Snapshot.BossRewardSummary?.FixedGold ?? 0}";
    public string ShanheBossSkillSlotsText => $"技能槽 +{_shanheSession?.Snapshot.BossRewardSummary?.SkillSlots ?? 0}";
    public ImageSource? ShanheBossGoldArtwork => AssetImages.Load("Assets/Shanhe/Encounter/14021801.png");
    public ImageSource? ShanheBossSkillSlotsArtwork => AssetImages.Load("Assets/Shanhe/2293.png");
    public string ShanheBossRewardContinueText => IsShanheBonusRewardOnly ? "继 续 行 军" :
        _shanheSession?.Snapshot.BossRewardSummary?.ChapterNumber is >= 3
        ? "完 成 征 程" : "进 入 下 一 章";
    public ICommand OpenShanheBossRewardGroupCommand { get; private set; } = null!;
    public ICommand CloseShanheBossRewardGroupCommand { get; private set; } = null!;
    public ICommand ConfirmShanheBossRewardCommand { get; private set; } = null!;
    private IEnumerable<ShanheCampaignChoice> CurrentShanheRewardChoices => IsShanheOpeningSkillDraft ? _shanheOpeningChoices :
        HasShanheBossRewardGroup ? _shanheBossRewardChoices : ShanheChoices;
    private bool IsCurrentShanheRewardChoice(ShanheCampaignChoice choice) => choice.IsEnabled &&
        _shanheSession?.Revision == choice.Revision && CurrentShanheRewardChoices.Any(current => ReferenceEquals(current, choice));

    private void InitializeShanheBossRewards()
    {
        OpenShanheBossRewardGroupCommand = new RelayCommand<ShanheBossRewardGroup>(group =>
        {
            if (!CanOpenShanheBossRewardGroup(group)) return;
            _shanheBossRewardGroupId = group.Id;
            RefreshShanheRewards();
        }, CanOpenShanheBossRewardGroup);
        CloseShanheBossRewardGroupCommand = new RelayCommand(() =>
        {
            _shanheBossRewardGroupId = null;
            RefreshShanheRewards();
        }, () => HasShanheBossRewardGroup);
        ConfirmShanheBossRewardCommand = new RelayCommand(() =>
        {
            if (!CanConfirmShanheBossRewards()) return;
            ApplyShanheCommand(new ConfirmShanheBossRewardCommand(_shanheSession!.Revision));
        }, CanConfirmShanheBossRewards);
    }

    private bool CanOpenShanheBossRewardGroup(ShanheBossRewardGroup group) => !IsTutorialActive && !IsShanheSkillReplacementOpen && !group.IsResolved &&
        _shanheSession?.Revision == group.Revision && ShanheBossRewardGroups.Contains(group);
    private bool CanConfirmShanheBossRewards() => !IsTutorialActive && IsShanheBossRewardOpen &&
        _shanheSession?.Snapshot.BossRewardSummary?.CanConfirm == true;

    private void RefreshShanheBossRewards()
    {
        ShanheBossRewardGroups.Clear();
        _shanheBossRewardChoices.Clear();
        var state = _shanheSession?.Snapshot;
        if (state?.BossRewardSummary is { } summary)
        {
            if (ActiveShanheBossRewardGroup is null) _shanheBossRewardGroupId = null;
            foreach (var group in summary.Groups)
            {
                IReadOnlyList<ShanheRewardDefinition> visualRewards = group.SelectedReward is { } chosen ? [chosen] : group.Offers;
                ShanheBossRewardGroups.Add(new(group.Id, group.Name, group.IsSkipped ? $"已换取 {group.SkipGold} 铜币" : group.SelectedReward?.Name ?? (group.IsExhausted ? "暂无可领取奖励" : $"{group.Offers.Count} 选 一"),
                    group.IsResolved, group.Id == _shanheBossRewardGroupId, state.Revision)
                {
                    Rarity = Math.Clamp(group.SelectedReward?.Rarity ?? group.Offers.Select(reward => reward.Rarity).DefaultIfEmpty(2).Max(), 1, 4),
                    TypeIconIndex = visualRewards.Count == 0 ? 0 : visualRewards.All(IsShanheTactic) ? 2
                        : visualRewards.All(reward => reward.SkillId is not null) ? 3
                        : visualRewards.All(reward => reward.StartingCards?.Count > 0 || reward.StartingEquipment?.Count > 0) ? 4 : 0,
                    IsSkipped = group.IsSkipped,
                    IsExhausted = group.IsExhausted,
                    OfferCount = group.Offers.Count,
                    SkipGold = group.SkipGold,
                    CanSkip = group.CanSkip,
                    Artworks = group.IsSkipped ? [] : group.SelectedReward is { } selected
                        ? [CreateShanheRewardArtwork(selected)]
                        : group.Offers.Take(3).Select(reward => CreateShanheRewardArtwork(reward, isPreview: true)).ToArray()
                });
            }
            if (ActiveShanheBossRewardGroup is { } active)
                foreach (var reward in active.Offers)
                {
                    var alreadyKnown = reward.SkillId is { } skillId && state.ExtraSkillIds.Contains(skillId);
                    if (reward.SkillId is not null && !alreadyKnown && state.ExtraSkillIds.Count >= state.ExtraSkillSlots + reward.AddSkillSlots)
                        foreach (var replaced in state.ExtraSkillIds)
                            _shanheBossRewardChoices.Add(new(reward.Id, $"{reward.Name} · 替换{ShanheSkillName(replaced)}",
                                reward.Description, true, replaced, true, state.Revision));
                    else _shanheBossRewardChoices.Add(new(reward.Id, reward.Name, reward.Description, true, null, !alreadyKnown, state.Revision));
                }
        }
        else _shanheBossRewardGroupId = null;
        foreach (var property in new[] { nameof(HasShanheBossRewardGroup), nameof(IsShanheBossRewardOpen), nameof(ShanheBossRewardTitle),
                     nameof(IsShanheBonusRewardOnly), nameof(HasShanheBossFixedRewards),
                     nameof(ShanheBossFixedRewardColumnWidth), nameof(ShanheBossRewardGroupColumnWidth), nameof(ShanheBossRewardNotice),
                     nameof(ShanheBossFixedGoldText), nameof(ShanheBossSkillSlotsText), nameof(ShanheBossRewardContinueText) }) RaisePropertyChanged(property);
        (CloseShanheBossRewardGroupCommand as RelayCommand)?.NotifyCanExecuteChanged();
        (ConfirmShanheBossRewardCommand as RelayCommand)?.NotifyCanExecuteChanged();
    }
}

public sealed record ShanheBossRewardGroup(string Id, string Name, string SelectedName, bool IsResolved, bool IsSelected, long Revision)
{
    public IReadOnlyList<ShanheRewardArtwork> Artworks { get; init; } = [];
    public int Rarity { get; init; }
    public int TypeIconIndex { get; init; }
    public bool IsSkipped { get; init; }
    public bool IsExhausted { get; init; }
    public int OfferCount { get; init; }
    public int SkipGold { get; init; }
    public bool CanSkip { get; init; }
    public string DisplaySelectedName => ShanheDisplayText.RewardName(SelectedName);
    public string OverviewTitle => !IsResolved ? $"{OfferCount} 选 一" : IsSkipped ? "已折换" : IsExhausted ? "暂无奖励" : "已领取";
    public string OverviewDescription => IsResolved ? DisplaySelectedName : "点击卡片，查看完整候选";
    public string OverviewFooter => IsResolved ? "已处理" : CanSkip ? $"也可折换 {SkipGold} 铜币" : "本组必须领取";
}
