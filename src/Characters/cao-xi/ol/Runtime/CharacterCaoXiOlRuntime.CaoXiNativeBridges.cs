#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoXiOlRuntime
{
    internal int FreezeAndCaptureCaoXiNativeDrawBase(CharacterState player, bool replaced = false)
    {
        var units = FreezeCaoXiGangshuDrawUse(player);
        var count = replaced ? 0 : _coreActualTurnsOperations.GetTurnDrawCount(player);
        CaptureCaoXiGangshuDrawUse(player, units);
        return count;
    }

    internal void CompleteCaoXiNativeCounterspellBeforeUseCompleted(CardActionContext action, ProgramCardContinuation continuation)
    {
        if (!HasCaoXiGangshuCapability || continuation != ProgramCardContinuation.NullificationResponse || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { NamedAmbushResponse: { } response } pending)
            return;
        if (response.ActionId != action.ActionId || action.EffectiveKind != CardKind.Nullification || action.Type != CardActionType.Response || response.ResponderSeat != action.ActorSeat || !action.PhysicalCards.Any(c => c.CardId == response.CardId) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != pending.Id)
            throw new InvalidOperationException("Gangshu requires the exact completed native counterspell before its nonbasic-use observer.");
        _coreNamedAmbushInvalidationOperations.CompleteNamedAmbushCounterspell(pending);
    }
}
