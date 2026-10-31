#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGaoGanOlRuntime
{
    internal bool TracksJuguanDrawObligations => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ObserveJuguanDrawObligation);

    internal bool IsJuguanConversion(CardConversionSource source) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(source.SkillId)?.Program is { } program && program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ObserveJuguanDrawObligation)) && program.ViewAs.Any(v => v.Id == source.BindingId && v.UsesPerPhase == 1 && v.UsageGroup == "juguan" && v.InputCount == 1 && v.SourceZones.SequenceEqual([CardZoneKind.Hand]) && v.ForPlay && !v.ForResponse && v.OutputKind is CardKind.Slash or CardKind.Duel);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26200)]
    internal void ObserveJuguanActualUseAndDamage(IGameEvent payload)
    {
        if (!TracksJuguanDrawObligations)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { } use && use.Action is { Type: CardActionType.Use, PhysicalCards.Count: 1 } action && action.ConversionChain.FirstOrDefault(IsJuguanConversion)is { } source)
        {
            if (use.JuguanUse is not null || action.ActorSeat != source.OwnerSeat || action.ProviderSeat != source.OwnerSeat || action.PhysicalCards[0].From != CardLocation.Hand(source.OwnerSeat) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != source.OwnerSeat || declared.CardId != use.CardId || declared.CardKind != use.CardKind || declared.SourceSeat != source.OwnerSeat)
                throw new InvalidOperationException("Juguan needs its actual native one-hand-card phase-limited use and original source.");
            var receipt = new JuguanUseReceipt(use.Id, action.ActionId, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, action.PhysicalCards[0].CardId, action.EffectiveKind);
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { JuguanUse = receipt });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JuguanUseIssuedEvent(receipt));
            return;
        }

        if (payload is not DamageAppliedEvent { Amount: > 0 } applied || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || damage.ParentFrameId != attack.ResolutionId || damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount || damage.Nature != applied.Nature || attack.SourceSeat != applied.SourceSeat || attack.TargetSeat != applied.TargetSeat || attack.IsSourceLess != applied.SourceLess || !attack.DamageWasApplied || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageRequestedEvent>().Any(e => e.ResolutionId == damage.Id && e.SourceSeat == applied.SourceSeat && e.TargetSeat == applied.TargetSeat && e.Amount == applied.Amount && e.Nature == applied.Nature && e.SourceLess == applied.SourceLess))
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { JuguanUse: { } r } actual && !applied.SourceLess)
        {
            RequireJuguanUse(actual);
            if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanVictimDamagedEvent>().Any(e => e.DamageFrameId == damage.Id && e.CardUseFrameId == actual.Id))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JuguanVictimDamagedEvent(actual.Id, r.CardActionId, r.Source.OwnerSeat, applied.TargetSeat, damage.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, applied.SourceSeat, applied.Amount));
        }

        if (applied.SourceLess)
            return;
        // Publishing a retaliation fact reenters observers and appends to the
        // same pending history. Freeze this qualified set before the first fact.
        foreach (var v in PendingJuguanVictims(applied.TargetSeat).Where(v => v.VictimSeat == applied.SourceSeat && v.DamageFrameId != damage.Id).DistinctBy(v => (v.CardUseFrameId, v.OwnerSeat, v.VictimSeat)).ToArray())
            if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanRetaliationRecordedEvent>().Any(e => e.CardUseFrameId == v.CardUseFrameId && e.OwnerSeat == v.OwnerSeat && e.VictimSeat == v.VictimSeat))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JuguanRetaliationRecordedEvent(v.CardUseFrameId, v.OwnerSeat, v.VictimSeat, damage.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
    }

    internal IEnumerable<JuguanVictimDamagedEvent> PendingJuguanVictims(int owner)
    {
        var lastDeadline = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanDeadlineSettledEvent>().Where(e => e.OwnerSeat == owner).Select(e => e.ActualTurn).DefaultIfEmpty().Max();
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanVictimDamagedEvent>().Where(e => e.OwnerSeat == owner && e.ActualTurn >= lastDeadline);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26201)]
    internal void ObserveJuguanDeadlineAndActualDraw(IGameEvent payload)
    {
        if (!TracksJuguanDrawObligations)
            return;
        if (payload is TurnStartedEvent start && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanUseIssuedEvent>().Any(e => e.Receipt.Source.OwnerSeat == start.ActorSeat))
        {
            var qualified = PendingJuguanVictims(start.ActorSeat).Any(v => !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanRetaliationRecordedEvent>().Any(r => r.CardUseFrameId == v.CardUseFrameId && r.OwnerSeat == v.OwnerSeat && r.VictimSeat == v.VictimSeat));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JuguanDeadlineSettledEvent(start.ActorSeat, start.TurnNumber, qualified));
        }

        if (payload is DrawPhaseSkippedEvent skipped && CurrentJuguanDeadline(skipped.OwnerSeat)is { Qualified: true } deadline && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanSkippedDrawExpiredEvent>().Any(e => e.OwnerSeat == skipped.OwnerSeat && e.ActualTurn == deadline.ActualTurn))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JuguanSkippedDrawExpiredEvent(skipped.OwnerSeat, skipped.ActualTurnNumber, skipped.FrameId));
        if (payload is ActualDiscardRecoveryPhaseStartedEvent { Phase.Kind: ActualDiscardRecoveryPhaseKind.Draw } draw && CurrentJuguanDeadline(draw.Phase.ActorSeat)is { Qualified: true } ready && ready.ActualTurn == draw.Phase.TurnNumber && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanDrawPhaseBoundEvent>().Any(e => e.OwnerSeat == draw.Phase.ActorSeat && e.DeadlineTurn == ready.ActualTurn) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanSkippedDrawExpiredEvent>().Any(e => e.OwnerSeat == draw.Phase.ActorSeat && e.ActualTurn == ready.ActualTurn))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JuguanDrawPhaseBoundEvent(draw.Phase.ActorSeat, ready.ActualTurn, draw.Phase));
    }

    internal JuguanDeadlineSettledEvent? CurrentJuguanDeadline(int owner) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanDeadlineSettledEvent>().LastOrDefault(e => e.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
    internal int JuguanActualDrawAdjustment(CharacterState player)
    {
        if (!TracksJuguanDrawObligations || !player.IsAlive || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase || phase.ActorSeat != player.Seat || CurrentJuguanDeadline(player.Seat)is not { Qualified: true } ready)
            return 0;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanDrawPhaseBoundEvent>().Count(e => e.OwnerSeat == player.Seat && e.DeadlineTurn == ready.ActualTurn && e.Phase == phase) == 1 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanSkippedDrawExpiredEvent>().Any(e => e.OwnerSeat == player.Seat && e.ActualTurn == ready.ActualTurn) ? 2 : 0;
    }

    internal void RequireJuguanUse(CardUseFrame use)
    {
        if (use.JuguanUse is not { } r || r.CardUseFrameId != use.Id || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 1 } action || action.ActionId != r.CardActionId || !action.ConversionChain.Contains(r.Source) || action.PhysicalCards[0].CardId != r.MaterialCardId || action.PhysicalCards[0].From != CardLocation.Hand(r.Source.OwnerSeat) || r.ActualTurn > _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.PhaseInstance < 1 || !IsJuguanConversion(r.Source) || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash != r.GameplayHash || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanUseIssuedEvent>().Where(e => e.Receipt.CardUseFrameId == use.Id).ToArray()is not [var fact] || fact.Receipt != r)
            throw new InvalidOperationException("Juguan lost its exact issued native use, one real hand entity, source hash or actual action identity.");
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 26200)]
    internal void AssertJuguanUses()
    {
        if (!TracksJuguanDrawObligations)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.JuguanUse is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JuguanUseIssuedEvent>().Any(e => e.Receipt.CardUseFrameId == u.Id)))
            RequireJuguanUse(use);
    }
}
