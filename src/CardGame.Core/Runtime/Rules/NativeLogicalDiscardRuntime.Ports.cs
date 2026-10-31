namespace CardGame.Core;

internal interface INativeLogicalDiscardOperations : IProgramRuntimeOperations
{
    bool TracksInvocations { get; }
    NativeLogicalDiscardInvocationDraft? Capture(int initiatorSeat, int executorSeat, IReadOnlyList<int> cardIds,
        CardMoveReason reason, long? parentFrameId = null);
    void Complete(NativeLogicalDiscardInvocationDraft? draft, IReadOnlyList<CardMovementRecord>? prepaidMaterials = null);
    void MoveProgram(ProgramSkillFrame frame, int executorSeat, IReadOnlyList<int> ids, CardMoveReason reason,
        Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords = null);
    void Move(int initiatorSeat, int executorSeat, Card card, CardLocation from, CardMoveReason reason,
        Action<CardMovementRecord>? beforeFact = null, long? parentFrameId = null);
    void MoveCards(int initiatorSeat, int executorSeat, IReadOnlyList<Card> cards, CardLocation from, CardMoveReason reason);
    ProgramSkillFrame AppendFrame(ProgramSkillFrame frame, int executorSeat, IReadOnlyList<int> ids, CardMoveReason reason);
    ProgramSkillFrame CompleteFrame(ProgramSkillFrame frame);
}

// Atomic physical movement is shared with the actual movement runtime. Invoice
// ordering remains here, and callbacks still run before each original fact.
internal interface INativeLogicalDiscardMovement : IProgramRuntimeOperations
{
    void Move(Card card, CardLocation from, CardLocation to, CardMoveReason reason,
        Action<CardMovementRecord>? beforeFact = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified);
    void MoveCards(IReadOnlyList<Card> cards, CardLocation from, CardLocation to, CardMoveReason reason,
        Action<CardMovementRecord>? beforeFact = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified);
}

// Preserve the original counters: the current movement sequence is not
// interchangeable with the last physical journal record.
internal interface INativeLogicalDiscardFacts : IProgramSelectionQuery
{
    bool SetupComplete { get; }
    int MovementSequence { get; }
    long ResolutionSequence { get; }
}

// Original source and prepaid-material owners retain their native proof rules.
internal interface INativeLogicalDiscardOrigins : IProgramSelectionQuery
{
    bool IsOwnedDiscardSource(CardLocation location);
    bool IsProgramDiscardOriginReason(CardMoveReason reason);
    CardLocation? GetProgramDiscardSource(CardMovementRecord movement);
    bool IsEphemeralCardDestroyedMaterial(CardMovementRecord movement);
}

// Zisheng issues and closes its own material admission on the original frame.
internal interface INativeLogicalDiscardAdmissions : IProgramRuntimeOperations
{
    NativeLogicalDiscardInvocationDraft? Admit(NativeLogicalDiscardInvocationDraft? draft);
    void AdmitMaterials(NativeLogicalDiscardInvocationDraft draft);
    void Close(NativeLogicalDiscardInvocationDraft? draft);
}

public sealed partial class GameEngine
{
}
