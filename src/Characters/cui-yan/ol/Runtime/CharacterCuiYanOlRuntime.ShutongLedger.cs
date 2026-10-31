#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCuiYanOlRuntime
{
    internal bool IsCuiYanNextUseForbidden(int actorSeat, IReadOnlyList<int> materialIds, Suit? nativeEffectiveSuit)
    {
        if (!HasCuiYanCapability || !CuiYanActualTurnOpen)
            return false;
        var suit = CuiYanTrueUseSuit(materialIds, nativeEffectiveSuit);
        if (suit == Suit.None)
            return false;
        return OutstandingShutong(actorSeat).Any(g => g.Suit == suit);
    }

    internal IEnumerable<ShutongNextUseGrantedEvent> OutstandingShutong(int seat) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShutongNextUseGrantedEvent>().Where(g => g.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && g.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && g.RecipientSeat == seat && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShutongNextUseConsumedEvent>().Any(c => c.GrantFrameId == g.FrameId && c.RecipientSeat == seat));
    internal void ConsumeShutongForActualUse(long useId, CardActionContext? a, int actor, bool response)
    {
        var id = a?.ActionId ?? 0;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShutongNextUseConsumedEvent>().Any(e => e.UseId == useId && e.ActionId == id && e.RecipientSeat == actor && e.ResponseUse == response))
            return;
        foreach (var grant in OutstandingShutong(actor).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShutongNextUseConsumedEvent(grant.FrameId, actor, useId, id, response, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat));
    }
}
