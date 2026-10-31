#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSchoolTrickHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSchoolTrickRuntime>> getCoreGameEngineShanheSchoolTrickRuntime) : IShanheSchoolTrickHost
{
    public void RepeatCurrentDamageTrick(ProgramSkillFrame frame) => getCoreGameEngineShanheSchoolTrickRuntime().Value.IssueShanheSchoolTrick(frame);
}
