#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSkillInvocationNativeEntries
{
    internal void CancelInvokedFactionDefense(NativeOwners.FactionDefenseHandle defense) => _resolve().CancelInvokedFactionDefense(defense);
    internal void CancelInvokedFactionSlash(NativeOwners.FactionCardRequestHandle request) => _resolve().CancelInvokedFactionSlash(request);
    internal void FinishCanceledNativeFactionResponse(NativeOwners.CardAttackHandle attack) => _resolve().FinishCanceledNativeFactionResponse(attack);
    internal void RecordNativeFactionInvocationCancellation(SkillInvocationFrame frame, long requestOwnerId) => _resolve().RecordNativeFactionInvocationCancellation(frame, requestOwnerId);
    internal void ResumeNativeSkillInvocation(SkillInvocationFrame frame) => _resolve().ResumeNativeSkillInvocation(frame);
    internal void ValidateNativeFactionInvocationReturn(SkillInvocationFrame frame, long requestOwnerId) => _resolve().ValidateNativeFactionInvocationReturn(frame, requestOwnerId);
}
