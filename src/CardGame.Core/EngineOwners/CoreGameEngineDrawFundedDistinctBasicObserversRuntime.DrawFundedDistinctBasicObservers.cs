#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicObserversRuntime
{
    internal bool DrawFundedDistinctBasicFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => ride is DrawFundedDistinctBasicFrame frame && frame.Payment.RequestFrameId == beneath.Id && frame.Payment.ParentFrameId is { } parent && _resolutionStackOperations._resolutionStack.Any(f => f.Id == parent) && frame.OriginalDecision.PlayerSeat == frame.Payment.Source.OwnerSeat && frame.OriginalDecision.PromptId == frame.Payment.OriginalPromptId && frame.OriginalDecision.Revision == frame.Payment.OriginalRevision && _coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(frame.Payment);
    internal bool DrawFundedDistinctBasicInitialChild(DrawFundedDistinctBasicFrame frame, ResolutionFrame child) => frame.ActiveChildFrameId == child.Id && child switch
    {
        CardsMovedTriggerWindowFrame move => move.ResumeDrawFundedDistinctBasicFrameId == frame.Id && move.Batch.ParentFrameId == frame.Id && move.Batch.AwaitingProgramFrameId is null && move.ResumeProgramFrameId is null && move.Batch.Movements.Count > 0 && move.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > frame.Payment.SequenceBefore && m.Sequence <= frame.Payment.SequenceAfter && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.Payment.Source.OwnerSeat) && m.Reason.Value == "skill-program.draw-funded-distinct-basic.draw" || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle)),
        HpChangedTriggerWindowFrame hp => _coreDrawFundedDistinctBasicPaymentOperations.IsDrawFundedDistinctBasicHpParent(frame, hp),
        RecoveryReplacementFrame recovery => _coreDrawFundedDistinctBasicPaymentOperations.IsDrawFundedDistinctBasicRecoveryParent(frame, recovery),
        _ => false
    };
    internal bool DrawFundedDistinctBasicPaidSuffix(int paidIndex)
    {
        if (_resolutionStackOperations._resolutionStack[paidIndex] is not DrawFundedDistinctBasicFrame paid || !_coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(paid.Payment))
            return false;
        if (paidIndex == _resolutionStackOperations._resolutionStack.Count - 1)
            return paid.ActiveChildFrameId is null;
        if (!DrawFundedDistinctBasicInitialChild(paid, _resolutionStackOperations._resolutionStack[paidIndex + 1]))
            return false;
        for (var dyingIndex = paidIndex + 2; dyingIndex < _resolutionStackOperations._resolutionStack.Count; dyingIndex++)
            if (_resolutionStackOperations._resolutionStack[dyingIndex] is DyingFrame dying && DrawFundedDistinctBasicLegacyAlcoholRide(paid, dyingIndex, dying))
                return true;
        for (var index = paidIndex + 2; index < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            var child = _resolutionStackOperations._resolutionStack[index];
            var parent = _resolutionStackOperations._resolutionStack[index - 1];
            if (parent is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(index - 1, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(index - 1, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(index - 1, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(index - 1, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(index - 1, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(index - 1, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(index - 1, dying))))
                return true;
            if (_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(index) || _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(child, parent) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingRide(child, parent) || parent is DyingFrame entry && global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(child, parent, entry) || index >= 2 && _resolutionStackOperations._resolutionStack[index - 2] is DyingFrame originalEntry && global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(child, parent, originalEntry))
                continue;
            return false;
        }

        return true;
    }

    internal DrawFundedDistinctBasicFrame? DrawFundedDistinctBasicObserverRoot(long? requestedId = null)
    {
        for (var index = 0; index < _resolutionStackOperations._resolutionStack.Count; index++)
            if (_resolutionStackOperations._resolutionStack[index] is DrawFundedDistinctBasicFrame paid && paid.Stage == DrawFundedDistinctBasicStage.DrawChildren && (requestedId is null || paid.Payment.ParentFrameId == requestedId || _resolutionStackOperations._resolutionStack.Skip(index + 1).Any(f => f.Id == requestedId)) && DrawFundedDistinctBasicPaidSuffix(index))
                return paid;
        return null;
    }

    internal bool HasDrawFundedDistinctBasicObserver(long cardUseId) => DrawFundedDistinctBasicObserverRoot(cardUseId)is not null;
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 10)]
    internal bool HasDrawFundedDistinctBasicDamageObserver(long damageId) => DrawFundedDistinctBasicObserverRoot(damageId)is not null;
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 10, group: 80)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 120)]
    internal bool IsDrawFundedDistinctBasicProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && (DrawFundedDistinctBasicObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id) || _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId)is var index && index >= 0 && (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(index, (DyingFrame)_resolutionStackOperations._resolutionStack[index]) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(index, (DyingFrame)_resolutionStackOperations._resolutionStack[index])));
    internal PendingDecision? DrawFundedDistinctBasicInvariantDecision(PendingDecision? nativeDecision)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is DrawFundedDistinctBasicFrame paid)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime.DrawFundedDistinctBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime.RequestedDeckBasicNativeDecision(paid.OriginalDecision));
        return nativeDecision is { } prompt ? global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime.DrawFundedDistinctBasicNativeDecision(prompt) : null;
    }

    internal ResolutionFrame? DrawFundedDistinctBasicInvariantTop(ResolutionFrame? nativeTop) => _resolutionStackOperations._resolutionStack.LastOrDefault()is DrawFundedDistinctBasicFrame paid ? _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == paid.Payment.RequestFrameId) : nativeTop;
    internal void AssertDrawFundedDistinctBasicFramesAndUses()
    {
        foreach (var paid in _resolutionStackOperations._resolutionStack.OfType<DrawFundedDistinctBasicFrame>())
        {
            var p = paid.Payment;
            var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == paid.Id);
            if (p.PaymentFrameId != paid.Id || paid.Stage != DrawFundedDistinctBasicStage.DrawChildren || !_coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(p) || paid.OriginalDecision.PlayerSeat != p.Source.OwnerSeat || paid.OriginalDecision.PromptId != p.OriginalPromptId || paid.OriginalDecision.Revision != p.OriginalRevision || paid.TargetSeats.Count == 0 && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(p.EffectiveKind) || paid.TargetSeats.Distinct().Count() != paid.TargetSeats.Count || paid.TargetSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || p.Intent != DrawFundedDistinctBasicIntent.Play && (index <= 0 || !DrawFundedDistinctBasicFrameRidesOn(paid, _resolutionStackOperations._resolutionStack[index - 1])) || p.Intent == DrawFundedDistinctBasicIntent.Play && index != 0 && !_characterKeBiNengOlOperations.IsKeBiNengPingduanPaymentFrame(paid) && !(_characterKongRongMouOlOperations.IsMouKrLiwenPaymentFrame(paid) || _characterMaTieAnniversaryOperations.IsAnniversaryMaTiePaymentFrame(paid)) && !_characterLuSuMouOperations.IsMouLuSuPaymentFrame(paid) || !DrawFundedDistinctBasicPaidSuffix(index) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawFundedDistinctBasicIssuedEvent>().Any(e => e.PaymentFrameId == p.PaymentFrameId))
                throw new InvalidOperationException("A paid basic-card attempt lost its owning frame, frozen original decision or exact child subtree.");
        }

        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
        {
            if (use.DrawFundedDistinctBasicUse is not null && !_coreDrawFundedDistinctBasicUsesOperations.IsIssuedDrawFundedDistinctBasicUse(use))
                throw new InvalidOperationException("A paid basic Use lost its immutable issuance and unique native draw.");
            if (use.DrawFundedDistinctBasicResponse is not null)
                _coreDrawFundedDistinctBasicResponsesOperations.RequireDrawFundedDistinctBasicResponseAction(use);
            if (use.Action?.ConversionChain.Any(s => _corePlayCardHistoryOperations.ViewAsRule(s)?.DrawFundedDistinctBasic is not null) == true && use.DrawFundedDistinctBasicUse is null)
                throw new InvalidOperationException("A new-policy Use cannot disguise a deleted owning receipt as a legacy zero-card action.");
        }

        foreach (var window in _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Where(w => w.Action.Type == CardActionType.Response && w.Action.ConversionChain.Any(s => _corePlayCardHistoryOperations.ViewAsRule(s)?.DrawFundedDistinctBasic is not null)))
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { DrawFundedDistinctBasicResponse: not null } owner || !global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime.TieredRoundActionsStructurallyMatch(_coreDrawFundedDistinctBasicResponsesOperations.RequireDrawFundedDistinctBasicResponseAction(owner), window.Action))
                throw new InvalidOperationException("A paid Dodge observer lost its exact original accepted response.");
    }

    internal bool DrawFundedDistinctBasicLegacyAlcoholRide(DrawFundedDistinctBasicFrame paid, int dyingIndex, DyingFrame dying)
    {
        var paidIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == paid.Id);
        if (paidIndex < 0 || paidIndex + 1 >= dyingIndex || dyingIndex + 3 >= _resolutionStackOperations._resolutionStack.Count || !_coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(paid.Payment) || !DrawFundedDistinctBasicInitialChild(paid, _resolutionStackOperations._resolutionStack[paidIndex + 1]) || _resolutionStackOperations._resolutionStack[dyingIndex].Id != dying.Id || _coreDyingCursorRuntimeOperations.ActiveDying?.FrameId != dying.Id || dying.Continuation != DyingContinuationKind.ProgramSkill || dying.KillerSeat is not null || dying.ResponderIndex < 0 || dying.ResponderIndex >= dying.ResponderSeats.Count || _resolutionStackOperations._resolutionStack[dyingIndex - 1] is not ProgramSkillFrame losing || losing.Id != dying.ParentFrameId || _resolutionStackOperations._resolutionStack[dyingIndex + 1] is not ProgramSkillFrame program || program.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dying.Id || context.TargetSeat != dying.VictimSeat || context.SourceSeat is not null || context.DamageFrameId is not null || context.OwnerSeat != program.OwnerSeat || context.OccurrenceIndex != 0 || program.OwnerSeat != dying.VictimSeat || program.OwnerSeat != dying.ResponderSeat || string.IsNullOrWhiteSpace(program.TriggerId) || string.IsNullOrWhiteSpace(program.SkillInstanceId) || program.ActivationId != program.TriggerId || program.InstructionIndex < 1 || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(program.SkillId)?.Program is not { } definition || definition.GameplayHash != program.GameplayHash || definition.Triggers.SingleOrDefault(trigger => trigger.Id == program.TriggerId)?.Window != context.Window)
            return false;
        for (var index = paidIndex + 2; index <= dyingIndex; index++)
        {
            var child = _resolutionStackOperations._resolutionStack[index];
            var parent = _resolutionStackOperations._resolutionStack[index - 1];
            if (!(_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(index) || _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(child, parent) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingRide(child, parent) || parent is DyingFrame entry && global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(child, parent, entry) || index >= 2 && _resolutionStackOperations._resolutionStack[index - 2] is DyingFrame originalEntry && global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(child, parent, originalEntry)))
                return false;
        }

        var plan = ProgramInstructionResolver.Default.Resolve(program, definition);
        if (program.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(program.InstructionIndex).Effect is not { Op: SkillProgramEffectOp.UseVirtualDyingAlcohol, Target: SkillProgramEffectTarget.Owner } || _resolutionStackOperations._resolutionStack[dyingIndex + 2] is not CardUseFrame { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, Action: not null, LegacyDyingAlcoholReturn: { } returned, Step: ResolutionFrameStep.ResolvingEffect } use || use.SourceSeat != dying.VictimSeat || !use.TargetSeats.SequenceEqual([dying.VictimSeat]) || use.DyingResponse is not null || use.VirtualBasicReturn is not null || returned.ProgramFrameId != program.Id || returned.InstructionIndex != program.InstructionIndex || returned.ProducerSource != new CardConversionSource(program.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(program), program.OwnerSeat, program.SkillInstanceId) || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use) || _resolutionStackOperations._resolutionStack[dyingIndex + 3] is not HpChangedTriggerWindowFrame hp || hp.Id != hp.Change.Id || hp.ResumeFrameId != use.Id || hp.Change.ParentFrameId != use.Id || hp.Continuation != PostEventContinuation.CardUse || hp.CardId != 0 || hp.CardKind != CardKind.Alcohol || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.SourceSeat != dying.VictimSeat || hp.Change.TargetSeat != dying.VictimSeat || hp.Change.Amount != 1 || hp.Change.HpBefore > 0 || hp.Change.HpAfter != hp.Change.HpBefore + 1 || hp.Candidates.Count == 0 || hp.Candidates.Count != hp.Contexts.Count || hp.CandidateIndex < 0 || hp.CandidateIndex > hp.Candidates.Count)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        // A paid rescue keeps its accepted source identity even if a child later
        // disables that skill. Re-enumerating live eligibility would revoke it.
        if (history.OfType<ProgramBindingStartedEvent>().Count(fact => fact.FrameId == program.Id && fact.SkillId == program.SkillId && fact.BindingId == program.TriggerId && fact.SkillInstanceId == program.SkillInstanceId && fact.OwnerSeat == program.OwnerSeat && fact.Window == context.Window) != 1 || history.OfType<PlayerDyingEvent>().Count(fact => fact.ResolutionId == dying.Id && fact.VictimSeat == dying.VictimSeat && fact.KillerSeat is null) != 1 || history.OfType<CardUseDeclaredEvent>().Count(fact => fact.ResolutionId == use.Id && fact.CardId == 0 && fact.CardKind == CardKind.Alcohol && fact.SourceSeat == dying.VictimSeat) != 1 || history.OfType<TargetsConfirmedEvent>().Count(fact => fact.ResolutionId == use.Id && fact.TargetSeats.SequenceEqual([dying.VictimSeat])) != 1 || history.OfType<ProgramDyingRescueEvent>().Count(fact => fact.DyingFrameId == dying.Id && fact.SkillId == program.SkillId && fact.OwnerSeat == program.OwnerSeat && fact.VictimSeat == dying.VictimSeat && fact.CardId == 0 && fact.RecoveredHp == 1 && fact.VictimHp == hp.Change.HpAfter) != 1)
            return false;
        for (var index = 0; index < hp.Candidates.Count; index++)
        {
            var candidate = hp.Candidates[index];
            var hpContext = hp.Contexts[index];
            if (candidate.OwnerSeat != dying.VictimSeat || hpContext.OwnerSeat != candidate.OwnerSeat || hpContext.ParentFrameId != hp.Id || hpContext.HpChange != hp.Change || hpContext.SourceSeat != hp.Change.SourceSeat || hpContext.TargetSeat != hp.Change.TargetSeat || hpContext.Amount != hp.Change.Amount || hpContext.OccurrenceIndex != candidate.OccurrenceIndex || hpContext.Window is not (SkillProgramTriggerWindow.AfterHpRecovered or SkillProgramTriggerWindow.AfterHealthChanged) || _coreProgramLifecycleOperations.GetProgramTrigger(candidate).Window != hpContext.Window)
                return false;
        }

        if (dyingIndex + 4 == _resolutionStackOperations._resolutionStack.Count)
            return true;
        return dyingIndex + 5 == _resolutionStackOperations._resolutionStack.Count && hp.CandidateIndex < hp.Candidates.Count && _resolutionStackOperations._resolutionStack[dyingIndex + 4] is ProgramSkillFrame observer && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(observer, hp.Candidates[hp.CandidateIndex]) && observer.WindowContext == hp.Contexts[hp.CandidateIndex];
    }
}
