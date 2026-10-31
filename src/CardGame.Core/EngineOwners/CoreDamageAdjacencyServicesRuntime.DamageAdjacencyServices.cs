#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDamageAdjacencyServicesRuntime
{
    internal DamageAdjacencyRuntime DamageAdjacency => _damageAdjacencyOperations.Get();

    int IDamageAdjacencyFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    ResolutionFrame IDamageAdjacencyFacts.FrameAt(int index) => _resolutionStackOperations._resolutionStack[index];
    int IDamageAdjacencyFacts.FrameIndex(long id) => _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
    DamageTriggerWindowFrame? IDamageAdjacencyFacts.ActiveDamageTrigger => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger;

    DyingFrame? IDamageAdjacencyFacts.ActiveDying => _coreDyingCursorRuntimeOperations.ActiveDying;

    Winner IDamageAdjacencyFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    SkillProgram? IDamageAdjacencyFacts.Program(string skillId) => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(skillId, out var skill) ? skill.Program : null;
    bool IDamageAdjacencyProtocols.RequestedDeckBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicFrameRidesOn(child, parent);
    bool IDamageAdjacencyProtocols.DrawFundedDistinctBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicFrameRidesOn(child, parent);
    bool IDamageAdjacencyProtocols.ChainedStateBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreChainedStateBasicReturnsOperations.ChainedStateBasicFrameRidesOn(child, parent);
    bool IDamageAdjacencyProtocols.ProvenanceClaimTurnedOverEdge(ResolutionFrame child, ResolutionFrame parent) => _coreProvenanceClaimObserversOperations.ProvenanceClaimTurnedOverEdge(child, parent);
    bool IDamageAdjacencyProtocols.HulaoActivityFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreHulaoActivityProgramsOperations.HulaoActivityFrameRidesOn(child, parent);
    bool IDamageAdjacencyProtocols.ShenwuTrickFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreShenwuCardsOperations.ShenwuTrickFrameRidesOn(child, parent) || _coreMantianGuohaiOperations.MantianGuohaiFrameRidesOn(child, parent);
    bool IDamageAdjacencyProtocols.HellSoulFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreHellSoulCardsOperations.HellSoulFrameRidesOn(child, parent);
    bool IDamageAdjacencyProtocols.WenheChaosFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreWenheChaosOperations.WenheChaosFrameRidesOn(child, parent);
    bool IDamageAdjacencyProtocols.RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    bool IDamageAdjacencyProtocols.PileEquipmentFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(child, parent);
    bool IDamageAdjacencyProtocols.RandomEquipmentFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(child, parent);
    bool IDamageAdjacencyProtocols.RecoveryReplacementDamageObserverRidesOn(int index) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementDamageObserverRidesOn(index);
    bool IDamageAdjacencyProtocols.CharacterTurnedOverDamageObserverRidesOn(int index) => _coreCharacterTurnedOverProgramsOperations.CharacterTurnedOverDamageObserverRidesOn(index);
    bool IDamageAdjacencyProtocols.IsWenhePoisonHealthChild(ResolutionFrame parent, HpChangedTriggerWindowFrame child) => _coreWenhePoisonOperations.IsWenhePoisonHealthChild(parent, child);
    bool IDamageAdjacencyProtocols.IsWenhePoisonDyingChild(ResolutionFrame parent, DyingFrame child) => _coreWenhePoisonOperations.IsWenhePoisonDyingChild(parent, child);
    bool IDamageAdjacencyProtocols.IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => _coreCardUsePaymentsOperations.IsPaidCardUseMovementReturn(use, movement);
    bool IDamageAdjacencyProtocols.IsWenyiDamageHealthReturn(DamageFrame damage, HpChangedTriggerWindowFrame health) => _coreWenyiDamageHealthOperations.IsWenyiDamageHealthReturn(damage, health);
    bool IDamageAdjacencyProtocols.IsHappyQiaoDamageHealthReturn(DamageFrame damage, HpChangedTriggerWindowFrame health) => _characterDaQiaoXiaoQiaoHappyGodOperations.IsHappyQiaoDamageHealthReturn(damage, health);
    bool IDamageAdjacencyProtocols.IsAmountSupportHealthChild(ResolutionFrame parent, HpChangedTriggerWindowFrame health) => _characterCaoSongOlOperations.IsAmountSupportHealthChild(parent, health);
    bool IDamageAdjacencyProtocols.HasPaidFactionRequestCostRecovery(PaidFactionRequestCostRecovery receipt) => _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } request && CoreGameEngineFactionRequestCostRecoveriesRuntime.PaidFactionRequestMatches(request, receipt) && _coreFactionRequestCostRecoveriesOperations.HasPaidFactionRequestCostRecovery(request);
    DamageTriggerWindowFrame IDamageWindowOperations.SetWindowCursor(long windowId, int candidateIndex) => _coreNativeDamageFramesOperations.SetDamageTriggerWindowCursor(windowId, candidateIndex);
    void IDamageWindowOperations.SetWindowStep(long windowId, ResolutionFrameStep step) => _coreNativeDamageFramesOperations.SetDamageTriggerWindowStep(windowId, step);
    IDamageTriggerCapturedAttack IDamageWindowOperations.CaptureAttack(DamageTriggerWindowFrame window) => _kernelCoreDamageAdjacencyServicesOperations.CreateDamageTriggerCapturedAttack(_coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window));
}
