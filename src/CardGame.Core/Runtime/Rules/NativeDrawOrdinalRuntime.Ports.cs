namespace CardGame.Core;

internal interface INativeDrawOrdinalInvoices : IProgramRuntimeOperations
{
    IReadOnlyList<NativeDrawOrdinalEntitlement>? Capture(long invocationId, int ownerSeat, long before);
    int? Freeze(NativeDrawInvocationDraft draft);
    bool IsValid(NativeDrawInvocationProof proof);
}

internal interface INativeDrawOrdinalSkillInstances : IProgramSelectionQuery
{
    bool HasRuntimeSkillInstance(CharacterState owner, string skillId, string skillInstanceId);
}
