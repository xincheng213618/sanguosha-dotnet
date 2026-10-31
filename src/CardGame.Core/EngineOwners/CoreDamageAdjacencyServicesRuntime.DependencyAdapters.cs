#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreRequestedDeckBasicOwnershipAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreRequestedDeckBasicOwnershipOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime> _targetFactory = targetFactory;
    public bool RequestedDeckBasicFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().RequestedDeckBasicFrameRidesOn(child, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreFactionRequestCostRecoveriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreFactionRequestCostRecoveriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> _targetFactory = targetFactory;
    public bool HasPaidFactionRequestCostRecovery(global::CardGame.Core.NativeOwners.FactionCardRequestHandle? request) => this._targetFactory().HasPaidFactionRequestCostRecovery(request);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreDamageAdjacencyServicesCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyQiaoDamageHealthReturn(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.HpChangedTriggerWindowFrame health) => this._targetFactory().IsHappyQiaoDamageHealthReturn(damage, health);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreRandomDamageTargetSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreRandomDamageTargetSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? RandomDamageTargetSkillDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, out long? dyingId) => this._targetFactory().RandomDamageTargetSkillDamageCursorRoot(window, out dyingId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime))]
internal sealed class CoreDamageAdjacencyServicesCorePublicPileEquipmentSequencesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCorePublicPileEquipmentSequencesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime> _targetFactory = targetFactory;
    public bool PileEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().PileEquipmentFrameRidesOn(ride, beneath);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreCardUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreCardUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public bool IsPaidCardUseMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement) => this._targetFactory().IsPaidCardUseMovementReturn(use, movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreMantianGuohaiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreMantianGuohaiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> _targetFactory = targetFactory;
    public bool MantianGuohaiFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().MantianGuohaiFrameRidesOn(child, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreWenheChaosAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreWenheChaosOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> _targetFactory = targetFactory;
    public bool WenheChaosFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().WenheChaosFrameRidesOn(child, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreAlcoholSlashSkillReplacementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreAlcoholSlashSkillReplacementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? AlcoholSlashReplacementDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, out long? dyingId) => this._targetFactory().AlcoholSlashReplacementDamageCursorRoot(window, out dyingId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime))]
internal sealed class CoreDamageAdjacencyServicesCharacterHuangYueyingOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCharacterHuangYueyingOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? MouLixianDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, out long? ownedDyingId) => this._targetFactory().MouLixianDamageCursorRoot(window, out ownedDyingId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreCharacterTurnedOverProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreCharacterTurnedOverProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> _targetFactory = targetFactory;
    public bool CharacterTurnedOverDamageObserverRidesOn(int index) => this._targetFactory().CharacterTurnedOverDamageObserverRidesOn(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreTypedRecoveryDamageCursorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreTypedRecoveryDamageCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.TypedRecoveryDamageCursorProjection ProjectTypedRecoveryDamageCursor(long windowId) => this._targetFactory().ProjectTypedRecoveryDamageCursor(windowId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreDrawFundedDistinctBasicObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreDrawFundedDistinctBasicObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime> _targetFactory = targetFactory;
    public bool DrawFundedDistinctBasicFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().DrawFundedDistinctBasicFrameRidesOn(ride, beneath);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreChainedStateBasicReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreChainedStateBasicReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> _targetFactory = targetFactory;
    public bool ChainedStateBasicFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().ChainedStateBasicFrameRidesOn(child, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime))]
internal sealed class CoreDamageAdjacencyServicesCharacterCaoSongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCharacterCaoSongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> _targetFactory = targetFactory;
    public bool IsAmountSupportHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame hp) => this._targetFactory().IsAmountSupportHealthChild(parent, hp);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.DamageAdjacencyRuntime))]
internal sealed class CoreDamageAdjacencyServicesDamageAdjacencyAdapter(global::System.Func<global::CardGame.Core.DamageAdjacencyRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesDamageAdjacencyOperations
{
    private readonly global::System.Func<global::CardGame.Core.DamageAdjacencyRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageAdjacencyRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public bool IsWenhePoisonHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame child) => this._targetFactory().IsWenhePoisonHealthChild(parent, child);
    public bool IsWenhePoisonDyingChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.DyingFrame child) => this._targetFactory().IsWenhePoisonDyingChild(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get => this._targetFactory().ActiveFactionCardRequest; set => this._targetFactory().ActiveFactionCardRequest = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame) => this._targetFactory().GetDamageTriggerAttack(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceClaimObserversRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreProvenanceClaimObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceClaimObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreProvenanceClaimObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceClaimObserversRuntime> _targetFactory = targetFactory;
    public bool ProvenanceClaimTurnedOverEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().ProvenanceClaimTurnedOverEdge(child, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreWenyiDamageHealthAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreWenyiDamageHealthOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> _targetFactory = targetFactory;
    public bool IsWenyiDamageHealthReturn(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.HpChangedTriggerWindowFrame health) => this._targetFactory().IsWenyiDamageHealthReturn(damage, health);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().RandomEquipmentFrameRidesOn(ride, beneath);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageFramesRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreNativeDamageFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreNativeDamageFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageFramesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame SetDamageTriggerWindowCursor(long frameId, int candidateIndex) => this._targetFactory().SetDamageTriggerWindowCursor(frameId, candidateIndex);
    public void SetDamageTriggerWindowStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetDamageTriggerWindowStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHulaoActivityProgramsRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreHulaoActivityProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHulaoActivityProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreHulaoActivityProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHulaoActivityProgramsRuntime> _targetFactory = targetFactory;
    public bool HulaoActivityFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().HulaoActivityFrameRidesOn(child, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public bool HellSoulFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().HellSoulFrameRidesOn(child, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreRecoveryReplacementDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreRecoveryReplacementDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> _targetFactory = targetFactory;
    public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().RecoveryReplacementFrameRidesOn(ride, beneath);
    public bool RecoveryReplacementDamageObserverRidesOn(int index) => this._targetFactory().RecoveryReplacementDamageObserverRidesOn(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreDamageTargetSkillRoundDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreDamageTargetSkillRoundDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? DamageTargetSkillRoundDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, out long? dyingId) => this._targetFactory().DamageTargetSkillRoundDamageCursorRoot(window, out dyingId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreShenwuCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreShenwuCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> _targetFactory = targetFactory;
    public bool ShenwuTrickFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().ShenwuTrickFrameRidesOn(ride, beneath);
}
