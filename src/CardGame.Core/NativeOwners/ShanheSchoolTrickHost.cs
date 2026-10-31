#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSchoolTrickHost(Func<RuntimeEntries.CoreGameEngineShanheSchoolTrickEntries> getCoreGameEngineShanheSchoolTrickRuntime) : IShanheSchoolTrickHost
{
    public void RepeatCurrentDamageTrick(ProgramSkillFrame frame) => getCoreGameEngineShanheSchoolTrickRuntime().IssueShanheSchoolTrick(frame);
}
