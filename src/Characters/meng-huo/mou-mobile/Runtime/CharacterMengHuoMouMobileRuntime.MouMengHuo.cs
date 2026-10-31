#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMengHuoMouMobileRuntime
{
    internal bool HasMouMengHuoCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouMengHuoZaiqi);

    internal static CardConversionSource MouMhSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_meng_huo_mou_mobile_MouMengHuo.MouMhSource(f);
    internal int MouMhCharge(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].Markers.GetValueOrDefault(PlayerMarkerKind.MouMengHuoCharge);
    internal MouMengHuoTurnKey? MouMhTurn()
    {
        var h = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return h.LastOrDefault(e => e is TurnStartedEvent or ActualTurnCursorReturnedEvent)is TurnStartedEvent t && t.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && t.ActorSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat ? new(t.TurnNumber, t.ActorSeat, h.OfType<TurnStartedEvent>().Count()) : null;
    }

    internal void ChangeMouMhCharge(ProgramSkillFrame f, int delta)
    {
        var before = MouMhCharge(f.OwnerSeat);
        var count = checked(before + delta);
        if (count is < 0 or > 7)
            throw new InvalidOperationException("Zaiqi's actual charge must remain between zero and seven.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var key = (PlayerMarkerKind.MouMengHuoCharge, f.OwnerSeat);
        if (count == 0)
        {
            owner.Markers.Remove(PlayerMarkerKind.MouMengHuoCharge);
            owner.MarkerSourceCounts.Remove(key);
        }
        else
        {
            owner.Markers[PlayerMarkerKind.MouMengHuoCharge] = count;
            owner.MarkerSourceCounts[key] = count;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, f.OwnerSeat, PlayerMarkerKind.MouMengHuoCharge, delta, count, f.OwnerSeat, f.SkillId));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 30900)]
    internal void CaptureMouMhUse(CardUseDeclaredEvent e)
    {
        if (!HasMouMengHuoCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(e.ResolutionId)is not { Action: { Type: CardActionType.Use, EffectiveKind: CardKind.BarbarianAssault } a } || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != a.ActorSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoBarbarianUsedEvent>().Any(x => x.UseId == e.ResolutionId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoBarbarianUsedEvent(a.ActorSeat, phase, e.ResolutionId, a.ActionId));
    }

    internal bool IsMouMengHuoUseForbidden(int actor, CardKind kind, CardActionType type, long? existingUseId)
    {
        if (!HasMouMengHuoCapability || kind != CardKind.BarbarianAssault || type != CardActionType.Use || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[actor], MouMengHuoComposition.Huoshou) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != actor)
            return false;
        if (existingUseId is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { Type: CardActionType.Use, EffectiveKind: CardKind.BarbarianAssault } a } && a.ActorSeat == actor && _coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoBarbarianUsedEvent>().Any(e => e.UseId == id && e.ActionId == a.ActionId && e.ActorSeat == actor && e.Phase == phase))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoBarbarianUsedEvent>().Any(e => e.ActorSeat == actor && e.Phase == phase);
    }

    internal bool RestoreMouMengHuoDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group)
    {
        if (!HasMouMengHuoCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(group.ResolutionId)is not { CardKind: CardKind.BarbarianAssault, Action: { } a, MouMengHuoDamageSource: { } r } use || r.UseId != use.Id || r.ActionId != a.ActionId || r.OriginalActorSeat != a.ActorSeat || !_coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoAttributionIssuedEvent>().Any(e => e.Attribution == r))
            return false;
        group.DamageSourceSeat = r.Source.OwnerSeat;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuoshouAttributedEvent(use.Id, a.ActorSeat, r.Source.OwnerSeat));
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30900, SkillProgramEffectOp.ResolveMouMengHuoAttribution, SkillProgramEffectOp.ObtainMouMengHuoBarbarian, SkillProgramEffectOp.GainMouMengHuoCharge, SkillProgramEffectOp.ResolveMouMengHuoZaiqi)]
    internal bool CanOfferMouMh(ProgramTriggerLegalityContext g)
    {
        if (g.Features.HasOperation(SkillProgramEffectOp.ResolveMouMengHuoAttribution))
            return g.Context.CardUse is { } c && c.ActorSeat != g.OwnerSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(c.ParentCardUseFrameId)is { CardKind: CardKind.BarbarianAssault, MouMengHuoDamageSource: null };
        if (g.Features.HasOperation(SkillProgramEffectOp.ObtainMouMengHuoBarbarian))
            return _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == g.OwnerSeat && _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Any(c => c.Kind == CardKind.BarbarianAssault);
        if (g.Features.HasOperation(SkillProgramEffectOp.GainMouMengHuoCharge))
            return g.Context.SourceSeat == g.OwnerSeat && g.Context.Amount > 0 && MouMhCharge(g.OwnerSeat) < 7 && MouMhTurn()is { } turn && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoChargeGainedEvent>().Any(e => e.Source.OwnerSeat == g.OwnerSeat && e.Turn == turn);
        return MouMhCharge(g.OwnerSeat) > 0 && g.Context.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && g.Context.SourceSeat == g.OwnerSeat;
    }

    SkillProgramStepOutcome global::CardGame.Core.IMouMengHuoHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => ResolveMouMh(f, op);
    internal SkillProgramStepOutcome ResolveMouMh(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext;
        if (f.InstructionIndex != 1 || c is null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Mou Meng Huo requires its original native trigger frame.");
        if (op == SkillProgramEffectOp.ResolveMouMengHuoAttribution)
        {
            if (c.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || c.CardUse is not { } card || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use, EffectiveKind: CardKind.BarbarianAssault } action, MouMengHuoDamageSource: null } use || action.ActorSeat == f.OwnerSeat)
                throw new InvalidOperationException("Huoshou must freeze the actual other actor's native finalized Barbarian Use.");
            var r = new MouMengHuoAttribution(MouMhSource(f), f.GameplayHash, use.Id, action.ActionId, action.ActorSeat);
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { MouMengHuoDamageSource = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoAttributionIssuedEvent(f.Id, r));
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.GainMouMengHuoCharge)
        {
            if (c.Window != SkillProgramTriggerWindow.AfterDamageApplied || c.SourceSeat != f.OwnerSeat || c.Amount <= 0 || MouMhTurn()is not { } turn || c.DamageFrameId is not { } damage || MouMhCharge(f.OwnerSeat) >= 7 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouMengHuoChargeGainedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.Turn == turn))
                return SkillProgramStepOutcome.Continue;
            var before = MouMhCharge(f.OwnerSeat);
            ChangeMouMhCharge(f, 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoChargeGainedEvent(f.Id, MouMhSource(f), turn, damage, before, before + 1));
            return SkillProgramStepOutcome.Continue;
        }

        var obtain = op == SkillProgramEffectOp.ObtainMouMengHuoBarbarian;
        if (c.Window != (obtain ? SkillProgramTriggerWindow.PlayPhaseStarting : SkillProgramTriggerWindow.DiscardPhaseEnded) || c.SourceSeat != f.OwnerSeat || f.MouMengHuo is not null)
            throw new InvalidOperationException("Mou Meng Huo lost its own exact phase boundary.");
        var state = new MouMengHuoReceipt(MouMhSource(f), f.GameplayHash, c.ParentFrameId, obtain, obtain ? 0 : MouMhCharge(f.OwnerSeat))
        {
            Stage = obtain ? MouMengHuoStage.ObtainChildren : MouMengHuoStage.Selecting,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Accepted = obtain
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = state });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoStartedEvent(f.Id, state.Source, state.GameplayHash, state.WindowId, obtain, state.Budget));
        if (!obtain)
        {
            PublishMouMh(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(k => k.Kind == CardKind.BarbarianAssault).ToArray();
        if (candidates.Length > 0)
        {
            var entity = candidates[_randomOperations._random.Next(candidates.Length)];
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(entity, CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new("skill-program.mou-mobile:huoshou.obtain"), move => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoObtainEvent(f.Id, entity.Id, move.From, move.To, move.Sequence)));
            SealMouMh(f.Id, before);
        }

        ContinueMouMh(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 70)]
    internal bool MouMengHuoCardUseForbidden(ProgramCardUseQuery query) => IsMouMengHuoUseForbidden(query.ActorSeat, query.Kind, query.ActionType, query.ExistingUseFrameId);
}
