#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuQiOlRuntime
{
    internal static string WenjiName(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_liu_qi_ol_Wenji_Observers.WenjiName(kind);
    internal bool IsWenjiResponseForbidden(CardActionContext originalUse, int responderSeat)
    {
        if (_characterZhangLiaoHappyGodOperations.HappyZlCannotRespond(originalUse, responderSeat))
            return true;
        if (!_characterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi || originalUse.Type != CardActionType.Use || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(originalUse.ActorSeat) || responderSeat == originalUse.ActorSeat || originalUse.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber < 1)
            return false;
        var grants = _coreMatchParticipantsPlayersAccess._players[originalUse.ActorSeat].WenjiTurnNameGrants;
        if (grants is not { Count: > 0 })
            return false;
        foreach (var grant in grants)
            if (grant.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && grant.Source.OwnerSeat == originalUse.ActorSeat && WenjiName(grant.Name) == WenjiName(originalUse.EffectiveKind) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WenjiNameArmedEvent>().Count(e => e.Grant == grant) == 1)
                return true;
        return false;
    }

    internal bool IsWenjiResponseForbidden(long useId, int responderSeat)
    {
        if (_characterZhangLiaoHappyGodOperations.HappyZlCannotRespond2(useId, responderSeat))
            return true;
        if (!_characterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { } use)
            return false;
        if (use.Action is { } action)
            return IsWenjiResponseForbidden(action, responderSeat);
        if (use.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || responderSeat == use.SourceSeat || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id && e.SourceSeat == use.SourceSeat && e.CardId == 0 && e.CardKind == use.CardKind))
            return false;
        return (_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].WenjiTurnNameGrants ?? []).Any(g => g.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && g.Source.OwnerSeat == use.SourceSeat && WenjiName(g.Name) == WenjiName(use.CardKind) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WenjiNameArmedEvent>().Count(e => e.Grant == g) == 1);
    }

    internal bool IsWenjiNullificationResponseForbidden(NullificationWindowFrame window, int responderSeat)
    {
        if (_characterZhangLiaoHappyGodOperations.HappyZlNullificationForbidden(window, responderSeat))
            return true;
        if (!_characterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi)
            return false;
        if (window.DelayedReturn is null && IsWenjiResponseForbidden(window.ParentFrameId, responderSeat))
            return true;
        if (window.WenjiCounterspell is not { } q || q.WindowId != window.Id || q.ParentFrameId != window.ParentFrameId || q.ChainDepth != window.ChainDepth || q.Grant.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || q.Grant.Source.OwnerSeat != q.Action.ActorSeat || responderSeat == q.Action.ActorSeat || q.Action.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || q.Action.Type != CardActionType.Response || q.Action.EffectiveKind != CardKind.Nullification || q.Action.ParentActionId != _coreDelayedNullificationOperations.NullificationParentActionId(window) || q.Action.ActorSeat != q.Action.ProviderSeat || q.Action.ResponderSeat != q.Action.ActorSeat || q.Action.RequesterSeat is not null || !_coreDelayedNullificationOperations.HasNullificationEffectOwner(window))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WenjiNameArmedEvent>().Count(e => e.Grant == q.Grant) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WenjiCounterspellQualifiedEvent>().Count(e => e == new WenjiCounterspellQualifiedEvent(window.Id, window.ParentFrameId, window.ChainDepth, q.Action.ActionId, q.Grant)) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == q.Action.ActionId && e.Action.ParentActionId == q.Action.ParentActionId && e.Action.Type == q.Action.Type && e.Action.ActorSeat == q.Action.ActorSeat && e.Action.ProviderSeat == q.Action.ProviderSeat && e.Action.EffectiveKind == q.Action.EffectiveKind);
    }
}
