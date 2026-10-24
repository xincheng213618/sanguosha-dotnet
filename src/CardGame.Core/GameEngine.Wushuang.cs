namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string WushuangGrantSource = WushuangModeFeature.GrantSource;
    private const string WushuangOriginalReplacementSource = WushuangModeFeature.OriginalReplacementSource;
    private WushuangModeFeature? WushuangFeature => _modeRuntime.Wushuang;
    private WushuangModeFeature RequireWushuangFeature => WushuangFeature ??
        throw new InvalidOperationException("The current match has no Wushuang feature.");
    // Compatibility bridges share the optional feature's sole progress store.
    // Native skill/acquisition/AI adapters do not retain a second copy.
    private IReadOnlyDictionary<int, WushuangPlayerProgress> _wushuangPlayers => RequireWushuangFeature.Players;
    private int _wushuangElapsedMilliseconds
    {
        get => WushuangFeature?.ElapsedMilliseconds ?? 0;
        set => RequireWushuangFeature.ElapsedMilliseconds = value;
    }
    private WushuangModeFeature CreateWushuangModeFeature() =>
        new(_options.Seed, _contentRegistry, new WushuangModeHost(this));
    public bool IsWushuangMode => _modeDefinition.Id == WushuangMode.ModeId;
    private bool IsWushuangSkillGrantQualified(SkillGrant grant) => !IsWushuangMode ||
        _contentRegistry.Skills.TryGetValue(grant.SkillId, out var skill) && WushuangCatalogue.IsSkillAllowed(skill);
    public bool IsWushuangClockRunning => WushuangFeature is not null && _started && _setupComplete && _winner == Winner.None;
    public int? WushuangRemainingMilliseconds => WushuangFeature?.RemainingMilliseconds;
    private void InitializeWushuangAutomaticHealth()
    {
        if (!IsWushuangMode || _options.UseInteractiveSetup) return;
        foreach (var p in _players) { p.MaxHp = p.General.BaseHp; p.Hp = p.General.InitialHp ?? p.MaxHp; }
    }
    private WushuangIntermissionFrame? ActiveWushuangWindow(int seat) => WushuangFeature?.ActiveWushuangWindow(seat);
    private bool HasWushuangIntermissionBoundary() => WushuangFeature?.HasWushuangIntermissionBoundary() ?? false;
    public WushuangSnapshot? CreateWushuangSnapshot(int viewerSeat) => WushuangFeature?.CreateWushuangSnapshot(viewerSeat);
    private bool CanUseWushuangShop(int seat) => WushuangFeature?.CanUseWushuangShop(seat) ?? false;
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 61001, WushuangMode.ModeId)]
    private void InitializeWushuangShops(GameStartedEvent started) => WushuangFeature?.InitializeWushuangShops(started);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 61002, WushuangMode.ModeId)]
    private void ProgressWushuangEconomy(TurnStartedEvent started) => WushuangFeature?.ProgressWushuangEconomy(started);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 61003, WushuangMode.ModeId)]
    private void ProgressWushuangRound(RoundStartedEvent started) => WushuangFeature?.ProgressWushuangRound(started);
    private bool TryBeginWushuangOpening() => WushuangFeature?.TryBeginWushuangOpening() ?? false;
    private bool TryBeginWushuangShopping(CharacterState previous, bool skipped) =>
        WushuangFeature?.TryBeginWushuangShopping(previous.Seat, skipped) ?? false;
    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.BeforeOpening, 0, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(WushuangIntermissionFrame), 0)]
    private void ContinueWushuangIntermission(long frameId) => RequireWushuangFeature.ContinueWushuangIntermission(frameId);
    [ProgramAnswer("wushuang-choice")]
    private void AnswerWushuangChoice(PromptChoice choice) => RequireWushuangFeature.AnswerWushuangChoice(choice);
    private void AdvanceWushuangShopSeat(WushuangIntermissionFrame frame) => RequireWushuangFeature.AdvanceWushuangShopSeat(frame);
    private void ChangeWushuangTokens(int seat, int delta, string reason) => RequireWushuangFeature.ChangeWushuangTokens(seat, delta, reason);
    private void RefreshWushuangOffers(int seat, bool opening = false) => RequireWushuangFeature.RefreshWushuangOffers(seat, opening);
    private CommandResult SubmitBuyWushuangOffer(BuyWushuangOfferCommand command) =>
        WushuangFeature?.SubmitBuyWushuangOffer(command) ?? RejectMissingWushuangFeature();
    private string? ValidateWushuangPurchase(int seat, WushuangShopOffer? offer, string? replacement) =>
        RequireWushuangFeature.ValidateWushuangPurchase(seat, offer, replacement);
    private void BuyWushuangOffer(int seat, WushuangShopOffer offer, string? replacement, bool opening = false) =>
        RequireWushuangFeature.BuyWushuangOffer(seat, offer, replacement, opening);
    private CommandResult SubmitRerollWushuangShop(RerollWushuangShopCommand command) =>
        WushuangFeature?.SubmitRerollWushuangShop(command) ?? RejectMissingWushuangFeature();
    private CommandResult SubmitUpgradeWushuangShop(UpgradeWushuangShopCommand command) =>
        WushuangFeature?.SubmitUpgradeWushuangShop(command) ?? RejectMissingWushuangFeature();
    private CommandResult SubmitFinishWushuangShop(FinishWushuangShopCommand command) =>
        WushuangFeature?.SubmitFinishWushuangShop(command) ?? RejectMissingWushuangFeature();
    private CommandResult SubmitToggleWushuangShopLock(ToggleWushuangShopLockCommand command) =>
        WushuangFeature?.SubmitToggleWushuangShopLock(command) ?? RejectMissingWushuangFeature();
    private void CompleteWushuangShopAction(long frameId) => RequireWushuangFeature.CompleteWushuangShopAction(frameId);
    private CommandResult RejectMissingWushuangFeature() => Reject(CommandErrorCode.IllegalAction, "当前场次没有虎符商店。");

    private sealed class WushuangModeHost(GameEngine engine) : IWushuangModeHost
    {
        public int PlayerCount => engine._playerCount;
        public int CurrentSeat => engine._currentSeat;
        public int RoundNumber => engine._roundNumber;
        public int FirstNormalSeat => engine.ProgramEventHistory<MoLuBuOpeningStartedEvent>().FirstOrDefault()?.NormalSeat ??
            engine.ProgramEventHistory<RoundStartedEvent>().FirstOrDefault()?.ActorSeat ?? engine._currentSeat;
        public long Revision => engine._revision;
        public bool ClockRunning => engine.IsWushuangClockRunning;
        public bool HasWinner => engine._winner != Winner.None;
        public bool IsNormalTurn => engine._turnProgression.Kind == ActualTurnKind.Normal;
        public bool AdvanceAfterHumanCommands => engine._options.AdvanceAfterHumanCommands;
        public IEnumerable<WushuangPlayerFacts> Players => engine._players.Select(player => Player(player.Seat));
        public WushuangPlayerFacts Player(int seat)
        {
            var player = engine._players[seat];
            return new(player.Seat, player.Name, player.IsHuman, player.IsAlive, player.Hp, player.MaxHp,
                Array.AsReadOnly(player.SkillGrants.Grants.ToArray()));
        }
        public IReadOnlyList<string> EnabledSkillIds(int seat) => Array.AsReadOnly(engine.EnabledContentSkillIds(engine._players[seat]).ToArray());
        public int EquipmentSlotCapacity(int seat, EquipmentSlot slot) => engine._players[seat].EquipmentSlotCapacity(slot);
        public EquipmentSlot GetEquipmentSlot(CardKind kind) => engine.EquipmentRules.Get(kind).Slot;
        public bool IsEquipment(CardKind kind) => engine.EquipmentRules.IsEquipment(kind);
        public bool IsSlash(CardKind kind) => IsSlashCard(kind);
        public bool HasReachedFiveTokens(int seat) => engine.ProgramEventHistory<WushuangTokensChangedEvent>().Any(fact => fact.Seat == seat && fact.Tokens >= 5);
        public IReadOnlyList<ResolutionFrame> Frames => Array.AsReadOnly(engine._resolutionStack.ToArray());
        public PendingDecision? PendingDecision => engine._pendingDecision;
        public long NextFrameId() => ++engine._resolutionSequence;
        public void PushFrame(ResolutionFrame frame) => engine.PushRuntimeFrame(frame);
        public void ReplaceFrame(long id, ResolutionFrame frame) => engine.ReplaceRuntimeFrame(id, frame);
        public void PopFrame(long id, ResolutionFrameKind kind) => engine.PopResolutionFrame(id, kind);
        public void ClearPrompt() => engine.ClearPendingDecision();
        public void SetPrompt(PendingDecision prompt)
        { engine._pendingDecision = prompt; engine._status = EngineStatus.AwaitingHumanResponse; }
        public PromptId NextPromptId() => engine.CreatePromptId();
        public void AdvanceState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
        public void PublishFact(IGameEvent fact) => engine.AdvanceEventRulesAndQueueFact(fact);
        public void LogPurchase(string message, int seat) => engine.AddLog("WushuangPurchase", message, seat);
        public CommandResult Accept(Action action) => engine.Accept(action);
        public CommandResult Reject(CommandErrorCode code, string message) => engine.Reject(code, message);
        public void RemoveSkillGrant(int seat, string grantId) => engine._players[seat].SkillGrants.RemoveGrant(grantId);
        public void GrantSkill(int seat, SkillGrant grant)
        {
            var player = engine._players[seat];
            player.SkillGrants.Grant(grant);
            engine._skillBindingIndex.Invalidate(seat);
            engine.RegisterTaggedConversionSkill(player, grant.SkillId);
        }
        public void PlaceShopCard(int seat, string offerId, WushuangCard definition) => engine.PlaceWushuangShopCard(seat, offerId, definition);
        public void PlaceFallbackCard(int seat, CardKind kind) => engine.PlaceShanhePhysicalCard(
            new(kind, kind == CardKind.Dodge ? Suit.Spade : Suit.Heart, kind == CardKind.Dodge ? 2 : 7), seat, equipment: false);
        public bool TryBeginAcquisition(long frameId, WushuangShopOffer offer) => engine.TryBeginWushuangAcquisition(frameId, offer);
        public bool TryContinueAiShopping(int seat, WushuangIntermissionFrame frame) => false;
        public void ContinueAfterShopping(int endedSeat, bool skipped) => engine.ContinueAfterWushuangShopping(engine._players[endedSeat], skipped);
    }
}
