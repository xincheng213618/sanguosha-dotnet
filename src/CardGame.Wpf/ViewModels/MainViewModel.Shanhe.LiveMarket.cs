using System.Windows.Input;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private bool _isShanheMarketOpen;
    private bool _shanheMarketWasAutoAdvance;
    private RelayCommand? _openShanheMarketCommand, _closeShanheMarketCommand;
    private IReadOnlyList<ShanheCampaignChoice> _shanheLiveMarketChoices = [];

    private GameEngine? _shanheProjectionGame;
    private long _shanheProjectionRevision = -1;
    private GameSnapshot? _shanheBattleProjection;
    private ShanheCampaignSnapshot? _shanheMarketProjection;
    private bool _shanheMarketProjectionCreated;

    private void ResetShanheBattleProjection()
    {
        _shanheProjectionGame = null;
        _shanheProjectionRevision = -1;
        _shanheBattleProjection = null;
        _shanheMarketProjection = null;
        _shanheMarketProjectionCreated = false;
    }

    private GameSnapshot? CurrentShanheBattleState
    {
        get
        {
            if (!IsShanheBattleActive || _shanheBattleGame is not { IsFaulted: false } battle)
            {
                ResetShanheBattleProjection();
                return null;
            }
            if (!ReferenceEquals(_shanheProjectionGame, battle) || _shanheProjectionRevision != battle.Revision)
            {
                // These are immutable player views. UI bindings share one projection
                // per committed revision, including while a hand prompt is waiting.
                var snapshot = battle.CreateSnapshot(0);
                _shanheProjectionGame = battle;
                _shanheProjectionRevision = battle.Revision;
                _shanheBattleProjection = snapshot;
                _shanheMarketProjection = null;
                _shanheMarketProjectionCreated = false;
            }
            return _shanheBattleProjection;
        }
    }

    private ShanheCampaignSnapshot? CurrentShanheMarketState
    {
        get
        {
            if (CurrentShanheBattleState is not { Status: not EngineStatus.Completed }) return _shanheSession?.Snapshot;
            if (!_shanheMarketProjectionCreated)
            {
                _shanheMarketProjection = _shanheProjectionGame!.CreateShanheMarketSnapshot(0);
                _shanheMarketProjectionCreated = true;
            }
            return _shanheMarketProjection;
        }
    }
    private ShanheCampaignSnapshot? CurrentShanheBuildState => CurrentShanheMarketState ?? _shanheSession?.Snapshot;
    private long ShanheMarketActionRevision => IsShanheBattleActive
        ? _shanheBattleGame?.Revision ?? -1 : _shanheSession?.Revision ?? -1;
    public bool IsShanheChapterMarketUnlocked => _shanheSession?.IsChapterMarketUnlocked == true;
    public string ShanheChapterMarketHint => IsShanheChapterMarketUnlocked
        ? "本章集市已开放，可查看货品与购买补给。" : "完成前两处奇遇（主线3/6）后开放集市。";
    public bool IsShanheMarketOpen
    {
        get => _isShanheMarketOpen;
        private set
        {
            if (!SetProperty(ref _isShanheMarketOpen, value)) return;
            if (value) { _shanheMarketWasAutoAdvance = IsAutoAdvance; IsAutoAdvance = false; }
            else if (!IsShanheOpen) IsAutoAdvance = _shanheMarketWasAutoAdvance;
            _closeShanheMarketCommand?.NotifyCanExecuteChanged();
        }
    }
    public ICommand OpenShanheMarketCommand => _openShanheMarketCommand ??= new RelayCommand(() =>
    {
        if (!OpenShanheMarketCommand.CanExecute(null)) return;
        RefreshShanheModules();
        IsShanheMarketOpen = true;
    }, () => !IsTutorialActive && IsShanheRunInProgress && IsShanheChapterMarketUnlocked && !IsShanheBattleRetryReady && _shanheBattleGame?.HasPendingShanheMarketPurchase != true);
    public ICommand CloseShanheMarketCommand => _closeShanheMarketCommand ??= new RelayCommand(() =>
    {
        CancelShanheSkillReplacement();
        IsShanheMarketOpen = false;
    }, () => IsShanheMarketOpen);
    public string ShanheMarketGoldText => $"铜币  {CurrentShanheMarketState?.Gold ?? 0}";
    public string ShanheMarketHealthText => CurrentShanheMarketState is { } state ? $"体力  {state.Hp}/{state.MaxHp}" : "体力 —";
    public string ShanheMarketEquipmentText
    {
        get
        {
            var kinds = IsShanheBattleActive && _shanheBattleGame is not null
                ? (CurrentShanheBattleState?.Players[0].Equipment ?? []).Select(card => card.Kind).ToArray()
                : (_shanheSession?.Snapshot.StartingEquipment ?? []).Select(card => card.Kind).ToArray();
            string Name(EquipmentSlot slot) => kinds.Where(kind => EquipmentCatalog.Get(kind).Slot == slot)
                .Select(kind => EquipmentCatalog.Get(kind).DisplayName).LastOrDefault() ?? "无";
            return $"当前武器：{Name(EquipmentSlot.Weapon)}   当前防具：{Name(EquipmentSlot.Armor)}   当前宝物：{Name(EquipmentSlot.Treasure)}";
        }
    }
    private IEnumerable<ShanheCampaignChoice> CurrentShanheMarketChoices =>
        _shanheSession?.HasPersistentMarket == true ? _shanheLiveMarketChoices : ShanheChoices;
    private void RefreshShanheLiveMarketChoices()
    {
        _shanheLiveMarketChoices = _shanheSession?.HasPersistentMarket == true
            ? (CurrentShanheMarketState?.ShopItems ?? []).Where(product => product.CanPurchase)
                .SelectMany(product => product.ReplaceableSkillIds.Count > 0
                    ? product.ReplaceableSkillIds.Select(skill => MarketChoice(product, skill))
                    : new[] { MarketChoice(product, null) }).ToArray() : [];
    }
    private ShanheCampaignChoice MarketChoice(ShanheShopItemSnapshot product, string? replacement) =>
        new(product.ChoiceId, product.Name, product.Description, false, replacement, product.CanPurchase, ShanheMarketActionRevision);
    private bool IsCurrentShanheMarketChoice(ShanheCampaignChoice choice) =>
        _shanheSession?.HasPersistentMarket != true ? IsCurrentShanheChoice(choice) :
        choice.IsEnabled && _shanheLiveMarketChoices.Any(current => ReferenceEquals(current, choice)) &&
        choice.Revision == ShanheMarketActionRevision &&
        CurrentShanheMarketState?.ShopItems.Any(product => product.ChoiceId == choice.Id && product.CanPurchase &&
            (choice.ReplaceSkillId is null ? product.ReplaceableSkillIds.Count == 0 : product.ReplaceableSkillIds.Contains(choice.ReplaceSkillId))) == true;

    private void BuyShanheMarketChoice(ShanheCampaignChoice choice)
    {
        if (IsTutorialActive || !IsCurrentShanheMarketChoice(choice)) return;
        if (_shanheSession?.HasPersistentMarket != true) { ChooseShanheOption(choice); return; }
        if (IsShanheBattleActive)
            ApplyShanheBattleMarketCommand(new CardGame.Core.BuyShanheMarketItemCommand(0, choice.Id, choice.ReplaceSkillId, choice.Revision));
        else ApplyShanheCommand(new CardGame.Core.Campaigns.BuyShanheMarketItemCommand(choice.Id, choice.ReplaceSkillId, choice.Revision));
    }
    private void ApplyShanheBattleMarketCommand(GameCommand command)
    {
        if (_shanheBattleGame is not { } battle || !IsShanheBattleActive || IsTutorialActive) return;
        if (!ReferenceEquals(_game, battle)) ResumeShanheBattle();
        var result = SubmitCommand(command);
        if (!result.Accepted) { ShanheStatusText = $"集市交易未执行：{result.Error?.Message}"; return; }
        RefreshCommandResult(result.State);
        if (battle.HasPendingShanheMarketPurchase)
        {
            IsShanheMarketOpen = false;
            IsShanheOpen = false;
        }
        RefreshShanhePresentation();
        SaveShanheProgress();
    }
    private void RefreshShanheLiveMarketPresentation()
    {
        if (!IsShanheRunInProgress && IsShanheMarketOpen) IsShanheMarketOpen = false;
        foreach (var property in new[] { nameof(ShanheMarketGoldText), nameof(ShanheMarketHealthText), nameof(ShanheMarketEquipmentText) }) RaisePropertyChanged(property);
        _openShanheMarketCommand?.NotifyCanExecuteChanged();
        _closeShanheMarketCommand?.NotifyCanExecuteChanged();
    }
}
