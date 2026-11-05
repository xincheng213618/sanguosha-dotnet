#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreActiveNativeAttackQueriesRuntime
{
    internal NativeOwners.CardAttackHandle? ActiveCardAttack
    {
        get => _resolutionStackOperations._resolutionStack.LastOrDefault(frame => (frame switch
        {
            CardUseFrame use => use.CardAttack?.Active == true,
            ProgramSkillFrame program => program.CardAttack?.Active == true,
            JudgmentFrame judgment => judgment.CardAttack?.Active == true,
            _ => false
        }) && !_nativeCursorSuspensions.IsSuspended(frame.Id, NativeCursorScope.AttackAndContinuations))
            is { } owner ? _kernelCoreMainOperations.CreateCardAttackHandle(owner.Id) : null;
        set
        {
            NativeOwners.CardAttackHandle? current = ActiveCardAttack;
            if (current is not null && current.ResolutionId != value?.ResolutionId)
                _coreCardAttackStateOperations.UpdateCardAttackState(current.ResolutionId, state => state!with { Active = false });
            if (value is not null)
                _coreCardAttackStateOperations.UpdateCardAttackState(value.ResolutionId, state => state!with { Active = true });
        }
    }
}
