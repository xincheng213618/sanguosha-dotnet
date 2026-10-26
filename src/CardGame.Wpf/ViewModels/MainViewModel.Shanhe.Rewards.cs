using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private ShanheRewardCard? _selectedShanheRewardCard;
    private ShanheCampaignSession? _shanheRewardOwner;
    private readonly List<ShanheCampaignChoice> _shanheOpeningChoices = [];
    private bool IsShanheOpeningSkillDraft => _shanheSession?.Snapshot.Phase == ShanheCampaignPhase.OpeningSkillDraft;
    private int CurrentShanheRewardSkipGold => ActiveShanheBossRewardGroup is { } group
        ? group.CanSkip ? group.SkipGold : 0 : _shanheSession?.Snapshot.RewardSkipGold ?? 0;

    public ObservableCollection<ShanheRewardCard> ShanheRewardCards { get; } = [];
    public ObservableCollection<ShanheCampaignChoice> SelectedShanheRewardReplacements { get; } = [];
    public bool IsShanheRewardOpen => IsShanheOpeningSkillDraft || _shanheSession?.Snapshot.Phase == ShanheCampaignPhase.Reward &&
        (_shanheSession.Snapshot.BossRewardSummary is null || HasShanheBossRewardGroup);
    public string ShanheRewardHeading => IsShanheOpeningSkillDraft ? "传 承 · 技 能 选 择" :
        _shanheSession?.Snapshot.RewardCategories.Count > 0 ? "奖 励 类 别" :
        ActiveShanheBossRewardGroup is { } group ? IsShanheBonusRewardOnly ? "追 加 奖 励 · 选 择" : group.Id switch
        { "tactics" => "战 法 选 择", "cards" => "卡 牌 选 择", _ => group.Name + " · 选择" } :
        _shanheSession?.Snapshot.RewardOffers is { Count: > 0 } offers && offers.All(reward => reward.SkillId is not null)
            ? "技 能 选 择" : "战 利 品 选 择";
    public string ShanheRewardNotice => IsShanheOpeningSkillDraft ? "传承：从3项稀有技能中选择1项" :
        _shanheSession?.Snapshot.RewardCategories.Count > 0 ? "选择奖励类别，确认后不可更换。" :
        ActiveShanheBossRewardGroup is { } group ? IsShanheBonusRewardOnly
            ? $"从 {group.Offers.Count} 项追加奖励中选择1项" + (group.CanSkip ? $"，或放弃换取 {group.SkipGold} 铜币" : group.Offers.All(IsShanheTactic) ? "，战法必须领取" : "")
            : $"本组 {group.Offers.Count} 项奖励，选择1项" + (group.CanSkip ? $"，或放弃换取 {group.SkipGold} 铜币" : group.Offers.All(IsShanheTactic) ? "，战法必须领取" : "") :
        _shanheSession?.Snapshot is { CanSkipReward: true } state
        ? $"选择1项奖励，或放弃换取 {state.RewardSkipGold} 铜币"
        : _shanheSession?.Snapshot.RewardOffers is { Count: > 0 } mandatory && mandatory.All(IsShanheTactic)
            ? "选择1项战法，必须领取" : "选择1项奖励";
    public ShanheRewardCard? SelectedShanheRewardCard => _selectedShanheRewardCard;
    public bool ShanheRewardNeedsReplacement => SelectedShanheRewardReplacements.Count > 0;
    public string ShanheRewardConfirmText => _selectedShanheRewardCard?.IsSkip == true ? "领 取 铜 币" :
        _shanheSession?.Snapshot.RewardCategories.Count > 0 ? "确 认 类 别" :
        HasShanheBossRewardGroup && IsShanheBonusRewardOnly ? "领 取 追 加 奖 励" : "领 取 奖 励";

    public ICommand SelectShanheRewardCardCommand { get; private set; } = null!;
    public ICommand ConfirmShanheRewardCommand { get; private set; } = null!;

    private void InitializeShanheRewards()
    {
        SelectShanheRewardCardCommand = new RelayCommand<ShanheRewardCard>(SelectShanheRewardCard, IsCurrentShanheRewardCard);
        ConfirmShanheRewardCommand = new RelayCommand(ConfirmShanheReward, CanConfirmShanheReward);
        InitializeShanheBossRewards();
        InitializeShanheSkillReplacement();
    }

    private bool IsCurrentShanheRewardCard(ShanheRewardCard card) => !IsTutorialActive && !IsShanheSkillReplacementOpen && IsShanheRewardOpen &&
        card.IsEnabled && _shanheSession?.Revision == card.Revision && ShanheRewardCards.Contains(card);

    private void SelectShanheRewardCard(ShanheRewardCard card)
    {
        if (!IsCurrentShanheRewardCard(card)) return;
        foreach (var item in ShanheRewardCards) item.IsSelected = ReferenceEquals(item, card);
        _selectedShanheRewardCard = card;
        SelectedShanheRewardReplacements.Clear();
        foreach (var choice in CurrentShanheRewardChoices.Where(choice => choice.IsReward && choice.Id == card.Id &&
                     choice.ReplaceSkillId is not null && choice.IsEnabled))
            SelectedShanheRewardReplacements.Add(choice);
        RaisePropertyChanged(nameof(SelectedShanheRewardCard));
        RaisePropertyChanged(nameof(ShanheRewardNeedsReplacement));
        RaisePropertyChanged(nameof(ShanheRewardConfirmText));
        (ConfirmShanheRewardCommand as RelayCommand)?.NotifyCanExecuteChanged();
    }

    private bool CanConfirmShanheReward()
    {
        if (_selectedShanheRewardCard is not { } card || !IsCurrentShanheRewardCard(card)) return false;
        if (card.IsSkip) return CurrentShanheRewardSkipGold > 0;
        if (card.RewardCategoryChoice is { } category)
            return _shanheSession!.Snapshot.RewardCategories.Contains(category);
        return ShanheRewardNeedsReplacement
            ? SelectedShanheRewardReplacements.Any(IsCurrentShanheRewardChoice)
            : CurrentShanheRewardChoices.Any(choice => choice.IsReward && choice.Id == card.Id &&
                choice.ReplaceSkillId is null && IsCurrentShanheRewardChoice(choice));
    }

    private void ConfirmShanheReward()
    {
        if (!CanConfirmShanheReward() || _selectedShanheRewardCard is not { } card) return;
        if (ShanheRewardNeedsReplacement)
        {
            OpenShanheSkillReplacement(card.Name, SelectedShanheRewardReplacements.ToArray(),
                choice => CompleteShanheRewardSelection(card, choice), IsCurrentShanheRewardChoice);
            return;
        }
        CompleteShanheRewardSelection(card, null);
    }

    private void CompleteShanheRewardSelection(ShanheRewardCard card, ShanheCampaignChoice? replacement)
    {
        if (!IsCurrentShanheRewardCard(card) || replacement is not null && !IsCurrentShanheRewardChoice(replacement)) return;
        if (IsShanheOpeningSkillDraft)
            ApplyShanheCommand(new ChooseShanheOpeningSkillCommand(card.Id, card.Revision));
        else if (card.RewardCategoryChoice is { } category)
            ApplyShanheCommand(new ChooseShanheRewardCategoryCommand(category, card.Revision));
        else if (card.IsSkip) ApplyShanheCommand(new SkipShanheRewardCommand(card.Revision)
            { GroupId = ActiveShanheBossRewardGroup?.Id });
        else if (ActiveShanheBossRewardGroup is { } group)
            ApplyShanheCommand(new ChooseShanheBossRewardCommand(group.Id, card.Id,
                replacement?.ReplaceSkillId, card.Revision));
        else ChooseShanheOption(replacement ??
            ShanheChoices.Single(choice => choice.IsReward && choice.Id == card.Id && choice.ReplaceSkillId is null));
    }

    private void RefreshShanheRewards()
    {
        if (!ReferenceEquals(_shanheRewardOwner, _shanheSession))
        {
            _shanheRewardOwner = _shanheSession;
            _selectedShanheRewardCard = null;
            _shanheBossRewardGroupId = null;
        }
        var previous = _selectedShanheRewardCard;
        RefreshShanheBossRewards();
        _shanheOpeningChoices.Clear();
        ShanheRewardCards.Clear();
        SelectedShanheRewardReplacements.Clear();
        _selectedShanheRewardCard = null;
        if (_shanheSession?.Snapshot is { } state && (state.Phase == ShanheCampaignPhase.Reward || IsShanheOpeningSkillDraft))
        {
            if (IsShanheOpeningSkillDraft)
                foreach (var reward in state.OpeningSkillOffers)
                    _shanheOpeningChoices.Add(new(reward.Id, reward.Name, reward.Description, true, null, true, state.Revision));
            foreach (var category in state.RewardCategories)
            {
                var name = category switch
                {
                    ShanheRewardCategory.Tactic => "战法奖励",
                    ShanheRewardCategory.Card => "卡牌奖励",
                    ShanheRewardCategory.Skill => "技能奖励",
                    _ => "补给奖励"
                };
                ShanheRewardCards.Add(new("reward-category:" + category, name,
                    "选择此类别后，从本轮已确定的候选中领取一项。", "类别确认后不能更换。", "奖励类别", false,
                    "", state.Revision, true) { RewardCategoryChoice = category });
            }
            foreach (var reward in IsShanheOpeningSkillDraft ? state.OpeningSkillOffers : ActiveShanheBossRewardGroup?.Offers ?? state.RewardOffers)
            {
                var rule = RewardRule(reward);
                var showSkillRule = reward.SkillId is not null && !string.IsNullOrWhiteSpace(rule);
                ShanheRewardCards.Add(new(reward.Id, reward.Name.Replace("传习·", ""), showSkillRule ? rule : reward.Description,
                    showSkillRule ? string.Empty : rule, RewardCategory(reward), false, "", state.Revision,
                    CurrentShanheRewardChoices.Any(choice => choice.IsReward && choice.Id == reward.Id && choice.IsEnabled))
                { Rarity = reward.Rarity, CardImage = (reward.StartingCards ?? []).Concat(reward.StartingEquipment ?? []).FirstOrDefault() is { } card ? CardArt.Get(card.Kind) : null,
                  CardRuleDescription = (reward.StartingCards ?? []).Concat(reward.StartingEquipment ?? []).FirstOrDefault() is { } ruleCard
                      ? CardCatalog.Get(ruleCard.Kind).Description : string.Empty });
            }
            if (CurrentShanheRewardSkipGold > 0)
            {
                var offers = ActiveShanheBossRewardGroup?.Offers ?? state.RewardOffers;
                var categories = offers.Select(RewardCategory).Distinct().ToArray();
                var rarities = offers.Select(reward => reward.Rarity).Distinct().ToArray();
                var quality = rarities.Length == 1 ? rarities[0] switch { 1 => "普通", 2 => "稀有", 3 => "史诗", _ => "" } : "";
                var label = categories.Length == 1 ? quality + categories[0] : "本次奖励";
                ShanheRewardCards.Add(new("reward:skip", "放弃 · " + label, "放弃" + label + "，将这份收获折为行军盘缠。",
                    "已获得的通关铜币照常保留。", label + "折换铜币", true, $"+ {CurrentShanheRewardSkipGold} 铜币", state.Revision, true));
            }
            if (previous?.Revision == state.Revision && ShanheRewardCards.FirstOrDefault(card => card.Id == previous.Id) is { } restored)
            {
                SelectShanheRewardCard(restored);
            }
        }
        foreach (var property in new[] { nameof(IsShanheRewardOpen), nameof(ShanheRewardHeading), nameof(ShanheRewardNotice),
                     nameof(ShanheRewardConfirmText),
                     nameof(SelectedShanheRewardCard), nameof(ShanheRewardNeedsReplacement) }) RaisePropertyChanged(property);
        (ConfirmShanheRewardCommand as RelayCommand)?.NotifyCanExecuteChanged();
    }
}

public sealed class ShanheRewardCard(string id, string name, string description, string ruleDescription,
    string category, bool isSkip, string bonusText, long revision, bool isEnabled) : ObservableObject
{
    private bool _isSelected;
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public string RuleDescription { get; } = ruleDescription;
    public bool HasRuleDescription => !string.IsNullOrWhiteSpace(RuleDescription) && RuleDescription != Description;
    public string Category { get; } = category;
    public string CategoryText => Category;
    public int Rarity { get; init; }
    public string QualityText => Rarity switch { 1 => "普通", 2 => "稀有", 3 => "史诗", 4 => "传说", _ => string.Empty };
    public string HeadingText => string.IsNullOrEmpty(QualityText) ? Category : QualityText + " · " + Category;
    public ImageSource? CardImage { get; init; }
    public string CardRuleDescription { get; init; } = string.Empty;
    public ShanheRewardCategory? RewardCategoryChoice { get; init; }
    public bool HasCardImage => CardImage is not null;
    public bool IsSkip { get; } = isSkip;
    public string BonusText { get; } = bonusText;
    public long Revision { get; } = revision;
    public bool IsEnabled { get; } = isEnabled;
    public bool IsSelected { get => _isSelected; internal set => SetProperty(ref _isSelected, value); }
    public string AccessibleText => $"{Name}，{HeadingText}，{Summary}，{BonusText}";
    public string Summary => IsSkip ? "不领取本次战利品" : HasCardImage ? "获得卡牌" : RewardCategoryChoice is not null
        ? "从本类候选中选择1项" : ShanheDisplayText.Effect(ShanheDisplayText.Rule(Description, RuleDescription));
    public string RuleTooltip => !string.IsNullOrWhiteSpace(CardRuleDescription) ? CardRuleDescription :
        ShanheDisplayText.Rule(Description, RuleDescription);
}
