namespace CardGame.Core;

public enum ModeMatchStartPhase { BeforeRunning, AfterRunning, AfterOpeningFacts }
public enum ModeCompletionBoundary { AfterCommand, AfterTurnEnded }

/// <summary>Ordered match boundaries inside the original command transaction.</summary>
public interface IMatchModeLifecycle : IGameModeFeature
{
    int Order { get; }
    void OnMatchStart(ModeMatchStartPhase phase) { }
    bool TryCompleteMatch(ModeCompletionBoundary boundary) => false;
    bool TryFinishAtTurnLimit(int maximumTurns) => false;
    bool TryHandleEmptyDrawPile() => false;
    void ApplyDeathReward(int victimSeat, int? killerSeat) { }
}

/// <summary>Only the installed ranking mode can publish a resolved death's rank.</summary>
public interface IMatchModeDeathRanking : IGameModeFeature
{
    void RecordDeathRank(long deathFrameId, int seat);
}

public interface IMatchModeNumericRules : IGameModeFeature
{
    int Order { get; }
    IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query);
    RuleQueryEvaluation AdjustNumericEvaluation(int seat, RuleQueryEvaluation ordinary, SkillRuleQuery query) => ordinary;
}

public interface IMatchModeDistanceRules : IGameModeFeature
{
    int Order { get; }
    IEnumerable<RuleQueryBaseTerm> BaseDistanceTerms(int sourceSeat, int targetSeat);
}

public interface IMatchModeEquipmentPolicy : IGameModeFeature
{
    int Order { get; }
    int? SlashLimitBonus(CardKind equipmentKind) => null;
}

/// <summary>Viewer-scoped additions run before Core freezes the completed snapshot.</summary>
public interface IMatchModeSnapshotProjector : IGameModeFeature
{
    int Order { get; }
    PlayerSnapshot ProjectPlayer(PlayerSnapshot player, int viewerSeat, bool revealAll) => player;
    GameSnapshot ProjectGame(GameSnapshot game, int viewerSeat, bool revealAll) => game;
}

public interface IMatchModeSkillVisibility : IGameModeFeature
{
    bool IsSkillVisible(int seat, string? skillId, int viewerSeat, bool revealAll);
}

public sealed record ModeCardActionFacts(long ActionId, CardActionType Type, int ActorSeat, CardKind EffectiveKind);

public interface IMatchModeCardActionObserver : IGameModeFeature
{
    int Order { get; }
    void OnCardAction(ModeCardActionFacts action, SkillProgramTriggerWindow window);
}

public interface IMatchModeInitialDeal : IGameModeFeature
{
    int Order { get; }
    int? InitialHandSizeOverride(int seat) => null;
    void BeforeInitialDeal() { }
    bool TryDealInitialCard(int seat, int cardIndex) => false;
    void AfterInitialDeal() { }
}
