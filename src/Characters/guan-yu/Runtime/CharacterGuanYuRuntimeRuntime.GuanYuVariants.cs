#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IGuanYuVariantHost.ExecuteGuanYuVariant(ProgramSkillFrame frame, SkillProgramEffectOp op) => ExecuteGuanYuVariant(frame, op);
    internal IReadOnlyList<int> ExclusiveSupportTargets(int recipient, string? skill = null) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DemonSupportGrantedEvent>().Where(e => e.RecipientSeat == recipient && (skill is null || e.SkillId == skill) && _coreMatchParticipantsPlayersAccess._players[e.OwnerSeat].IsAlive).Select(e => e.OwnerSeat).Distinct().Order().ToArray();
    internal CardActionContext GuanParentAction(ProgramSkillFrame frame)
    {
        var context = frame.WindowContext?.CardUse ?? throw new InvalidOperationException("Guan rule requires its actual card-action context.");
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == frame.WindowContext!.ParentFrameId);
        return parent.Action;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1000)]
    internal bool CanOfferGuanVariant(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        var owner = candidate.OwnerSeat;
        if (candidate.SkillId == "magic:hanguo" && trigger.Id == "aid-reward")
        {
            if (context.CardUse is not { ActorSeat: var defender } || DemonFocus(defender)?.Source.OwnerSeat != owner)
                return false;
            var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId);
            return window is { Continuation: ProgramCardContinuation.FactionDefenseDodge } && window.Action.ProviderSeat != defender;
        }

        if (candidate.SkillId == "magic:longyin")
            return context.CardUse is { ActorSeat: var actor } && actor == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && ExclusiveSupportTargets(owner, candidate.SkillId).Contains(actor) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner]).Count > 0;
        var ops = trigger.Effects.Select(e => e.Op).ToArray();
        if (ops.Contains(SkillProgramEffectOp.HoldRoundFocusCards))
            return _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats.Count == 1 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DemonRoundFocusEvent>().Any(e => e.Source.OwnerSeat == owner && e.RoundNumber == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber);
        if (ops.Contains(SkillProgramEffectOp.SpareFirstLethalTarget))
            return context.SourceSeat == owner && owner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && context.TargetSeat is { } target && target != owner && context.Amount >= _coreMatchParticipantsPlayersAccess._players[target].Hp && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuanLethalSparedEvent>().Any(e => e.OwnerSeat == owner && e.TargetSeat == target);
        if (ops.Contains(SkillProgramEffectOp.RecordPhaseFocusedSlash))
            return GuanPhaseFocus(owner)is { } focus && context.CardUse?.DesignatedTargetSeats?.Contains(focus.TargetSeat) == true;
        if (ops.Contains(SkillProgramEffectOp.CancelSparedCardTargets))
            return context.CardUse?.DesignatedTargetSeats?.Any(t => GuanSparedThisTurn(owner, t)) == true;
        if (ops.Contains(SkillProgramEffectOp.RewriteWeilinTargetHand))
            return context.CardUse is { } use && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == use.CardActionId && e.Action.ConversionChain.Any(s => s.SkillId == candidate.SkillId && s.OwnerSeat == owner && s.BindingId != "weilin-hand-slash"));
        if (ops.Contains(SkillProgramEffectOp.KillRoundFocusSlashVictim))
            return context.TargetSeat is { } victim && DemonFocus(victim)is { } focus && focus.Source.OwnerSeat == owner && context.SourceSeat == owner && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsChainPropagation: false, IsSourceLess: false, EffectiveCardKind: { } kind } && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind);
        return true;
    }

    internal SkillProgramStepOutcome ExecuteGuanYuVariant(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != supplied.SkillId || frame.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash)
            throw new InvalidOperationException("Guan rule lost its original content binding.");
        var source = new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        switch (op)
        {
            case SkillProgramEffectOp.ArmPhaseSlashFocus:
                if (frame.SelectedTargetSeats is not [var target] || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreMatchParticipantsPlayersAccess._players[target].Role == Role.Lord)
                    throw new InvalidOperationException("Slash focus requires its own actual Play phase and non-lord target.");
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanPhaseFocusEvent(frame.Id, frame.OwnerSeat, frame.SkillId, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId));
                break;
            case SkillProgramEffectOp.RecordPhaseFocusedSlash:
                var focus = GuanPhaseFocus(frame.OwnerSeat) ?? throw new InvalidOperationException("Focused Slash lost its phase lease.");
                var slash = GuanParentAction(frame);
                if (slash.ActorSeat != frame.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(slash.EffectiveKind) || !slash.EffectiveDesignatedTargetSeats.Contains(focus.TargetSeat))
                    throw new InvalidOperationException("Focused Slash requires the accepted designated target.");
                if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuanFocusedSlashEvent>().Any(e => e.ActionId == slash.ActionId && e.OwnerSeat == frame.OwnerSeat))
                {
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanFocusedSlashEvent(slash.ActionId, frame.OwnerSeat, focus.TargetSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId));
                    _coreCardUseDebitsOperations.RefundProgramCardUseDebit(frame);
                    _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, frame.OwnerSeat, _coreNativeDamageHandlesOperations.IsClassicIdentityMode ? 2 : 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.focused-slash.draw"));
                }

                break;
            case SkillProgramEffectOp.SpareFirstLethalTarget:
                if (frame.WindowContext?.TargetSeat is not { } victim || _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == frame.WindowContext.ParentFrameId)is not { } damage || damage.SourceSeat != frame.OwnerSeat || damage.Amount < _coreMatchParticipantsPlayersAccess._players[victim].Hp)
                    throw new InvalidOperationException("Lethal prevention lost its actual source damage window.");
                _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(frame);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanLethalSparedEvent(frame.Id, frame.OwnerSeat, victim, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
                break;
            case SkillProgramEffectOp.CancelSparedCardTargets:
                var action = GuanParentAction(frame);
                var use = _coreCardUseLifecycleOperations.LifecycleCardUse(frame.WindowContext!.CardUse!.ParentCardUseFrameId) ?? throw new InvalidOperationException("Spared target lost its card use.");
                var canceled = action.EffectiveDesignatedTargetSeats.Where(t => GuanSparedThisTurn(frame.OwnerSeat, t));
                _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { IneffectiveTargetSeats = Array.AsReadOnly((use.IneffectiveTargetSeats ?? []).Concat(canceled).Distinct().ToArray()) });
                break;
            case SkillProgramEffectOp.RecordOpeningTurnUse:
                var opening = GuanParentAction(frame);
                if (opening.ActorSeat != frame.OwnerSeat)
                    throw new InvalidOperationException("Opening use changed actor.");
                var firstBasic = !GuanOpeningUseExists(frame.OwnerSeat, false) && CardUseCategoryCatalog.Get(opening.EffectiveKind) == CardUseCategories.Basic;
                var weilin = opening.ConversionChain.Any(s => s.SkillId == "mou-ol:weilin" && s.OwnerSeat == frame.OwnerSeat && s.BindingId != "weilin-hand-slash");
                if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuanOpeningUseEvent>().Any(e => e.ActionId == opening.ActionId && e.OwnerSeat == frame.OwnerSeat))
                {
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanOpeningUseEvent(opening.ActionId, frame.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, opening.EffectiveIsRed == true, CardUseCategoryCatalog.Get(opening.EffectiveKind) == CardUseCategories.Basic, weilin));
                    if (firstBasic)
                    {
                        _coreCardUseDebitsOperations.RefundProgramCardUseDebit(frame);
                        if (opening.EffectiveKind == CardKind.Alcohol && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == frame.OwnerSeat)
                            _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].UsedPlayPhaseAlcoholThisTurn = false;
                    }
                }

                break;
            case SkillProgramEffectOp.RewriteWeilinTargetHand:
                var converted = GuanParentAction(frame);
                var appearance = converted.EffectiveIsRed;
                if (appearance is null || converted.EffectiveSuit == Suit.None)
                    break;
                var conversionSource = converted.ConversionChain.Single(s => s.SkillId == frame.SkillId && s.OwnerSeat == frame.OwnerSeat);
                var recipients = converted.EffectiveKind == CardKind.Alcohol ? new[]
                {
                    frame.OwnerSeat
                }

                : converted.EffectiveDesignatedTargetSeats;
                foreach (var seat in recipients.Distinct())
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeilinHandColorEvent(converted.ActionId, conversionSource, seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, appearance.Value));
                break;
            case SkillProgramEffectOp.HoldRoundFocusCards:
                if (frame.SelectedTargetSeats is not [var heldTarget] || _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber <= 0 || heldTarget == frame.OwnerSeat)
                    throw new InvalidOperationException("Round hold requires one other living target.");
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DemonRoundFocusEvent(frame.Id, source, heldTarget, _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber));
                foreach (var zone in new[]
                {
                    CardLocation.Hand(heldTarget),
                    CardLocation.Equipment(heldTarget),
                    CardLocation.Judgment(heldTarget)
                }

                )
                    foreach (var card in _cardZonesOperations._cardZones.CardsAt(zone).ToArray())
                    {
                        _corePhysicalMovementFacadesOperations.MoveCard(card, zone, CardLocation.RoundHold(heldTarget), new("skill-program.hanguo.hold"));
                    }

                break;
            case SkillProgramEffectOp.KillRoundFocusSlashVictim:
                if (frame.WindowContext?.TargetSeat is not { } killed || !_coreMatchParticipantsPlayersAccess._players[killed].IsAlive)
                    break;
                _coreNativePlayerDeathOperations.BeginPlayerDeath(frame.Id, _coreMatchParticipantsPlayersAccess._players[killed], null, null, null, causingProgramSkillFrameId: frame.Id);
                return SkillProgramStepOutcome.AwaitChild;
            case SkillProgramEffectOp.GrantExclusiveRende:
            case SkillProgramEffectOp.GrantExclusiveQingnang:
            case SkillProgramEffectOp.GrantExclusiveLongyin:
                if (frame.SelectedTargetSeats is [var recipient])
                {
                    var id = op switch
                    {
                        SkillProgramEffectOp.GrantExclusiveRende => "magic:rende",
                        SkillProgramEffectOp.GrantExclusiveQingnang => "magic:qingnang",
                        _ => "magic:longyin"
                    };
                    _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[recipient], $"magic:weiwo:{(frame.OwnerSeat)}", [id]);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DemonSupportGrantedEvent(frame.Id, frame.OwnerSeat, recipient, id));
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { SelectedTargetSeats = [] });
                break;
            default:
                throw new InvalidOperationException("Unsupported Guan operation.");
        }

        return SkillProgramStepOutcome.Continue;
    }
}
