namespace CardGame.Core;

// Selection/payment facts are read on demand. No player or zone store crosses
// this boundary, and modules never retain a suspended payment outside its frame.
internal interface IProgramPaidCardFacts : IProgramSelectionQuery
{
    int PlayerCount { get; }
    IReadOnlyList<int> Seats { get; }
    int Seat(int seat);
    bool IsValidSeat(int seat);
    bool IsAlive(int seat);
    bool IsHuman(int seat);
    string Name(int seat);
    bool EquipmentAreaAbolished(int seat);
    bool JudgmentAreaAbolished(int seat);
    int EquipmentSlotCapacity(int seat, EquipmentSlot slot);
    EquipmentSlot FieldEquipmentSlot(Card card);
    bool HasSkillInstance(int seat, string skillId, string instanceId);
    ContentSkillDefinition Skill(string skillId);
    PlayerSkillContext SkillContext(int seat);
    bool CanContinueAfterOwnerDeath(ProgramSkillFrame frame);
    bool CanContinueAfterSkillLoss(ProgramSkillFrame frame);
    bool? EvaluateOptionExpression(ProgramSkillFrame frame, ProgramExpression expression);
    IReadOnlyList<Card> Hand(int seat);
    IReadOnlyList<Card> Equipment(int seat);
    IReadOnlyList<Card> Judgment(int seat);
    IReadOnlyList<Card> CardsAt(CardLocation location);
    CardLocation Location(int cardId);
    int ProviderSeat(ProgramSkillFrame frame);
    SkillProgramEffect OwnedSelectionEffect(ProgramSkillFrame frame);
    bool IsSatisfiedOwnHandReward(ProgramSkillFrame frame, SkillProgramEffect effect);
    bool ForeignDiscardPrevented(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent);
    bool SelfDiscardForbidden(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent);
    bool AllowsOwnedCardPayment(ProgramSkillFrame frame, CardLocation source, Card card);
    bool IsActiveSourceEquipment(int owner, string skillId, string instanceId, Card card);
    bool IsZixiJudgment(int cardId);
    bool CanTransferZixiJudgment(Card card, int destinationSeat);
    bool HasCardPolicy(int seat, SkillProgramCardPolicyKind policy);
    CardKind JudgmentKind(Card card);
    bool IsDelayedCard(CardKind kind);
    bool HasJudgmentEffectiveCard(int seat, CardKind kind);
    Suit EffectiveSuit(int seat, Card card);
    int CountLivingInAttackRange(int seat);
    IReadOnlyList<int> TargetSeats(int owner, SkillProgramTargetKind kind, ProgramSkillWindowContext? context);
    bool CanSelectPindianOpponent(ProgramSkillFrame frame, int seat);
    Card[] ClaimableDamageCards(ProgramSkillFrame frame);
    bool FrozenSuitMatchesChoice(ProgramSkillFrame frame, string cardBind, string choiceBind);
    bool BoundCardsMatchCategories(ProgramSkillFrame frame, string bind, IReadOnlyList<SkillProgramCardCategory> categories);
}

internal interface IProgramPaidCardOperations : IProgramRuntimeOperations
{
    void Cancel(ProgramSkillFrame frame, string reason);
    void SetCardSet(long frameId, string bind, IReadOnlyList<int> ids, SkillProgramCardSetVisibility visibility,
        IReadOnlyList<CardLocation>? locations = null, Suit? frozenSuit = null);
    void Move(Card card, CardLocation source, CardLocation destination, CardMoveReason reason,
        CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified);
    bool MoveToProcessing(ProgramSkillFrame frame, Card card, CardLocation source, CardMoveReason reason,
        CardMovementDisclosure disclosure);
    bool SettleRetainedGain(ProgramSkillFrame frame, Card card, CardLocation source,
        CardLocation destination, CardMoveReason reason);
    void Give(ProgramSkillFrame frame, int giver, Card card, CardLocation source, CardLocation destination, CardMoveReason reason, bool throughProcessing, CardMovementDisclosure disclosure);
    bool BeginMovementWindow();
    bool BeginPublicHandReveal(long frameId, string returnKey);
    void ReturnMovement(long frameId);
    void Log(string type, string message, int? actor = null);
}

[ProgramRuntimeModule]
internal sealed partial class ProgramPaidCardRuntime
{
    private readonly IProgramPaidCardFacts _facts;
    private readonly IProgramPaidCardOperations _operations;
    private readonly IProgramRuntimeFrames _frames;
    private readonly IProgramRuntimePrompts _prompts;
    private readonly IProgramRuntimeEvents _events;
    private readonly IProgramCardSnapshotQueries _snapshots;
    private readonly INativeLogicalDiscardOperations _logicalDiscards;

    internal ProgramPaidCardRuntime(IProgramPaidCardFacts facts, IProgramPaidCardOperations operations,
        IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts, IProgramRuntimeEvents events,
        IProgramCardSnapshotQueries snapshots, INativeLogicalDiscardOperations logicalDiscards)
    {
        _facts = facts;
        _operations = operations;
        _frames = frames;
        _prompts = prompts;
        _events = events;
        _snapshots = snapshots;
        _logicalDiscards = logicalDiscards;
    }

    private static ProgramSkillCardSetBinding CardSet(ProgramSkillFrame frame, string bind) =>
        frame.CardSetBindings.SingleOrDefault(binding => binding.Name == bind) ??
        throw new InvalidOperationException($"Program card-set binding '{bind}' is unavailable.");
}
