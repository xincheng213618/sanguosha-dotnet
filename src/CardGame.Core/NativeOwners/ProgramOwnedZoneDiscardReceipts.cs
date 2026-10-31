#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramOwnedZoneDiscardReceipts(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBuZhiOlEntries> getCharacterBuZhiOlRuntime) : IProgramOwnedZoneDiscardReceipts
{
    public IProgramOwnedZoneDiscardInvocation? Capture(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => getCharacterBuZhiOlRuntime().CaptureNativeBuZhiDiscardInvocation(frame, zones, reason);
    public void Complete(IProgramOwnedZoneDiscardInvocation? invocation, IReadOnlyList<int> actualIds) => getCharacterBuZhiOlRuntime().CompleteNativeBuZhiDiscardInvocation((global::CardGame.Core.GameEngine.NativeBuZhiDiscardInvocationDraft? )invocation, actualIds);
}
