#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuAnGuoOlRuntime
{
    internal const string LiyongGainReason = "skill-program.wu-an-guo.liyong.obtain";
    internal bool HasWuAnGuoCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveLiyongUsedSuitDeckAndDuel);

    internal int WuAnGuoUsedSuitMask(int seat) => !HasWuAnGuoCapability ? 0 : _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WuAnGuoActualSuitUsedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && e.ActorSeat == seat).Aggregate(0, (bits, e) => bits | global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime.ActualTurnSuitBit(e.Suit));
    internal bool IsWuAnGuoYangConversionAvailable(CharacterState owner, Card card, CardConversionSource source, bool forResponse)
    {
        if (source.SkillId != "ol:liyong" || source.BindingId != "yang-unused-suit")
            return true;
        var bit = global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime.ActualTurnSuitBit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card));
        return HasWuAnGuoCapability && !forResponse && source.OwnerSeat == owner.Seat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner.Seat && owner.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, source.SkillId, source.SkillInstanceId) && !_coreProgramPersistentStateOperations.GetProgramBooleanState2(owner.Seat, source.SkillId, source.SkillInstanceId, "yin") && bit != 0 && (WuAnGuoUsedSuitMask(owner.Seat) & bit) == 0;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27010)]
    internal void ObserveWuAnGuoActualUses(IGameEvent payload)
    {
        if (!HasWuAnGuoCapability || !_coreCurrentTurnUseKindsOperations.HasOngoingActualUseTurn)
            return;
        CardActionContext? action = null;
        CardUseFrame? use = null;
        if (payload is CardUseDeclaredEvent declared)
        {
            use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == declared.ResolutionId);
            if (use is null || use.SourceSeat != declared.SourceSeat || use.CardKind != declared.CardKind || use.CardId != declared.CardId)
                throw new InvalidOperationException("Liyong suit history requires the exact native declared use.");
            action = use.Action;
            if (action is null || action.Type != CardActionType.Use || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind)
                return;
        }
        else if (payload is CardActionAcceptedEvent accepted && accepted.Action is { Type: CardActionType.Response } response)
        {
            if (response.ActorSeat != response.ProviderSeat || response.ResponderSeat != response.ActorSeat || response.RequesterSeat is not null)
                return;
            if (response.EffectiveKind == CardKind.Nullification)
            {
                var window = _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault();
                if (window is null || !_coreDelayedNullificationOperations.HasNullificationEffectOwner(window) || response.ParentActionId != _coreDelayedNullificationOperations.NullificationParentActionId(window) || response.OpponentSeat != window.SourceSeat)
                    throw new InvalidOperationException("Liyong counterspell suit requires its exact native use node.");
            }
            else if (response.EffectiveKind == CardKind.Dodge)
            {
                var parent = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Action?.ActionId == response.ParentActionId);
                if (parent?.CardAttack is not { EffectiveCardKind: { } kind } attack || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || attack.TargetSeat != response.ActorSeat || response.OpponentSeat != attack.SourceSeat)
                    return;
            }
            else
                return;
            action = response;
        }

        if (action is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat))
            return;
        if (action.EffectiveSuit is { } suit && global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime.ActualTurnSuitBit(suit) != 0 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WuAnGuoActualSuitUsedEvent>().Any(e => e.ActionId == action.ActionId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuAnGuoActualSuitUsedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, action.ActorSeat, action.ActionId, suit));
        if (use is null)
            return;
        foreach (var source in action.ConversionChain.Where(s => s.SkillId == "ol:liyong" && s.BindingId == "yang-unused-suit"))
        {
            if (source.OwnerSeat != action.ActorSeat || action.PhysicalCards is not [var physical] || physical.From.OwnerSeat != source.OwnerSeat || physical.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || action.EffectiveKind != CardKind.Duel || action.EffectiveSuit is not { } usedSuit || global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime.ActualTurnSuitBit(usedSuit) == 0 || _coreProgramPersistentStateOperations.GetProgramBooleanState2(source.OwnerSeat, source.SkillId, source.SkillInstanceId, "yin") || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WuAnGuoActualSuitUsedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && e.ActorSeat == action.ActorSeat && e.Suit == usedSuit && e.ActionId != action.ActionId))
                throw new InvalidOperationException("Liyong Yang requires its original unused effective suit, one native owned physical payment and current source generation.");
            _coreProgramPersistentStateOperations.SetProgramBooleanState(source.OwnerSeat, source.SkillId, source.SkillInstanceId, "yin", true);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuAnGuoLiyongYangConsumedEvent(use.Id, action.ActionId, source, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, usedSuit));
        }
    }

    SkillProgramStepOutcome global::CardGame.Core.IWuAnGuoProgramHost.Resolve(ProgramSkillFrame frame) => BeginWuAnGuoLiyong(frame);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 27010, SkillProgramEffectOp.ResolveLiyongUsedSuitDeckAndDuel)]
    internal bool CanActivateWuAnGuoLiyong(CharacterState owner) => _coreMatchParticipantsPlayersAccess._players.Any(actor => CanIssueWuAnGuoLiyongDuel(actor.Seat, owner.Seat));
    internal SkillProgramStepOutcome BeginWuAnGuoLiyong(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var mask = WuAnGuoUsedSuitMask(f.OwnerSeat);
        if (f.WuAnGuoLiyong is not null || f.SkillId != "ol:liyong" || f.ActivationId != "yin-obtain-and-incoming-duel" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var actor] || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !_coreProgramPersistentStateOperations.GetProgramBooleanState(f, "yin") || !CanIssueWuAnGuoLiyongDuel(actor, f.OwnerSeat))
            throw new InvalidOperationException("Liyong Yin requires its current real Play state, earlier actual used suits and native legal incoming Duel actor.");
        var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => (global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime.ActualTurnSuitBit(c.Suit) & mask) != 0).OrderBy(c => c.Id).ToArray();
        var card = candidates.Length == 0 ? null : candidates[_randomOperations._random.Next(candidates.Length)];
        var source = new CardConversionSource(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuAnGuoLiyong = new(source, f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, actor, mask, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, card?.Id) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuAnGuoLiyongYinStartedEvent(f.Id, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, actor, mask));
        _coreProgramSkillChangesOperations.SetProgramBooleanState(f, "yin", false);
        if (card is not null)
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new(LiyongGainReason), movement =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { WuAnGuoLiyong = current.WuAnGuoLiyong!with { After = movement.Sequence } });
            });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var r = f.WuAnGuoLiyong!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuAnGuoLiyongGainPaidEvent(f.Id, r.ObtainedCardId, r.Before, r.After));
        ContinueWuAnGuoLiyong(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueWuAnGuoLiyong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuAnGuoLiyong: { } r } f || f.Id != id)
            return;
        AssertWuAnGuoLiyong(f);
        if (r.Stage != WuAnGuoLiyongStage.GainChildren)
            throw new InvalidOperationException("An issued Liyong Duel must return through its native typed completion.");
        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !CanIssueWuAnGuoLiyongDuel(r.InitialActorSeat, f.OwnerSeat))
        {
            FinishWuAnGuoLiyong(f, false);
            return;
        }

        var origin = new WuAnGuoLiyongDuelOrigin(f.Id, f.InstructionIndex, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, r.Source, r.GameplayHash, r.ActualTurn, r.PhaseInstance, r.InitialActorSeat, f.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuAnGuoLiyong = r with { Stage = WuAnGuoLiyongStage.DuelIssued, Duel = origin } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuAnGuoLiyongDuelIssuedEvent(origin));
        BeginWuAnGuoLiyongDuel(f, origin);
    }

    internal void FinishWuAnGuoLiyong(ProgramSkillFrame f, bool issued)
    {
        if (!ValidWuAnGuoLiyong(f) || issued != (f.WuAnGuoLiyong!.Stage == WuAnGuoLiyongStage.DuelIssued))
            throw new InvalidOperationException("Liyong lost its paid gain or exact issued native Duel return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuAnGuoLiyongCompletedEvent(f.Id, issued));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuAnGuoLiyong = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27010)]
    internal bool ResumeWuAnGuoLiyong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuAnGuoLiyong: not null } f || f.Id != id)
            return false;
        ContinueWuAnGuoLiyong(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27010)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27010)]
    internal bool ContinuePaidWuAnGuoLiyong(ProgramSkillFrame f) => f.WuAnGuoLiyong is not null && ValidWuAnGuoLiyong(f);
    [ProgramCardLegality(ProgramCardLegalityStage.ConversionSourceBan, 40)]
    internal bool WuAnGuoConversionUnavailable(ProgramViewAsSourceQuery query) => !IsWuAnGuoYangConversionAvailable(query.Owner, query.Card, query.Source, query.ForResponse);
}
