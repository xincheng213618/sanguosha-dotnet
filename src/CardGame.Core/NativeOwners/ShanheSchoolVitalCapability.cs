#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSchoolVitalCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolVitalEntries> getCoreGameEngineShanheSchoolVitalRuntime) : IShanheSchoolVitalHost
{
    public void SetSchoolHpToMaximum(ProgramSkillFrame frame) => getCoreGameEngineShanheSchoolVitalRuntime().SetShanheSchoolHpToMaximum(frame);
}
