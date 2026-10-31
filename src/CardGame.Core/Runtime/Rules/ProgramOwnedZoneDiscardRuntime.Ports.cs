namespace CardGame.Core;

// The native producer retains the original immutable invoice. The generic
// discard sequences its completion without copying or interpreting its fields.
internal interface IProgramOwnedZoneDiscardInvocation { }

internal interface IProgramOwnedZoneDiscardFacts : IProgramSelectionQuery
{
    bool HasAtomicDiscardReplacement { get; }
    CardLocation? DiscardSource(CardMovementRecord movement);
}

internal interface IProgramOwnedZoneDiscardReceipts : IProgramRuntimeOperations
{
    IProgramOwnedZoneDiscardInvocation? Capture(ProgramSkillFrame frame,
        IReadOnlyList<CardZoneKind> zones, CardMoveReason reason);
    void Complete(IProgramOwnedZoneDiscardInvocation? invocation, IReadOnlyList<int> actualIds);
}
