#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangWumiaoRuntime
{
    internal bool CanWuZglQingshi(int owner, ProgramSkillWindowContext c)
    {
        var action = WuZglAction(c);
        return c.Window == SkillProgramTriggerWindow.CardUseCommitted && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && owner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && WuZglTurnOpen(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat) && action is not null && action.ActorSeat == owner && (action.Type == CardActionType.Use || action.ProviderSeat == owner && action.RequesterSeat is null && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[owner], action.EffectiveKind)) && !_coreProgramEventHistoryOperations.ProgramEventHistory<WuZhugeLiangQingshiDisabledEvent>().Any(e => e.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat) && !_coreProgramEventHistoryOperations.ProgramEventHistory<WuZhugeLiangQingshiAcceptedEvent>().Any(e => e.Source.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && e.Name == global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(action.EffectiveKind)) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Any(card => !action.PhysicalCards.Any(m => m.CardId == card.Id) && global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[owner], card)) == global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(action.EffectiveKind));
    }

    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 60)]
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> WuZglQingshiDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => attack.IsChainPropagation ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhugeLiangQingshiDamageEvent>().Where(e => e.UseId == attack.ResolutionId && e.TargetSeat == attack.TargetSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(e.UseId)?.Action is { } a && a.ActionId == e.ActionId && a.ActorSeat == e.Source.OwnerSeat && attack.CardUserSeat == a.ActorSeat).Select(e => (new CardUseEffectSource(e.Source.SkillId, e.Source.BindingId, e.Source.OwnerSeat, e.Source.SkillInstanceId), 1));
}
