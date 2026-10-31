#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDamageAdjacencyServicesRuntime
{
    internal DamageAdjacencyRuntime DamageAdjacency => _damageAdjacencyOperations.Get();

    int global::CardGame.Core.IDamageAdjacencyFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    ResolutionFrame global::CardGame.Core.IDamageAdjacencyFacts.FrameAt(int index) => _resolutionStackOperations._resolutionStack[index];
    int global::CardGame.Core.IDamageAdjacencyFacts.FrameIndex(long id) => _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
    DamageTriggerWindowFrame? global::CardGame.Core.IDamageAdjacencyFacts.ActiveDamageTrigger => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger;

    DyingFrame? global::CardGame.Core.IDamageAdjacencyFacts.ActiveDying => _coreDyingCursorRuntimeOperations.ActiveDying;

    Winner global::CardGame.Core.IDamageAdjacencyFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    SkillProgram? global::CardGame.Core.IDamageAdjacencyFacts.Program(string skillId) => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(skillId, out var skill) ? skill.Program : null;
    bool global::CardGame.Core.IDamageAdjacencyProtocols.RequestedDeckBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.DrawFundedDistinctBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.ChainedStateBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreChainedStateBasicReturnsOperations.ChainedStateBasicFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.ProvenanceClaimTurnedOverEdge(ResolutionFrame child, ResolutionFrame parent) => _coreProvenanceClaimObserversOperations.ProvenanceClaimTurnedOverEdge(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.HulaoActivityFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreHulaoActivityProgramsOperations.HulaoActivityFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.ShenwuTrickFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreShenwuCardsOperations.ShenwuTrickFrameRidesOn(child, parent) || _coreMantianGuohaiOperations.MantianGuohaiFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.HellSoulFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreHellSoulCardsOperations.HellSoulFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.WenheChaosFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreWenheChaosOperations.WenheChaosFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.PileEquipmentFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.RandomEquipmentFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.RecoveryReplacementDamageObserverRidesOn(int index) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementDamageObserverRidesOn(index);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.CharacterTurnedOverDamageObserverRidesOn(int index) => _coreCharacterTurnedOverProgramsOperations.CharacterTurnedOverDamageObserverRidesOn(index);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.IsWenhePoisonHealthChild(ResolutionFrame parent, HpChangedTriggerWindowFrame child) => _coreWenhePoisonOperations.IsWenhePoisonHealthChild(parent, child);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.IsWenhePoisonDyingChild(ResolutionFrame parent, DyingFrame child) => _coreWenhePoisonOperations.IsWenhePoisonDyingChild(parent, child);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => _coreCardUsePaymentsOperations.IsPaidCardUseMovementReturn(use, movement);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.IsWenyiDamageHealthReturn(DamageFrame damage, HpChangedTriggerWindowFrame health) => _coreWenyiDamageHealthOperations.IsWenyiDamageHealthReturn(damage, health);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.IsHappyQiaoDamageHealthReturn(DamageFrame damage, HpChangedTriggerWindowFrame health) => _characterDaQiaoXiaoQiaoHappyGodOperations.IsHappyQiaoDamageHealthReturn(damage, health);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.IsAmountSupportHealthChild(ResolutionFrame parent, HpChangedTriggerWindowFrame health) => _characterCaoSongOlOperations.IsAmountSupportHealthChild(parent, health);
    TypedRecoveryDamageCursorProjection global::CardGame.Core.IDamageAdjacencyProtocols.ProjectTypedRecoveryDamageCursor(long windowId) => _coreTypedRecoveryDamageCursorOperations.ProjectTypedRecoveryDamageCursor(windowId);
    ProgramSkillFrame? global::CardGame.Core.IDamageAdjacencyProtocols.MouLixianDamageCursorRoot(DamageTriggerWindowFrame window, out long? dyingId) => _characterHuangYueyingOlMouOperations.MouLixianDamageCursorRoot(window, out dyingId);
    ProgramSkillFrame? global::CardGame.Core.IDamageAdjacencyProtocols.AlcoholSlashReplacementDamageCursorRoot(DamageTriggerWindowFrame window, out long? dyingId) => _coreAlcoholSlashSkillReplacementsOperations.AlcoholSlashReplacementDamageCursorRoot(window, out dyingId);
    ProgramSkillFrame? global::CardGame.Core.IDamageAdjacencyProtocols.RandomDamageTargetSkillDamageCursorRoot(DamageTriggerWindowFrame window, out long? dyingId) => _coreRandomDamageTargetSkillsOperations.RandomDamageTargetSkillDamageCursorRoot(window, out dyingId);
    ProgramSkillFrame? global::CardGame.Core.IDamageAdjacencyProtocols.DamageTargetSkillRoundDamageCursorRoot(DamageTriggerWindowFrame window, out long? dyingId) => _coreDamageTargetSkillRoundDrawOperations.DamageTargetSkillRoundDamageCursorRoot(window, out dyingId);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.MatchesObserver(ProgramObserverProofPhase phase, long windowId) => _coreProgramRuntimeHooksOperations.RuntimeHooks.MatchesObserver(phase, windowId);
    bool global::CardGame.Core.IDamageAdjacencyProtocols.HasPaidFactionRequestCostRecovery(PaidFactionRequestCostRecovery receipt) => _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } request && global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime.PaidFactionRequestMatches(request, receipt) && _coreFactionRequestCostRecoveriesOperations.HasPaidFactionRequestCostRecovery(request);
    DamageTriggerWindowFrame global::CardGame.Core.IDamageWindowOperations.SetWindowCursor(long windowId, int candidateIndex) => _coreNativeDamageFramesOperations.SetDamageTriggerWindowCursor(windowId, candidateIndex);
    void global::CardGame.Core.IDamageWindowOperations.SetWindowStep(long windowId, ResolutionFrameStep step) => _coreNativeDamageFramesOperations.SetDamageTriggerWindowStep(windowId, step);
    IDamageTriggerCapturedAttack global::CardGame.Core.IDamageWindowOperations.CaptureAttack(DamageTriggerWindowFrame window) => _kernelCoreDamageAdjacencyServicesOperations.CreateDamageTriggerCapturedAttack(_coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window));
}
