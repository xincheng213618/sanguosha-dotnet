namespace CardGame.Core;

internal interface IDamageAdjacencyFacts : IProgramSelectionQuery
{
    int FrameCount { get; }
    ResolutionFrame FrameAt(int index);
    int FrameIndex(long id);
    DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
    DyingFrame? ActiveDying { get; }
    Winner Winner { get; }
    SkillProgram? Program(string skillId);
}

// These are neighboring mechanism proofs; this module owns the traversal and
// their short-circuit order, while each proof keeps its original paid receipt.
internal interface IDamageAdjacencyProtocols : IProgramRuntimeOperations
{
    bool RequestedDeckBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool DrawFundedDistinctBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool ChainedStateBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool ProvenanceClaimTurnedOverEdge(ResolutionFrame child, ResolutionFrame parent);
    bool HulaoActivityFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool ShenwuTrickFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool HellSoulFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool WenheChaosFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool PileEquipmentFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool RandomEquipmentFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool RecoveryReplacementDamageObserverRidesOn(int index);
    bool CharacterTurnedOverDamageObserverRidesOn(int index);
    bool IsWenhePoisonHealthChild(ResolutionFrame parent, HpChangedTriggerWindowFrame child);
    bool IsWenhePoisonDyingChild(ResolutionFrame parent, DyingFrame child);
    bool IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement);
    bool IsWenyiDamageHealthReturn(DamageFrame damage, HpChangedTriggerWindowFrame health);
    bool IsHappyQiaoDamageHealthReturn(DamageFrame damage, HpChangedTriggerWindowFrame health);
    bool IsAmountSupportHealthChild(ResolutionFrame parent, HpChangedTriggerWindowFrame health);
    bool HasPaidFactionRequestCostRecovery(PaidFactionRequestCostRecovery receipt);
}

internal interface IDamageWindowOperations : IProgramRuntimeOperations
{
    DamageTriggerWindowFrame SetWindowCursor(long windowId, int candidateIndex);
    void SetWindowStep(long windowId, ResolutionFrameStep step);
    IDamageTriggerCapturedAttack CaptureAttack(DamageTriggerWindowFrame window);
}

// Ephemeral views preserve the original attack lookup before SetStep and the
// target lookup after it. They expose only this completion, with no player or
// attack state object available to the independent runtime or its fields.
internal interface IDamageTriggerCapturedAttack : IProgramRuntimeOperations
{
    IDamageTriggerCompletion BindTarget();
}

internal interface IDamageTriggerCompletion : IProgramRuntimeOperations
{
    int Hp { get; }
    bool IsAlive { get; }
    int SourceSeat { get; }
    int TargetSeat { get; }
    int DamageAmount { get; }
    DamageNature Nature { get; }
    bool IsSourceLess { get; }
    bool HasResolvedDying(long damageFrameId);
    void BeginDying(long damageFrameId);
    bool TryBeginAfterDamage(long damageFrameId);
    void CompleteAttack();
}

public sealed partial class GameEngine
{
}
