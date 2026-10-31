#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoChunOlRuntime
{
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 10)]
    internal void CaptureCaoChunUsePaymentBatch(CardMovementBatchContext batch)
    {
        if (!HasCaoChunCapability)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { Action: { Type: CardActionType.Use } a } use || a.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || a.ProviderSeat != a.ActorSeat || _coreMatchParticipantsPlayersAccess._players[a.ActorSeat].Shanjia?.Debt is not { } d || d.Remaining <= 0 || d.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || d.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || batch.ParentFrameId != use.Id || batch.ParentBatchId is not null || batch.Movements.Count != a.PhysicalCards.Count || !a.PhysicalCards.Any(c => c.From.OwnerSeat == a.ActorSeat && c.From.Zone is CardZoneKind.Hand or CardZoneKind.WoodenOxGrain))
            return;
        if (batch.Movements.Any(m => !a.PhysicalCards.Any(c => c.CardId == m.CardId && c.CardKind == m.CardKind && c.From == m.From)) || batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)))
            return;
        if (use.CaoChunMaterialBatch is not null)
            throw new InvalidOperationException("Shanjia actual hand use cannot pay its same material batch twice.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { CaoChunMaterialBatch = batch });
    }

    internal CaoChunHandUsePayment? CaoChunPaidUseQualification(CardActionContext action)
    {
        if (!HasCaoChunCapability)
            return null;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Action?.ActionId == action.ActionId);
        if (use?.CaoChunMaterialBatch is not { } batch)
            return null;
        if (use.CaoChunDebtQualification is { } old)
            return CaoChunDebtCandidate(old)is not null ? old : null;
        return CaptureCaoChunPaidHandUse(use.Id, action, batch.Movements.Min(m => m.Sequence) - 1, batch.Movements.Max(m => m.Sequence));
    }

    internal bool TryBeginCaoChunSimpleDebt(long id, ProgramSimpleCardContinuation continuation)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id);
        if (use?.Action is not { } a || CaoChunPaidUseQualification(a)is not { } p)
            return false;
        return _coreCardActionsOperations.TryBeginProgramCardWindow(null, a, SkillProgramTriggerWindow.CardUseBeforeTargetEffects, a.TargetSeats, ProgramCardContinuation.CommittedSimpleCard, simpleContinuation: continuation, caoChunPayment: p, caoChunDebtOnly: true);
    }

    internal bool CaoChunResponseParentMatches(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCompletedResponseKind kind)
    {
        if (kind == ProgramCompletedResponseKind.Nullification)
            return _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } n && action.ParentActionId == _coreDelayedNullificationOperations.NullificationParentActionId(n) && action.OpponentSeat == n.SourceSeat;
        return attack is not null && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && attack.TargetSeat == action.ActorSeat && attack.SourceSeat == action.OpponentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action?.ActionId == action.ParentActionId;
    }

    internal bool TryBeginCaoChunPaidResponseDebt(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        if ((!HasCaoChunCapability) || !_coreResponseUseCompletionOperations.IsCompletedResponseUse(action, continuation) || action.PhysicalCards.Count == 0)
            return false;
        var kind = continuation == ProgramCardContinuation.Dodge ? ProgramCompletedResponseKind.Dodge : ProgramCompletedResponseKind.Nullification;
        if (!CaoChunResponseParentMatches(attack, action, kind))
            return false;
        var reason = kind == ProgramCompletedResponseKind.Nullification && action.PhysicalCards.Count == 1 ? CardMoveReasons.Nullification : CardMoveReasons.Respond;
        var moves = action.PhysicalCards.Select(c => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == c.CardId && m.CardKind == c.CardKind && m.From == c.From && (m.To == CardLocation.Processing || _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, c.CardId) && m.To == CardLocation.DrawPile) && m.Reason == reason)).ToArray();
        if (moves.Any(m => m is null))
            return false;
        var parent = kind == ProgramCompletedResponseKind.Nullification ? _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow!.Id : attack!.ResolutionId;
        var p = CaptureCaoChunPaidHandUse(parent, action, moves.Min(m => m!.Sequence) - 1, moves.Max(m => m!.Sequence), kind);
        if (p is null)
            return false;
        return _coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseBeforeTargetEffects, [], continuation: null, completedResponseReturn: new(kind, IsCommitted: true), caoChunPayment: p, caoChunDebtOnly: true);
    }
}
