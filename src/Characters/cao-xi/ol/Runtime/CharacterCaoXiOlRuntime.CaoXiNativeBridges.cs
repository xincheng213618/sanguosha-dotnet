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
        _coreNamedAmbushInvalidationOperations.CompleteNamedAmbushCounterspell(pending);
    }
}
