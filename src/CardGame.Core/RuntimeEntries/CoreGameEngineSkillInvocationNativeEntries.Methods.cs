#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSkillInvocationNativeEntries
{
    internal void CancelInvokedFactionDefense(global::CardGame.Core.NativeOwners.FactionDefenseHandle defense) => _resolve().CancelInvokedFactionDefense(defense);
    internal void CancelInvokedFactionSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request) => _resolve().CancelInvokedFactionSlash(request);
    internal void FinishCanceledNativeFactionResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().FinishCanceledNativeFactionResponse(attack);
    internal void RecordNativeFactionInvocationCancellation(global::CardGame.Core.SkillInvocationFrame frame, long requestOwnerId) => _resolve().RecordNativeFactionInvocationCancellation(frame, requestOwnerId);
    internal void ResumeNativeSkillInvocation(global::CardGame.Core.SkillInvocationFrame frame) => _resolve().ResumeNativeSkillInvocation(frame);
    internal void ValidateNativeFactionInvocationReturn(global::CardGame.Core.SkillInvocationFrame frame, long requestOwnerId) => _resolve().ValidateNativeFactionInvocationReturn(frame, requestOwnerId);
}
