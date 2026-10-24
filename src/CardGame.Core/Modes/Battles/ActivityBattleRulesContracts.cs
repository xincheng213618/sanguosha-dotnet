namespace CardGame.Core;

/// <summary>Trusted public quantities; hidden card identities and mutable characters stay with the native owner.</summary>
public sealed record ModeBattlePlayerFacts(int Seat, string Name, bool IsAlive, int Hp, int MaxHp,
    string? TeamId, int HandCount, int EquipmentCount);

public sealed record ModeBattleRulesFacts(bool SetupComplete, bool StartingProgramsResolved, bool HasWinner,
    int HumanSeat, int CurrentSeat, int TurnNumber, int RoundNumber, TurnPhase Phase, int ResolutionCount,
    DecisionKind? PendingDecisionKind, int PendingMovementBatches, int PendingHpChanges, int DrawPileCount,
    bool IsExtraDrawPhase, ActualDiscardRecoveryPhaseKey? ActualPhase, IReadOnlyList<ModeBattlePlayerFacts> Players)
{
    public IReadOnlyList<ModeBattlePlayerFacts> Players { get; } =
        Array.AsReadOnly((Players ?? throw new ArgumentNullException(nameof(Players))).ToArray());
}

public sealed record ModeBattleCardAction(long ActionId, CardActionType Type, int ActorSeat, CardKind Kind);
public sealed record ModeBattleNativeUse(long FrameId, ModeBattleCardAction Action, bool HasExtraEffect);
public sealed record ModeBattleDraftFacts(int Seat, bool GeneralSelected, string GeneralId, IReadOnlyList<string> Candidates)
{
    public IReadOnlyList<string> Candidates { get; } = Array.AsReadOnly(Candidates.ToArray());
}
public sealed record ModeActivityDamageFacts(long ResolutionId, int SourceSeat, int TargetSeat, int CardUserSeat,
    bool IsSourceLess, bool IsChainPropagation, CardKind? EffectiveCardKind);
public sealed record ModeWenhePoisonMovementFacts(CardKind PrintedKind, CardMovementDisclosure Disclosure,
    CardZoneKind FromZone, int? FromOwnerSeat, CardZoneKind ToZone, int? ToOwnerSeat, bool OwnerIsAlive);

/// <summary>Match-local primitive services. Rule choices are made by the installed activity feature.</summary>
public interface IModeBattleRulesHost
{
    ModeBattleRulesFacts CaptureFacts();
    IReadOnlyList<TEvent> History<TEvent>() where TEvent : IGameEvent;
    GuanduBattleConfiguredEvent? InitialGuanduConfiguration { get; }
    ModeBattleNativeUse? FindUse(long frameId);
    ModeBattleNativeUse? FindUseByAction(long actionId);
    ModeBattleDraftFacts Draft(int seat);
    bool HasActivitySkill(int seat, string skillId);
    bool HasIndependentSkillGrant(int seat, string skillId);
    bool IsSlash(CardKind kind);
    string FactionName(string factionId);
    IEnumerable<RuleQueryContribution> NativeGeneralContributions(int seat, SkillRuleQuery query);
    int NextRandom(int exclusiveMaximum);
    void PublishFact(IGameEvent fact);
    void PublishState();
    void ClearDecision();
    void SetWinner(Winner winner, string? teamId = null, int? seat = null);
    Winner WinnerForTeam(string teamId);
    void CompleteGame();
    void TransferAllHandCards(int fromSeat, int toSeat, string reason);
    void DiscardAllHandAndEquipment(int seat, string reason);
    void Log(string kind, string message, int sourceSeat, int? targetSeat = null);
}

public sealed record ModeWenheTriggerFacts(string SkillId, string BindingId, int OwnerSeat,
    SkillProgramTriggerWindow? Window, int? SourceSeat, int? TargetSeat);
public sealed record ModeGuanduTriggerFacts(string SkillId, string BindingId, int OwnerSeat,
    bool HasContext, int Amount, bool HasTenthUse, bool HasDamageRewardUse);

public sealed record ModeWenheFlowPlan(int Amount, IReadOnlyList<int> Seats, bool BeginsChaos = false)
{
    public IReadOnlyList<int> Seats { get; } = Array.AsReadOnly(Seats.ToArray());
}
public sealed record ModeWenheFlowCursor(long FrameId, int OwnerSeat, WenheRuleFlowReceipt Receipt);

/// <summary>Exact owning-frame operations; pending state never leaves its original program receipt.</summary>
public interface IWenheRuleFlowHost
{
    ModeWenheFlowCursor? BeginContinuation(long frameId);
    ModeWenheFlowCursor Current(long frameId);
    void AdvanceCursor(long frameId, int expectedIndex, int expectedStage, int nextIndex, int nextStage);
    void ClearFlow(long frameId);
    bool DrainChildren(long frameId);
    bool DrainHpChildren(long frameId);
    void IncreaseMaximumHp(long frameId, int amount);
    bool LoseHp(long frameId, int seat, int amount);
    void BeginDraw(long frameId, int seat, int amount);
    void EquipRandomAvailable(long frameId, int seat);
}

public sealed record ModeGuanduFlowCursor(long FrameId, int OwnerSeat, GuanduEventFlowReceipt Receipt, bool HasNativeDraw);
public interface IGuanduEventFlowHost
{
    long? DamageRewardUse(long frameId);
    void IssueRepeat(long frameId);
    void IssueFlow(long frameId, SkillProgramEffectOp operation, long? useFrameId);
    void BeginRewardDraw(long frameId, int ownerSeat);
    bool LoseOwnerHp(long frameId);
    ModeGuanduFlowCursor? Current(long frameId);
    bool DrainChildren(long frameId);
    void ClearFlow(long frameId);
}

public interface IWenheBattleRulesFeature : IGameModeFeature
{
    IReadOnlyList<CardKind> OrdinaryTrickKinds { get; }
    WenheRuleKind? CurrentRule { get; }
    WenheRoundRuleSnapshot? RoundRuleSnapshot { get; }
    WenheAspirationSnapshot? Aspiration(int viewerSeat);
    int? SurvivalRank(int seat);
    bool ChampionMarkUsed(int seat);
    int GoalRank(int seat);
    int SweepDamageBonus(bool isChainPropagation);
    int? PoisonRound { get; }
    bool IsPhysicalPoison(CardKind kind, int cardId, bool alive, bool inOwnerHand);
    bool AllowsPoisonDeparture(ModeWenhePoisonMovementFacts movement);
    int PoisonHpLoss { get; }
    bool AllowsActivation(string skillId, int ownerSeat);
    bool AllowsTrigger(ModeWenheTriggerFacts facts);
    bool HasDeadOwnerEnding(int actualOwner);
    bool IsDeadOwnerEndingCandidate(string skillId, string bindingId, int candidateOwner, int actualOwner);
    ModeWenheFlowPlan IssueFlow(long frameId, SkillProgramEffectOp operation, int ownerSeat);
    bool ResumeFlow(long frameId);
    int DrawCount(SkillProgramEffectOp operation, int amount);
}

public interface IDragonBoatBattleRulesFeature : IGameModeFeature
{
    int TeamScore(string teamId);
    IReadOnlyList<DragonBoatTeamSnapshot>? Standings(bool finalizing = false);
    DragonBoatDraftSnapshot? Draft(int viewerSeat);
    int? EquipmentSlashLimitBonus(CardKind kind);
}

public interface IGuanduBattleRulesFeature : IGameModeFeature
{
    GuanduBattleConfiguredEvent? Configuration { get; }
    GuanduBattleEvent? CurrentEvent { get; }
    GuanduActivitySnapshot? Snapshot { get; }
    bool IsCommander(int seat);
    bool Allies(int first, int second);
    void RecordDeckExhaustion();
    void RecordCardAction(ModeBattleCardAction action, SkillProgramTriggerWindow window);
    DamageNature TransformDamageNature(DamageNature nature);
    int DamageBonus(ModeActivityDamageFacts damage);
    int IntoxicationDamageBonus(int targetSeat);
    bool CanUseRoundConversion(string skillId, string bindingId, int ownerSeat);
    void ConsumeRoundConversion(CardConversionSource source);
    bool IsTenthAction(long actionId);
    bool DamageRewardAvailable(long useFrameId);
    bool AllowsEvent(ModeGuanduTriggerFacts facts);
    SkillProgramStepOutcome ExecuteEvent(long frameId, int ownerSeat, SkillProgramEffectOp operation);
    bool ResumeEvent(long frameId);
}
