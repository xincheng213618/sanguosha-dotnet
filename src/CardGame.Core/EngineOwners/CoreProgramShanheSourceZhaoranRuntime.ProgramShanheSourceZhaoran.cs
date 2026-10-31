#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramShanheSourceZhaoranRuntime
{
    internal const string ShanheSourceZhaoranSkill = "shanhe:source-zhaoran-7014";
    internal bool HasShanheSourceZhaoranRules => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ArmShanheSourceZhaoran);
    internal bool IsShanheSourceZhaoranMode => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null;

    internal ShanheSourceZhaoranArmedEvent? ActiveShanheSourceZhaoranArm(int ownerSeat)
    {
        if (!HasShanheSourceZhaoranRules || !IsShanheSourceZhaoranMode || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != ownerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId <= 0)
            return null;
        var arm = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanheSourceZhaoranArmedEvent>().LastOrDefault(e => e.Source.OwnerSeat == ownerSeat && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PlayInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
        return arm is not null && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).ProgramInstances.Any(instance => instance.SkillId == ShanheSourceZhaoranSkill && instance.SkillInstanceId == arm.Source.SkillInstanceId) ? arm : null;
    }

    internal bool HasShanheSourceZhaoranPublicHand(int ownerSeat) => ActiveShanheSourceZhaoranArm(ownerSeat)is not null;
    internal void ArmShanheSourceZhaoran(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsShanheSourceZhaoranMode || frame.SkillId != ShanheSourceZhaoranSkill || frame.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } context || context.OwnerSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != frame.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId <= 0 || ActiveShanheSourceZhaoranArm(frame.OwnerSeat)is not null)
            throw new InvalidOperationException("Source Zhaoran must arm once on its owner's exact actual Play entry.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceZhaoranArmedEvent(frame.Id, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId));
    }

    internal void CaptureShanheSourceZhaoranSuitLosses(CardMovementBatchContext batch)
    {
        if (!HasShanheSourceZhaoranRules || ActiveShanheSourceZhaoranArm(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)is not { } arm || batch.TurnNumber != arm.ActualTurn || batch.MovementTiming is not { Phase: TurnPhase.Play } timing || timing.PhaseActorSeat != arm.Source.OwnerSeat)
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[arm.Source.OwnerSeat];
        var losses = batch.Movements.Select((movement, index) => (movement, index)).Where(item => item.movement.From == CardLocation.Hand(owner.Seat) && item.movement.To != item.movement.From).Select(item => (item.movement, item.index, suit: _coreNativeCardAppearanceOperations.EffectiveSuit(owner, _coreAdvancedLifecycleOperations.GetAdvancedCard(item.movement.CardId)))).Where(item => item.suit != Suit.None).GroupBy(item => item.suit).ToArray();
        foreach (var suitLosses in losses)
        {
            if (_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Any(card => _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card) == suitLosses.Key) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanheSourceZhaoranSuitLostEvent>().Any(e => e.OwnerSeat == owner.Seat && e.ActualTurn == arm.ActualTurn && e.PlayInstance == arm.PlayInstance && e.Suit == suitLosses.Key))
                continue;
            var first = suitLosses.First();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceZhaoranSuitLostEvent(arm.FrameId, owner.Seat, arm.Source.SkillInstanceId, arm.ActualTurn, arm.PlayInstance, batch.Id, first.index, first.movement.Sequence, suitLosses.Key));
        }
    }

    internal ShanheSourceZhaoranSuitLostEvent? EligibleShanheSourceZhaoranSuit(int owner, string instanceId, ProgramSkillWindowContext? context)
    {
        if (ActiveShanheSourceZhaoranArm(owner)is not { } arm || arm.Source.SkillInstanceId != instanceId || context is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count || batch.Movements[index].From != CardLocation.Hand(owner) || batch.Movements[index].From == batch.Movements[index].To)
            return null;
        var loss = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanheSourceZhaoranSuitLostEvent>().SingleOrDefault(e => e.ArmFrameId == arm.FrameId && e.OwnerSeat == owner && e.SkillInstanceId == instanceId && e.BatchId == batch.Id && e.MovementIndex == index && e.MovementSequence == batch.Movements[index].Sequence);
        return loss is not null && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanheSourceZhaoranSuitClaimedEvent>().Any(e => e.ArmFrameId == loss.ArmFrameId && e.OwnerSeat == owner && e.Suit == loss.Suit) ? loss : null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24661, SkillProgramEffectOp.ClaimShanheSourceZhaoranSuit)]
    internal bool CanClaimShanheSourceZhaoranSuit(ProgramTriggerLegalityContext context) => context.SkillId == ShanheSourceZhaoranSkill && EligibleShanheSourceZhaoranSuit(context.OwnerSeat, context.Candidate.SkillInstanceId, context.WindowContext)is not null;
    internal void ClaimShanheSourceZhaoranSuit(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var loss = EligibleShanheSourceZhaoranSuit(frame.OwnerSeat, frame.SkillInstanceId, frame.WindowContext);
        if (frame.SkillId != ShanheSourceZhaoranSkill || loss is null || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not CardsMovedTriggerWindowFrame window || frame.WindowContext?.ParentFrameId != window.Id || window.Batch.Id != loss.BatchId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex]))
            throw new InvalidOperationException("Source Zhaoran lost its exact first last-suit batch and original movement owner.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceZhaoranSuitClaimedEvent(frame.Id, loss.ArmFrameId, frame.OwnerSeat, loss.BatchId, loss.MovementIndex, loss.Suit));
    }

    void global::CardGame.Core.IShanheSourceZhaoranHost.Arm(ProgramSkillFrame frame) => ArmShanheSourceZhaoran(frame);
    void global::CardGame.Core.IShanheSourceZhaoranHost.ClaimSuit(ProgramSkillFrame frame) => ClaimShanheSourceZhaoranSuit(frame);
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 200)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureShanheSourceZhaoranSuitLosses(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureShanheSourceZhaoranSuitLosses(completed);
        return completed;
    }
}
