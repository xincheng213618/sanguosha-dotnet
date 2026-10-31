#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSchoolTrickHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolTrickEntries> getCoreGameEngineShanheSchoolTrickRuntime) : IShanheSchoolTrickHost
{
    public void RepeatCurrentDamageTrick(ProgramSkillFrame frame) => getCoreGameEngineShanheSchoolTrickRuntime().IssueShanheSchoolTrick(frame);
}
