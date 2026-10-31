#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRecoveryReplacementDamageRidesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRecoveryReplacementDamageRidesCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreRecoveryReplacementDamageRidesCoreTypedRecoveryDamageCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.TypedCommittedSlashPaymentCursorProjection ProjectTypedCommittedSlashPaymentCursor();
}

internal interface ICoreRecoveryReplacementDamageRidesCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreRecoveryReplacementDamageRidesCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreRecoveryReplacementDamageRidesCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreRecoveryReplacementDamageRidesCorePublicPileEquipmentSequencesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PileEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreRecoveryReplacementDamageRidesCoreCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidCardUseMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement);
}

internal interface ICoreRecoveryReplacementDamageRidesCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduCardEffectRecoveryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryReplacementFrame frame);
}

internal interface ICoreRecoveryReplacementDamageRidesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreRecoveryReplacementDamageRidesCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
    global::CardGame.Core.ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false);
}

internal interface ICoreRecoveryReplacementDamageRidesCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWumiaoFloodRecoveryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryReplacementFrame frame);
}

internal interface ICoreRecoveryReplacementDamageRidesCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDragonBoatCardEffectRecoveryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryReplacementFrame recovery);
}
