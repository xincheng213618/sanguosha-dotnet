#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDelayedNullificationRuntime
{
    internal void BeginDelayedNullificationWindow(CharacterState owner, Card card, CardKind kind)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null)
            throw new InvalidOperationException("A delayed effect cannot open a second counterspell window.");
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault();
        var delayed = new DelayedEffectContinuation(card.Id, kind, owner.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase, parent?.Id ?? 0, parent?.Kind, parent?.Step);
        var action = kind switch
        {
            CardKind.Indulgence => LegalActionKind.Indulgence,
            CardKind.SupplyShortage => LegalActionKind.SupplyShortage,
            CardKind.Lightning => LegalActionKind.Lightning,
            _ => throw new InvalidOperationException("Only delayed tricks can enter delayed counterspell resolution.")};
        var window = new NullificationWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, delayed.ParentFrameId, owner.Seat, Array.AsReadOnly(new[] { owner.Seat }), card.Id, kind, action, null, null, _coreNativeNullificationOperations.BuildNullificationCandidateSeats(owner.Seat))
        {
            DelayedReturn = delayed
        };
        _coreRuntimeOperations.PushRuntimeFrame(window);
        _coreNativeNullificationOperations.ContinueNullificationWindow(window);
    }

    internal bool HasNullificationEffectOwner(NullificationWindowFrame window)
    {
        if (window.GroupTargetReturn is not null)
            return _coreGroupTargetNullificationOperations.IsGroupTargetNullification(window);
        if (window.DelayedReturn is not { } delayed)
            return _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not null;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == window.Id);
        var parent = index > 0 ? _resolutionStackOperations._resolutionStack[index - 1] : null;
        return index >= 0 && (parent?.Id ?? 0) == delayed.ParentFrameId && parent?.Kind == delayed.ParentKind && parent?.Step == delayed.ParentStep && window.ParentFrameId == delayed.ParentFrameId && window.EffectCardId == delayed.CardId && window.EffectCardKind == delayed.EffectiveKind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(delayed.EffectiveKind) && window.SourceSeat == delayed.OwnerSeat && window.TargetSeats.SequenceEqual([delayed.OwnerSeat]) && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber == delayed.TurnNumber && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == delayed.Phase && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == delayed.OwnerSeat && (delayed.OriginalUseLightning is not null ? _coreOriginalUseLightningOperations.ValidOriginalUseLightningDelayed(delayed) : _coreShanheDelayedNullifiedTrickClaimsOperations.HasShanheClaimedDelayedOwner(window, delayed) || _cardZonesOperations._cardZones.GetLocation(delayed.CardId) == CardLocation.Judgment(delayed.OwnerSeat) && _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(_coreNativeNullificationFramesOperations.GetNullificationEffectCard(window)) == delayed.EffectiveKind);
    }

    internal long? NullificationParentActionId(NullificationWindowFrame window) => window.DelayedReturn is null ? _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)?.Action?.ActionId : null;
    internal void CompleteDelayedNullification(NullificationWindowFrame window, DelayedEffectContinuation delayed)
    {
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault();
        if ((parent?.Id ?? 0) != delayed.ParentFrameId || parent?.Kind != delayed.ParentKind || parent?.Step != delayed.ParentStep || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber != delayed.TurnNumber || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != delayed.Phase || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != delayed.OwnerSeat)
            throw new InvalidOperationException("Delayed counterspell lost its exact turn and parent return.");
        if (delayed.OriginalUseLightning is not null)
        {
            _coreOriginalUseLightningOperations.CompleteOriginalUseLightningNullification(delayed, window.EffectNullified, window.Id);
            return;
        }

        if (_coreShanheDelayedNullifiedTrickClaimsOperations.TryCompleteShanheClaimedDelayedNullification(window, delayed))
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[delayed.OwnerSeat];
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive)
            return;
        var card = _corePhysicalOwnedCardQueriesOperations.GetJudgment(owner).SingleOrDefault(card => card.Id == delayed.CardId);
        if (card is null || _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(card) != delayed.EffectiveKind)
            throw new InvalidOperationException("Delayed counterspell lost its held card identity.");
        if (window.EffectNullified)
        {
            _coreMatchLogOperations.AddLog("CardEffect", $"【{(CardCatalog.Get(delayed.EffectiveKind).DisplayName)}】的效果被无懈。", owner.Seat);
            if (delayed.EffectiveKind == CardKind.Lightning)
                _coreNativeJudgmentOperations.CompleteLightningWithoutHit(window.Id, card, owner, null);
            else
            {
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Judgment(owner.Seat), CardLocation.DiscardPile, CardMoveReasons.DelayedCardFinish);
                _coreActualTurnsOperations.BeginDelayedJudgmentOrTurnStart(owner);
            }

            return;
        }

        if (_coreCardEffectProgramsOperations.TryBeginDelayedCardEffectPrograms(owner, card, delayed.EffectiveKind))
            return;
        _coreCardEffectProgramsOperations.BeginActualDelayedJudgment(owner, card, delayed.EffectiveKind);
    }
}
