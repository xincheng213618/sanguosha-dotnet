namespace CardGame.Core;

internal readonly record struct ProgramDamageCardOwner(long FrameId, bool IsNativeCard);

// These remaining native boundaries select the current attack through existing
// suspension rules and issue multi-effect receipts on their original owners.
// Material lookup, physical claims, group accounting and events live in the
// independent runtime; neither boundary owns another pending state or ledger.
internal interface IProgramDamageCardClaimNativeRules : IProgramRuntimeOperations
{
    DamageTriggerWindowFrame? CurrentDamageWindow { get; }
    ProgramDamageCardOwner SelectAttackOwner(DamageTriggerWindowFrame window);
    long? ActiveGroupOwnerId { get; }
    void CaptureExtraEffectClaims(ProgramSkillFrame frame, DamageTriggerWindowFrame window, IReadOnlyList<Card> cards,
        long sequenceBefore, CardMoveReason reason);
}

public sealed partial class GameEngine
{
}
