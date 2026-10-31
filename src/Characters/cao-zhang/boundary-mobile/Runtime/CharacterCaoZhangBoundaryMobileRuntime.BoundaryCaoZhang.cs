#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhangBoundaryMobileRuntime
{
    internal static CardConversionSource BoundaryCaoZhangSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IBoundaryCaoZhangHost.Execute(ProgramSkillFrame f, SkillProgramEffect e) => ExecuteBoundaryCaoZhang(f, e);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 55500)]
    internal bool CanOfferBoundaryCaoZhang(ProgramTriggerCandidate c) => c.SkillId != BoundaryCaoZhangComposition.Skill || c.BindingId != "discard-assault" || _coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(c.OwnerSeat, c.OwnerSeat);
    internal bool ValidBoundaryJiangchiPreparation(ProgramSkillFrame f, BoundaryJiangchiPreparation r) => f.SkillId == BoundaryCaoZhangComposition.Skill && r.Source == BoundaryCaoZhangSource(f) && r.Hash == f.GameplayHash && r.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && r.Phase.ActorSeat == f.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) && r.DebitPhase > 0 && r.Before >= 0 && r.Before <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && r.Mode == (f.TriggerId == "draw-restraint" ? "restraint" : f.TriggerId == "discard-assault" ? "assault" : null) && f.WindowContext is { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: var actor, TargetSeat: var target } context && actor == f.OwnerSeat && target == f.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().Any(parent => parent.Id == context.ParentFrameId && parent.OwnerSeat == f.OwnerSeat);
    internal SkillProgramStepOutcome ExecuteBoundaryCaoZhang(ProgramSkillFrame supplied, SkillProgramEffect e)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (e.Op == SkillProgramEffectOp.PrepareBoundaryCaoZhangJiangchi)
        {
            if (f.InstructionIndex != 1 || f.BoundaryJiangchi is not null || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != f.OwnerSeat || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat)
                throw new InvalidOperationException("Jiangchi preparation requires the original actual own Play-start.");
            var r = new BoundaryJiangchiPreparation(BoundaryCaoZhangSource(f), f.GameplayHash, phase, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, e.StateId!, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
            if (!ValidBoundaryJiangchiPreparation(f, r))
                throw new InvalidOperationException("Jiangchi lost its exact native starting parent.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryJiangchi = r });
            return SkillProgramStepOutcome.Continue;
        }

        if (f.InstructionIndex != 3 || f.BoundaryJiangchi is not { } paid || !ValidBoundaryJiangchiPreparation(f, paid) || paid.Mode != e.StateId || f.PendingMovementContinuation is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryJiangchiIssuedEvent>().Any(g => g.FrameId == f.Id))
            throw new InvalidOperationException("Jiangchi must issue once after its original completed native draw or paid discard child.");
        int? cost = null;
        if (paid.Mode == "assault")
        {
            var bound = _coreProgramLifecycleOperations.GetProgramCardSet(f, BoundaryCaoZhangComposition.Cost);
            if (bound.CardIds is not [var id] || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > paid.Before && m.CardId == id && m.From.OwnerSeat == f.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program." + f.SkillId + ".SelectAndMoveOwnedCard") != 1)
                throw new InvalidOperationException("Jiangchi assault requires its one actual HE discard, never a recast or unpaid selection.");
            cost = id;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase() != paid.Phase || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId != paid.DebitPhase)
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryJiangchiIssuedEvent(f.Id, paid.Source, paid.Hash, paid.Phase, paid.DebitPhase, paid.Mode, cost, paid.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        return SkillProgramStepOutcome.Continue;
    }

    internal BoundaryJiangchiIssuedEvent? ActiveBoundaryJiangchi(int owner)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ArmBoundaryCaoZhangJiangchi) || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != owner)
            return null;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryJiangchiIssuedEvent>().SingleOrDefault(g => g.Source.OwnerSeat == owner && g.Phase == phase && g.DebitPhase == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
    }

    internal IEnumerable<RuleQueryContribution> BoundaryCaoZhangPhaseContributions(CharacterState player, SkillRuleQuery query)
    {
        if (query != SkillRuleQuery.SlashLimit || ActiveBoundaryJiangchi(player.Seat)is not { Mode: "assault" } grant)
            yield break;
        yield return new FiniteRuleQueryContribution("issued:boundary-jiangchi:" + grant.FrameId, SkillRuleOperation.Add, 1);
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 55500)]
    internal bool BoundaryJiangchiCardDistance(ProgramCardDistanceQuery q) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && ActiveBoundaryJiangchi(q.Actor.Seat)is { Mode: "assault" };
    [ProgramDistanceRule(ProgramDistanceStage.ConvertedSlashExemption, 55500)]
    internal bool BoundaryJiangchiConvertedDistance(ProgramSlashConversionDistanceQuery q) => ActiveBoundaryJiangchi(q.Owner.Seat)is { Mode: "assault" };
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 55500)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 55500)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 55500)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 55500)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 55500)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 55500)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 55500)]
    internal bool BoundaryJiangchiSlashDistance(ProgramSlashDistanceQuery q) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && ActiveBoundaryJiangchi(q.Actor.Seat)is { Mode: "assault" };
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeIntrinsicUseBan, 55500)]
    internal bool BoundaryJiangchiUseBan(ProgramCardUseQuery q) => q.ActionType == CardActionType.Use && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && ActiveBoundaryJiangchi(q.ActorSeat)is { Mode: "restraint" };
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 55500)]
    internal void AssertBoundaryJiangchiFrame(ProgramSkillFrame f)
    {
        if (f.BoundaryJiangchi is { } r && !ValidBoundaryJiangchiPreparation(f, r))
            throw new InvalidOperationException("Jiangchi lost its original phase, source or prepared parent while a native child was suspended.");
    }
}
