#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiangXingOlRuntime
{
    internal SkillProgramStepOutcome GiveProgramSelectedTargetHand(ProgramSkillFrame frame, int targetSeat)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var selected] || selected != targetSeat)
            throw new InvalidOperationException("LueLve lost its selected counterpart.");
        var location = CardLocation.Hand(targetSeat);
        var cards = _cardZonesOperations._cardZones.CardsAt(location);
        if (cards.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCards(cards.ToArray(), location, CardLocation.Hand(frame.OwnerSeat), new CardMoveReason($"skill-program.{(frame.SkillId)}.luelve-gift"), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, location.OwnerSeat!.Value, frame.OwnerSeat, cards.Select(c => c.Id).ToArray(), _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome LuelveProgramCounterSlash(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var selected] || selected == frame.OwnerSeat)
            throw new InvalidOperationException("LueLve lost its selected counterpart.");
        if (!_coreMatchParticipantsPlayersAccess._players[selected].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            return SkillProgramStepOutcome.Continue;
        if (_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(selected, frame.OwnerSeat, CardKind.Slash) || _coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]) || _coreNativeRuleQueriesOperations.GetSeatDistance(selected, frame.OwnerSeat) > _coreNativeRuleQueriesOperations.GetAttackRange(selected))
            return SkillProgramStepOutcome.Continue;
        var resolutionId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(resolutionId, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, selected, selected, null, selected, null, CardKind.Slash, [frame.OwnerSeat], [], Array.AsReadOnly(new[] { new CardConversionSource(active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), selected, active.SkillInstanceId) }), effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(resolutionId, selected, 0, CardKind.Slash, Array.AsReadOnly(new[] { frame.OwnerSeat }), PhysicalCardIds: Array.AsReadOnly(Array.Empty<int>())) { Action = action });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(resolutionId, 0, CardKind.Slash, selected));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(resolutionId, Array.AsReadOnly(new[] { frame.OwnerSeat })));
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(resolutionId, selected, frame.OwnerSeat, card: null, damageAmount: 1, playedCardKind: CardKind.Slash, programSkillCardUseFrameId: frame.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, selected, frame.OwnerSeat));
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, [frame.OwnerSeat], ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
        return SkillProgramStepOutcome.AwaitChild;
    }

    SkillProgramStepOutcome global::CardGame.Core.ILiangXingProgramHost.GiveSelectedTargetHand(ProgramSkillFrame frame, int targetSeat) => GiveProgramSelectedTargetHand(frame, targetSeat);
    SkillProgramStepOutcome global::CardGame.Core.ILuelveCounterSlashProgramHost.SelectedTargetVirtualSlashAgainstOwner(ProgramSkillFrame frame) => LuelveProgramCounterSlash(frame);
}
