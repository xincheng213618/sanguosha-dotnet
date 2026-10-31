#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDamageAdjacencyServicesCoreRequestedDeckBasicOwnershipOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RequestedDeckBasicFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreDamageAdjacencyServicesCoreFactionRequestCostRecoveriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidFactionRequestCostRecovery(global::CardGame.Core.NativeOwners.FactionCardRequestHandle? request);
}

internal interface ICoreDamageAdjacencyServicesCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyQiaoDamageHealthReturn(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.HpChangedTriggerWindowFrame health);
}

internal interface ICoreDamageAdjacencyServicesCoreRandomDamageTargetSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? RandomDamageTargetSkillDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, out long? dyingId);
}

internal interface ICoreDamageAdjacencyServicesCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreDamageAdjacencyServicesCorePublicPileEquipmentSequencesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PileEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreDamageAdjacencyServicesCoreCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidCardUseMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement);
}

internal interface ICoreDamageAdjacencyServicesCoreMantianGuohaiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MantianGuohaiFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreDamageAdjacencyServicesCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreDamageAdjacencyServicesCoreWenheChaosOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WenheChaosFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreDamageAdjacencyServicesCoreAlcoholSlashSkillReplacementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? AlcoholSlashReplacementDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, out long? dyingId);
}

internal interface ICoreDamageAdjacencyServicesCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? MouLixianDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, out long? ownedDyingId);
}

internal interface ICoreDamageAdjacencyServicesCoreCharacterTurnedOverProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CharacterTurnedOverDamageObserverRidesOn(int index);
}

internal interface ICoreDamageAdjacencyServicesCoreTypedRecoveryDamageCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.TypedRecoveryDamageCursorProjection ProjectTypedRecoveryDamageCursor(long windowId);
}

internal interface ICoreDamageAdjacencyServicesCoreDrawFundedDistinctBasicObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DrawFundedDistinctBasicFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreDamageAdjacencyServicesCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreDamageAdjacencyServicesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDamageAdjacencyServicesCoreChainedStateBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ChainedStateBasicFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreDamageAdjacencyServicesCharacterCaoSongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAmountSupportHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame hp);
}

internal interface ICoreDamageAdjacencyServicesDamageAdjacencyOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageAdjacencyRuntime Get();
}

internal interface ICoreDamageAdjacencyServicesCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenhePoisonHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame child);
    bool IsWenhePoisonDyingChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.DyingFrame child);
}

internal interface ICoreDamageAdjacencyServicesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDamageAdjacencyServicesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }
}

internal interface ICoreDamageAdjacencyServicesCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame);
}

internal interface ICoreDamageAdjacencyServicesCoreProvenanceClaimObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProvenanceClaimTurnedOverEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreDamageAdjacencyServicesKernelCoreDamageAdjacencyServicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DamageTriggerCapturedAttack CreateDamageTriggerCapturedAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreDamageAdjacencyServicesCoreWenyiDamageHealthOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenyiDamageHealthReturn(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.HpChangedTriggerWindowFrame health);
}

internal interface ICoreDamageAdjacencyServicesCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreDamageAdjacencyServicesCoreNativeDamageFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame SetDamageTriggerWindowCursor(long frameId, int candidateIndex);
    void SetDamageTriggerWindowStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreDamageAdjacencyServicesCoreHulaoActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HulaoActivityFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreDamageAdjacencyServicesCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HellSoulFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreDamageAdjacencyServicesCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
    bool RecoveryReplacementDamageObserverRidesOn(int index);
}

internal interface ICoreDamageAdjacencyServicesCoreDamageTargetSkillRoundDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? DamageTargetSkillRoundDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, out long? dyingId);
}

internal interface ICoreDamageAdjacencyServicesCoreShenwuCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShenwuTrickFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}
