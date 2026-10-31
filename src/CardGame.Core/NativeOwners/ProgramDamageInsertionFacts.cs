#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDamageInsertionFacts(global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries> getCoreGameEngineProgramChildResolutionRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries> getCoreDyingCursorRuntimeRuntime) : IProgramDamageInsertionFacts
{
    public int FrameCount => getFrameStore().Count;

    public ResolutionFrame FrameAt(int index) => getFrameStore()[index];
    public int FrameIndex(long frameId) => getFrameStore().FindIndex(frame => frame.Id == frameId);
    public bool HasIssuedLink(int index) => getFrameStore().HasIssuedLink(index);
    public bool HasConsistentChild(int index) => getCoreGameEngineProgramChildResolutionRuntime().ProgramChildResolutionEdge(index);
    public long? AttackOwnerFrameId => getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt?.ResolutionId;
    public long? DyingFrameId => getCoreDyingCursorRuntimeRuntime().ActiveDying?.Id;
}
