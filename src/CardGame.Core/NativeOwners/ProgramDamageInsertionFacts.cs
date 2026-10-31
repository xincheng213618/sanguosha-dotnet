#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDamageInsertionFacts(Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramChildResolutionEntries> getCoreGameEngineProgramChildResolutionRuntime, Func<RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreDyingCursorRuntimeRuntime>> getCoreDyingCursorRuntimeRuntime) : IProgramDamageInsertionFacts
{
    public int FrameCount => getFrameStore().Count;

    public ResolutionFrame FrameAt(int index) => getFrameStore()[index];
    public int FrameIndex(long frameId) => getFrameStore().FindIndex(frame => frame.Id == frameId);
    public bool HasIssuedLink(int index) => getFrameStore().HasIssuedLink(index);
    public bool HasConsistentChild(int index) => getCoreGameEngineProgramChildResolutionRuntime().ProgramChildResolutionEdge(index);
    public long? AttackOwnerFrameId => getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt?.ResolutionId;
    public long? DyingFrameId => getCoreDyingCursorRuntimeRuntime().Value.ActiveDying?.Id;
}
