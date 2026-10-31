#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageAdjacencyFacts(Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreDamageCursorRuntimeEntries> getCoreDamageCursorRuntimeRuntime, Func<RuntimeEntries.CoreDyingCursorRuntimeEntries> getCoreDyingCursorRuntimeRuntime, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<ContentRegistry> getContentRegistry) : IDamageAdjacencyFacts
{
    public int FrameCount => getFrameStore().Count;

    public ResolutionFrame FrameAt(int index) => getFrameStore()[index];
    public int FrameIndex(long id) => getFrameStore().FindIndex(frame => frame.Id == id);
    public DamageTriggerWindowFrame? ActiveDamageTrigger => getCoreDamageCursorRuntimeRuntime().ActiveDamageTrigger;
    public DyingFrame? ActiveDying => getCoreDyingCursorRuntimeRuntime().ActiveDying;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;

    public SkillProgram? Program(string skillId) => getContentRegistry().Skills.TryGetValue(skillId, out var skill) ? skill.Program : null;
}
