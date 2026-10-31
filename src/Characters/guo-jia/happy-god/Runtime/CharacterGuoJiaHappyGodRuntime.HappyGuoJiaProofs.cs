#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaHappyGodRuntime
{
    internal bool ValidHappyGj(ProgramSkillFrame f)
    {
        if (f.HappyGuoJia is not { } r || r.Source != HappyGjSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || f.TriggerId is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Turn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || f.InstructionIndex != (r.Operation == SkillProgramEffectOp.ResolveHappyGuoJiaHuishi ? 2 : 1) || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards.Count > 5 || r.Cards.Distinct().Count() != r.Cards.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenGuoJiaStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new ShenGuoJiaStartedEvent(f.Id, r.Source, f.GameplayHash, r.Operation, r.Turn, r.TurnOwner, 0, -1) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenGuoJiaCompletedEvent>().Any(e => e.FrameId == f.Id))
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<ShenGuoJiaMaximumPaidEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (paid.Where((e, i) => e.Index != i || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(e.Seat) || e.Before < 0 || e.After < 0).Any())
            return false;
        if (paid.Length > 0 && (r.MaximumBefore != paid[^1].Before || r.MaximumAfter != paid[^1].After))
            return false;
        if (r.Operation == SkillProgramEffectOp.ResolveHappyGuoJiaHuishi)
        {
            if (f.SkillId != HappyGjHuishi || r.Plan is not null || r.Benefactor != -1 || r.Stage is not (HappyGuoJiaStage.GrowChildren or HappyGuoJiaStage.Recipient or HappyGuoJiaStage.GainChildren or HappyGuoJiaStage.CleanupChildren or HappyGuoJiaStage.CostChildren) || f.CardSetBindings.SingleOrDefault(b => b.Name == "huishi-five")is not { } b || !r.Cards.SequenceEqual(b.CardIds) || b.Visibility != SkillProgramCardSetVisibility.Public || b.SourceLocations.Any(l => l != CardLocation.Processing) || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardsRevealedEvent>().Where(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.BindingId == f.ActivationId && e.Bind == b.Name).ToArray()is not [var shown] || !shown.Cards.Select(c => c.Id).SequenceEqual(r.Cards) || r.SuitCount != shown.Cards.Select(c => c.Suit).Where(s => s != Suit.None).Distinct().Count())
                return false;
            if (paid.Length is < 1 or > 2 || paid[0].Purpose != "happy-huishi-grow" || paid[0].Seat != f.OwnerSeat || paid.Length == 2 && (paid[1].Purpose != "happy-huishi-most-hand" || paid[1].Seat != f.OwnerSeat || r.Stage != HappyGuoJiaStage.CostChildren))
                return false;
            return r.Stage != HappyGuoJiaStage.GainChildren || _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target);
        }

        if (r.Cards.Count != 0 || r.SuitCount != 0)
            return false;
        if (r.Operation == SkillProgramEffectOp.ResolveHappyGuoJiaHuishiGrant)
            return f.SkillId == HappyGjGift && r.Plan is null && r.Benefactor == -1 && r.Stage is HappyGuoJiaStage.Target or HappyGuoJiaStage.Awakening or HappyGuoJiaStage.Drawing or HappyGuoJiaStage.DrawChildren or HappyGuoJiaStage.CostChildren && (r.Stage == HappyGuoJiaStage.Target ? r.Target == -1 && paid.Length == 0 : _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target)) && (r.Stage != HappyGuoJiaStage.CostChildren || paid is [{ Purpose: "happy-huishi-gift" }] && (r.DrawOutcome is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenGuoJiaAwakeningOverrideEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.Recipient == r.Target) == 1));
        if (r.Operation != SkillProgramEffectOp.ResolveHappyGuoJiaZuoxing || f.SkillId != HappyGjZuoxing || r.Stage is not (HappyGuoJiaStage.UseChoice or HappyGuoJiaStage.CostChildren or HappyGuoJiaStage.IssuingUse or HappyGuoJiaStage.NativeUse or HappyGuoJiaStage.ReturnedUse))
            return false;
        if (r.Stage == HappyGuoJiaStage.UseChoice)
            return r.Plan is null && r.Benefactor == -1 && paid.Length == 0;
        if (r.Plan is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Benefactor) || paid is not [{ Purpose: "happy-zuoxing" } cost] || cost.Seat != r.Benefactor || !_coreProgramEventHistoryOperations.ProgramEventHistory<HappyGuoJiaBenefactorIssuedEvent>().Any(e => e.Recipient == f.OwnerSeat && e.GrantedInstance == f.SkillInstanceId && e.Source.OwnerSeat == r.Benefactor && e.FrameId < f.Id))
            return false;
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<ShenGuoJiaNativeIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage is HappyGuoJiaStage.CostChildren or HappyGuoJiaStage.IssuingUse)
            return issued.Length == 0 && r.UseId == 0 && r.ActionId == 0;
        return issued is [var use] && use.UseId == r.UseId && use.ActionId == r.ActionId && use.Kind == r.Plan.Kind && use.Benefactor == r.Benefactor && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == r.UseId && e.CardId == 0 && e.CardKind == r.Plan.Kind) == 1;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 40900)]
    internal void AssertHappyGuoJia(ProgramSkillFrame f)
    {
        if (f.HappyGuoJia is not { } r)
            return;
        if (!ValidHappyGj(f) || r.Cards is not System.Collections.IList { IsReadOnly: true } || r.Plan is { Targets: not System.Collections.IList { IsReadOnly: true } })
            throw new InvalidOperationException("Happy Guo Jia lost its exact original source, public reveal, paid maximum, native use or immutable receipt.");
        if (f.PendingMovementContinuation is { } pending && !HappyGjMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            throw new InvalidOperationException("Happy Guo Jia lost its exact movement parent.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } p && r.Stage is HappyGuoJiaStage.Recipient or HappyGuoJiaStage.Target or HappyGuoJiaStage.Awakening or HappyGuoJiaStage.UseChoice && (p.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, HappyGjChoices(f))))
            throw new InvalidOperationException("Happy Guo Jia changed its published owner menu.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 40900)]
    internal bool HappyGjMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.HappyGuoJia is { } r && e?.Op == r.Operation && f.PendingMovementContinuation == p && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is HappyGuoJiaStage.GrowChildren or HappyGuoJiaStage.GainChildren or HappyGuoJiaStage.CleanupChildren or HappyGuoJiaStage.DrawChildren or HappyGuoJiaStage.CostChildren;
    internal bool HappyGjFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidHappyGj(f) || f.HappyGuoJia is not { } r)
            return false;
        if (child is CardUseFrame use)
            return use.VirtualOrdinaryTrickOrigin?.ParentProgramFrameId == f.Id && IsHappyGuoJiaVirtualUse(use);
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == HappyGuoJiaStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == r.Target && draw.Context.RequestedCount == 4;
        if (child is HpChangedTriggerWindowFrame hp)
            return f.PendingMovementContinuation is not null && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.MaximumHp && hp.Change.TargetSeat == (r.Operation == SkillProgramEffectOp.ResolveHappyGuoJiaZuoxing ? r.Benefactor : f.OwnerSeat) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenGuoJiaMaximumPaidEvent>().Any(e => e.FrameId == f.Id && e.Seat == hp.Change.TargetSeat && Math.Abs(e.After - e.Before) == hp.Change.Amount);
        if (child is DeathFrame death)
            return r.Stage == HappyGuoJiaStage.CostChildren && r.MaximumAfter == 0 && death.ParentFrameId == f.Id && death.VictimSeat == f.OwnerSeat && death.ReturnKind == DeathReturnKind.ProgramSkill;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.Reason == CardMoveReasons.Reshuffle || r.Stage == HappyGuoJiaStage.DrawChildren && m.Reason.Value == "skill-program.happy:huishi-gift.draw" && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Target) || r.Cards.Contains(m.CardId) && m.From == CardLocation.Processing && (r.Target >= 0 && m.Reason.Value == "skill-program.happy:huishi.give" && m.To == CardLocation.Hand(r.Target) || m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.happy:huishi.cleanup")));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? HappyGjObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterGuoJiaHappyGodOperations.HappyGjFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 40900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 40900)]
    internal bool HappyGjDamageObserver(long id) => HappyGjObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(c => c.Id == id && c is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(c => c.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 40900)]
    internal bool HappyGjBeforeDamageObserver(long id) => HappyGjObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(c => c.Id == id && c is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(c => c.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 40900)]
    internal bool HappyGjDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && HappyGjObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(c => c.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(c => c.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 40900)]
    internal bool AdvanceHappyGjSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HappyGjObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
