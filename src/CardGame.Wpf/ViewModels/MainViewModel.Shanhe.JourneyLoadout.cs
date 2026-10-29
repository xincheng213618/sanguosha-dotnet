using CardGame.Content.Standard;
using CardGame.Core;
using System.Windows.Media;
using CardGame.Wpf.Resources;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    public bool HasShanheJourneyGeneralPortrait => CurrentShanheGeneralPortrait is System.Windows.Media.ImageBrush { ImageSource: not null };
    public string ShanheJourneyGeneralGlyph => CurrentShanheGeneralName[..Math.Min(2, CurrentShanheGeneralName.Length)];
    public ImageSource? ShanheJourneyHeroFrame
    {
        get
        {
            var rank = _shanheSession?.Snapshot.PlayerRank ?? SelectedShanheRank.Value;
            return rank >= 4 ? AssetImages.Load($"Assets/Shanhe/Hud/other_general_border{rank - 1}.png") : null;
        }
    }
    public IReadOnlyList<ShanheJourneyTacticSlot> ShanheJourneyTacticSlots
    {
        get
        {
            var tactics = ShanheJourneyTactics;
            var count = tactics.Count == 0 ? 0 : 3 * ((tactics.Count + 2) / 3 + (tactics.Count <= 12 ? 1 : 0));
            return tactics.Select(tactic => new ShanheJourneyTacticSlot(tactic))
                .Concat(Enumerable.Range(tactics.Count, count - tactics.Count).Select(_ => new ShanheJourneyTacticSlot(null))).ToArray();
        }
    }
    public double ShanheJourneyTacticDockWidth
    {
        get
        {
            var count = ShanheJourneyTacticSlots.Count;
            return count == 0 ? 0 : Math.Min(170, (count + 2) / 3 * 32 + 10);
        }
    }
    public IReadOnlyList<CardViewModel> ShanheJourneyHandCards => (CurrentShanheBuildState?.StartingCards ?? [])
        .Select((card, index) => new CardViewModel
        {
            Id = index, Kind = card.Kind, Name = CardCatalog.Get(card.Kind).DisplayName,
            KindLabel = "随身手牌", SuitGlyph = GetSuitGlyph(card.Suit),
            Rank = card.Rank switch { 1 => "A", 11 => "J", 12 => "Q", 13 => "K", _ => card.Rank.ToString(System.Globalization.CultureInfo.InvariantCulture) },
            Description = CardCatalog.Get(card.Kind).Description, IsPlayable = false
        }).ToArray();

    public IReadOnlyList<WushuangTacticViewModel> ShanheJourneyTactics => _shanheSession is not { } session || CurrentShanheBuildState is not { } state
        ? [] : state.TacticIds.Select(id => session.Definition.Rewards.Single(reward => reward.Id == id)).Select(reward =>
        {
            StandardShanheCampaignCatalog.ShopMetadataByRewardId.TryGetValue(reward.Id, out var metadata);
            var entry = metadata is null ? ShanheCodexTree.Entries.FirstOrDefault(item => ShanheCodexTree.RuntimeRewardId(item.Id) == reward.Id) : null;
            var rule = RewardRule(reward);
            return WushuangTacticViewModel.FromContent(metadata?.SourceRewardId ?? entry?.RewardId ?? 0,
                reward.Name, string.IsNullOrWhiteSpace(rule) ? reward.Description : rule, reward.Rarity > 0 ? reward.Rarity : 1,
                reward.TacticSkillId ?? reward.CampaignEconomy?.BattleSkillId);
        }).ToArray();

    public IReadOnlyList<ShanheJourneySkillSlot> ShanheJourneySkillSlots
    {
        get
        {
            if (_shanheSession is not { } session || CurrentShanheBuildState is not { } state) return [];
            var registry = StandardShanheCampaignCatalog.PlayerRosterRegistry;
            if (!registry.Generals.TryGetValue(session.Snapshot.PlayerGeneralId, out var general))
            {
                registry = CurrentPresentationRegistry;
                general = registry.Generals.GetValueOrDefault(session.Snapshot.PlayerGeneralId);
            }
            ShanheJourneySkillSlot Slot(string id, bool printed)
            {
                var skill = registry.Skills.GetValueOrDefault(id) ?? CurrentPresentationRegistry.Skills.GetValueOrDefault(id);
                var reward = session.Definition.Rewards.FirstOrDefault(item => item.SkillId == id);
                return new(skill?.Name ?? reward?.Name.Replace("传习·", "", StringComparison.Ordinal) ?? id,
                    skill is null ? reward?.Description ?? string.Empty : GetVisibleSkillDescription(skill), false, printed);
            }
            return (general?.SkillIds ?? []).Select(id => Slot(id, true))
                .Concat(state.ExtraSkillIds.Select(id => Slot(id, false)))
                .Concat(Enumerable.Range(0, Math.Max(0, state.ExtraSkillSlots - state.ExtraSkillIds.Count))
                    .Select(_ => new ShanheJourneySkillSlot("空技能槽", "可在征程中学习额外技能。", true, false))).ToArray();
        }
    }

    private void NotifyShanheJourneyLoadout()
    {
        RaisePropertyChanged(nameof(ShanheJourneyHandCards));
        RaisePropertyChanged(nameof(ShanheJourneyTactics));
        RaisePropertyChanged(nameof(ShanheJourneyTacticSlots));
        RaisePropertyChanged(nameof(ShanheJourneyTacticDockWidth));
        RaisePropertyChanged(nameof(ShanheJourneyHeroFrame));
        RaisePropertyChanged(nameof(ShanheJourneySkillSlots));
        RaisePropertyChanged(nameof(HasShanheJourneyGeneralPortrait));
        RaisePropertyChanged(nameof(ShanheJourneyGeneralGlyph));
    }
}

public sealed record ShanheJourneyTacticSlot(WushuangTacticViewModel? Tactic)
{
    public bool IsEmpty => Tactic is null;
    public string Name => Tactic?.Name ?? "空战法位";
    public string Tooltip => Tactic?.Tooltip ?? "获得更多战法后会继续显示，不占额外技能槽。";
}

public sealed record ShanheJourneySkillSlot(string Name, string Description, bool IsEmpty, bool IsPrinted)
{
    public string SourceText => IsEmpty ? "额外技能槽" : IsPrinted ? "自带技能" : "已学技能";
}
