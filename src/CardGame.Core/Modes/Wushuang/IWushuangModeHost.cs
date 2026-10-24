namespace CardGame.Core;

/// <summary>Immutable native facts consumed by the optional single-match feature.</summary>
public sealed record WushuangPlayerFacts(int Seat, string Name, bool IsHuman, bool IsAlive,
    int Hp, int MaxHp, IReadOnlyList<SkillGrant> SkillGrants);

public sealed record WushuangDamageFacts(int SourceSeat, int CardUserSeat,
    bool IsSourceLess, bool IsChainPropagation, CardKind? EffectiveCardKind);

/// <summary>
/// The shop owns its economy and flow; native payments and continuations remain
/// inside the engine's existing command session and resolution stack.
/// </summary>
public interface IWushuangModeHost
{
    int PlayerCount { get; }
    int CurrentSeat { get; }
    int RoundNumber { get; }
    int FirstNormalSeat { get; }
    long Revision { get; }
    bool ClockRunning { get; }
    bool HasWinner { get; }
    bool IsNormalTurn { get; }
    bool AdvanceAfterHumanCommands { get; }
    IEnumerable<WushuangPlayerFacts> Players { get; }
    WushuangPlayerFacts Player(int seat);
    IReadOnlyList<string> EnabledSkillIds(int seat);
    int EquipmentSlotCapacity(int seat, EquipmentSlot slot);
    EquipmentSlot GetEquipmentSlot(CardKind kind);
    bool IsEquipment(CardKind kind);
    bool IsSlash(CardKind kind);
    bool HasReachedFiveTokens(int seat);
    IWushuangShopRandom CreateShopRandom(int seed);
    IReadOnlyList<ResolutionFrame> Frames { get; }
    PendingDecision? PendingDecision { get; }
    long NextFrameId();
    void PushFrame(ResolutionFrame frame);
    void ReplaceFrame(long id, ResolutionFrame frame);
    void PopFrame(long id, ResolutionFrameKind kind);
    void ClearPrompt();
    void SetPrompt(PendingDecision prompt);
    PromptId NextPromptId();
    void AdvanceState();
    void AdvanceToHumanBoundary();
    void PublishFact(IGameEvent fact);
    void LogPurchase(string message, int seat);
    CommandResult Accept(Action action);
    CommandResult Reject(CommandErrorCode code, string message);
    void RemoveSkillGrant(int seat, string grantId);
    void GrantSkill(int seat, SkillGrant grant);
    void PlaceShopCard(int seat, string offerId, WushuangCard definition);
    void PlaceFallbackCard(int seat, CardKind kind);
    bool TryBeginAcquisition(long frameId, WushuangShopOffer offer);
    bool TryContinueAiShopping(int seat, WushuangIntermissionFrame frame);
    void ContinueAfterShopping(int endedSeat, bool skipped);
}

/// <summary>Shop-local deterministic shuffle; one instance serves the complete page refresh.</summary>
public interface IWushuangShopRandom
{
    void Shuffle<T>(IList<T> values);
}
