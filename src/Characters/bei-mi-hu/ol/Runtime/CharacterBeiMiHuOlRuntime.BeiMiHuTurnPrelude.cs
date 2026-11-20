#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBeiMiHuOlRuntime
{
    internal bool TryBeginBeiMiHuActualTurnPrelude(CharacterState owner)
    {
        if (!_coreActualTurnsOperations.HasBeforeTurnPreludeBindingWork(owner) &&
            !_programPipelineHooksOperations._programPipelineHooks.HasBeforeTurnPreludeWork(owner.Seat))
            return false;
        var id = _coreActualTurnsOperations.OpenBeforeTurnPrelude(owner);
        _characterLuSuMobileShiOperations.ExpireShiLsHandGrants(owner.Seat, id);
        _coreRuntimeOperations.AdvanceRuntimeFrame(id);
        return true;
    }
}
