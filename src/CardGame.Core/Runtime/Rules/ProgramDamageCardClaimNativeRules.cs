namespace CardGame.Core;

internal readonly record struct ProgramDamageCardOwner(long FrameId, bool IsNativeCard);

// These remaining native boundaries select the current attack through existing
// suspension rules. Actual material receipts stay on the original native use.
// Material lookup, physical claims, group accounting and events live in the
// independent runtime; neither boundary owns another pending state or ledger.
internal interface IProgramDamageCardClaimNativeRules : IProgramRuntimeOperations
{
    DamageTriggerWindowFrame? CurrentDamageWindow { get; }
    ProgramDamageCardOwner SelectAttackOwner(DamageTriggerWindowFrame window);
    long? ActiveGroupOwnerId { get; }
}
