namespace CardGame.Core;

internal interface IProgramBoundCardFacts : IProgramSelectionQuery
{
    bool IsValidSeat(int seat);
    bool IsHuman(int seat);
    string Name(int seat);
    bool HasHpValue(int seat);
    int Hp(int seat);
    int MaxHp(int seat);
    IReadOnlyList<Card> Hand(int seat);
    IReadOnlyList<Card> Equipment(int seat);
    IReadOnlyList<Card> CardsAt(CardLocation location);
    CardLocation Location(int cardId);
    CardMovementRecord? LastMovement(int cardId);
    EquipmentSlot FieldEquipmentSlot(Card card);
    Suit EffectiveSuit(int seat, Card card);
    bool ForeignDiscardPrevented(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent);
}

internal interface IProgramBoundInstructionFacts : IProgramSelectionQuery
{
    ContentSkillDefinition Skill(string skillId);
    bool HasForeignDiscardCapability { get; }
    bool HasAtomicDiscardCapability { get; }
    int? SelectionActor(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations);
    int Participant(ProgramSkillFrame frame, ProgramParticipantReference reference);
    IReadOnlyList<int> TargetSeats(int owner, SkillProgramTargetKind kind, ProgramSkillWindowContext? context = null);
    bool IsAtomicDiscard(ProgramSkillFrame frame, string sourceBind);
    IReadOnlyList<PromptChoice> PresentDistribution(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices,
        string sourceBind, SkillProgramTargetKind targetKind);
}

// A transient argument for the existing native completion callback. It is never
// retained by a module or substituted for the original frame's payment state.
internal sealed record ProgramBoundCardGive(long FrameId, int Giver, int Recipient,
    IReadOnlyList<int> CardIds, long SequenceBefore);

internal interface IProgramBoundCardMovementOperations : IProgramRuntimeOperations
{
    long Sequence { get; }
    Card? DrawToProcessing(CardMoveReason reason);
    void Move(Card card, CardLocation source, CardLocation destination, CardMoveReason reason,
        ProgramBoundCardGive? give = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified);
    void MoveCards(IReadOnlyList<Card> cards, CardLocation source, CardLocation destination, CardMoveReason reason,
        ProgramBoundCardGive? give = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified);
    void MoveMultipleSources(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason,
        CardMovementDisclosure disclosure, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords = null);
    void PlaceAtBottom(IReadOnlyList<int> ids);
    void PlaceAtTop(IReadOnlyList<int> ids);
}

// Existing native payment producers keep their exact frame identities and
// receipts. The generic runtime only sequences their established adjacency.
internal interface IProgramBoundCardPaymentOperations : IProgramRuntimeOperations
{
    void Cancel(ProgramSkillFrame frame, string reason);
    void CleanupRepeatedJudgment(ProgramSkillFrame frame);
    void CleanupDeckSlash(ProgramSkillFrame frame);
    ProgramSkillFrame FreezeSequentialDiscardTop(ProgramSkillFrame frame, string bind, string? exceptBind,
        SkillProgramCardDestination destination);
    ProgramSkillFrame FreezeLostHpChained(ProgramSkillFrame frame, string bind, string? exceptBind,
        SkillProgramCardDestination destination);
    long? BeginWholeHandAcquisition(ProgramSkillFrame frame, string bind, SkillProgramCardDestination destination);
    void CommitWholeHandAcquisition(long frameId, string bind, long sequenceBefore);
    bool PreparePhaseAlcohol(ProgramSkillFrame frame, string bind, CardMoveReason reason);
    void FreezeDiscardColors(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations);
    SkillProgramStepOutcome BeginParticipantGift(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids,
        CardLocation target, CardMoveReason reason);
    void CommitLostHpChained(long frameId);
    void CommitPhaseAlcohol(long frameId);
}

public sealed partial class GameEngine
{
}
