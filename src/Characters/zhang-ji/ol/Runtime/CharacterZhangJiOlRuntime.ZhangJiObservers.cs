#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangJiOlRuntime
{
    internal bool ValidZhangJi(ProgramSkillFrame f)
    {
        if (f.ZhangJi is not { } r || r.Source != ZhangJiSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || r.Turn != _coreActualTurnProgressTurnNumberAccess._turnNumber || r.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || f.TriggerId is not null || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || target != r.Target || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || r.Before > r.After || r.HistoricalCount < 0 || r.Slots.Distinct().Count() != r.Slots.Count || r.Slots.Count > r.HistoricalCount || r.Stage is not ("rank" or "judgment" or "outcome" or "movement" or "damage" or "equipment" or "issuing-equipment" or "equipment-issued"))
            return false;
        if (ZhangJiPausedEffect(f)?.Op != (r.Equipment ? SkillProgramEffectOp.UseDistinctDeckEquipmentByLueMingHistory : SkillProgramEffectOp.ResolveTargetChosenRankJudgment))
            return false;
        var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var starts = events.OfType<ZhangJiStartedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (starts is not [var start] || start != new ZhangJiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Turn, r.PhaseInstance, target, r.Equipment, r.HistoricalCount))
            return false;
        var prior = events.TakeWhile(e => !ReferenceEquals(e, start)).OfType<ZhangJiStartedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && !e.Equipment);
        if (prior != r.HistoricalCount)
            return false;
        if (r.Equipment)
        {
            if (r.ChosenRank != 0 || r.JudgmentId is not null || r.JudgmentReturned || r.FinalRank is not null || r.GainCard is not null)
                return false;
            var uses = events.OfType<ZhangJiNativeUseIssuedEvent>().Where(e => e.Origin.ProgramFrameId == f.Id).Select(e => e.Origin).ToArray();
            if (uses.Any(o => o.Source != r.Source || o.GameplayHash != r.GameplayHash || o.Turn != r.Turn || o.PhaseInstance != r.PhaseInstance || o.InstructionIndex != f.InstructionIndex || o.Actor != target || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(o.Kind) || _coreEquipmentContentOperations.EquipmentRules.Get(o.Kind).Slot != o.Slot) || !uses.Select(o => o.Slot).SequenceEqual(r.Slots.Take(uses.Length)))
                return false;
            var returned = events.OfType<ZhangJiNativeUseReturnedEvent>().Where(e => e.Origin.ProgramFrameId == f.Id).ToArray();
            if (returned.Any(e => uses.Count(o => o == e.Origin) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > e.Origin.Before && m.Sequence <= e.After && m.CardId == e.Origin.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing) != 1) || returned.Select(e => e.Origin.UseId).Distinct().Count() != returned.Length)
                return false;
            if (r.NativeUse is { } currentOrigin)
                return uses.LastOrDefault() == currentOrigin && returned.Count(e => e.Origin == currentOrigin) == (r.NativeReturned ? 1 : 0) && uses.Length == r.Slots.Count;
            return !r.NativeReturned && (r.Stage == "issuing-equipment" ? uses.Length + 1 == r.Slots.Count : returned.Length == uses.Length && uses.Length == r.Slots.Count);
        }

        if (r.Slots.Count != 0 || r.NativeUse is not null || r.NativeReturned || target == f.OwnerSeat)
            return false;
        var ranks = events.OfType<ZhangJiRankChosenEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == "rank")
            return r.ChosenRank == 0 && r.JudgmentId is null && ranks.Length == 0 && !r.JudgmentReturned;
        if (r.ChosenRank is < 1 or > 13 || r.JudgmentId is not { } judgment || ranks is not [var rank] || rank != new ZhangJiRankChosenEvent(f.Id, target, r.ChosenRank, judgment))
            return false;
        var results = events.OfType<ZhangJiJudgmentReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (!r.JudgmentReturned)
            return r.Stage == "judgment" && results.Length == 0;
        if (results is not [var result] || result != new ZhangJiJudgmentReturnedEvent(f.Id, judgment, r.JudgmentCard, r.FinalRank) || events.OfType<JudgmentResolvedEvent>().Count(e => e.ResolutionId == judgment && e.ParentResolutionId == f.Id && e.TargetSeat == f.OwnerSeat && e.Reason == "lueming" && e.CardId == r.JudgmentCard && e.Rank == r.FinalRank) != 1)
            return false;
        var gains = events.OfType<ZhangJiCardGainedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == "movement")
            return r.FinalRank != r.ChosenRank && r.GainCard is { } card && r.GainFrom is { } from && from.OwnerSeat == target && from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment && gains is [var gain] && gain == new ZhangJiCardGainedEvent(f.Id, target, f.OwnerSeat, r.Before, r.After) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == card && m.From == from && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZhangJiGainReason) == 1;
        return gains.Length == 0 && (r.Stage != "damage" || r.FinalRank == r.ChosenRank && events.OfType<ZhangJiDamageIssuedEvent>().Count(e => e.FrameId == f.Id && e.Source == f.OwnerSeat && e.Target == target && e.Amount == 2) == 1);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 250)]
    internal void AssertZhangJi(ProgramSkillFrame f)
    {
        if (f.ZhangJi is null)
            return;
        if (!ValidZhangJi(f))
            throw new InvalidOperationException("Zhang Ji lost its original binding, rank, native judge result, movement or distinct equipment invoices.");
        if (ReferenceEquals(f, _resolutionStackOperations._resolutionStack.LastOrDefault()) && f.ZhangJi.Stage == "rank" && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } p && (p.PlayerSeat != f.ZhangJi.Target || !p.IsPrivate || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, ZhangJiChoices(f))))
            throw new InvalidOperationException("Lueming cannot expose the rank choice to a different actor.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 210)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 210)]
    internal bool CanContinueZhangJi(ProgramSkillFrame f) => ValidZhangJi(f);
    internal bool ZhangJiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhangJi is not { } r || !ValidZhangJi(f))
            return false;
        if (child is JudgmentFrame j)
            return IsZhangJiJudgment(j, f);
        if (child is CardUseFrame use)
            return ZhangJiCardUseRidesOn(use, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == "movement" && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count == 1 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.GainCard);
        if (child is ProgramLifecycleTriggerWindowFrame skills)
            return skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return r.Stage == "damage" && f.AttackAttempt is { } a && f.AttackReturn is not null && a.SourceSeat == f.OwnerSeat && a.TargetSeat == r.Target && a.DamageAmount == 2 && !a.SourceLess && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
    }

    internal ProgramSkillFrame? ZhangJiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhangJiOlOperations.ZhangJiFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 140)]
    internal bool IsZhangJiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ZhangJiObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 170)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 280)]
    internal bool HasZhangJiDamageObserver(long id) => _resolutionStackOperations._resolutionStack.Any(x => x.Id == id && x is DamageTriggerWindowFrame) && ZhangJiObserverRoot()is not null;
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 370)]
    internal bool HasZhangJiBeforeDamageObserver(long id) => _resolutionStackOperations._resolutionStack.Any(x => x.Id == id && x is BeforeDamageProgramWindowFrame) && ZhangJiObserverRoot()is not null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 280)]
    internal bool TryAdvanceZhangJiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhangJiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
