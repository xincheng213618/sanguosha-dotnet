#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeReturnBridgesRuntime
{
    [NativeAttackReturn(NativeAttackReturnPhase.BeforeCleanup, 10)]
    internal bool ReturnQinglongNativeAttack(GameEngine.AttackCompletionReceipt completion) => _coreQinglongInsertionOperations.RestoreContinuationAfterQinglongFollowup(completion.ResolutionId);
    [NativeUseReturn(180)]
    internal void ReturnDyingNativeUse(NativeUseReturnContext context)
    {
        if (context.DyingResponse is { } response)
            _coreNativeDyingOperations.CompleteDyingCardResponse(response);
    }
}
