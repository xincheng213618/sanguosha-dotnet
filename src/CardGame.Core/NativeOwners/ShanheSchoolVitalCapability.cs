#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSchoolVitalCapability(Func<RuntimeEntries.CoreGameEngineShanheSchoolVitalEntries> getCoreGameEngineShanheSchoolVitalRuntime) : IShanheSchoolVitalHost
{
    public void SetSchoolHpToMaximum(ProgramSkillFrame frame) => getCoreGameEngineShanheSchoolVitalRuntime().SetShanheSchoolHpToMaximum(frame);
}
