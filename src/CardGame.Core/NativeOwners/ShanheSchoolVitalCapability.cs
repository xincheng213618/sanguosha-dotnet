#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSchoolVitalCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSchoolVitalRuntime>> getCoreGameEngineShanheSchoolVitalRuntime) : IShanheSchoolVitalHost
{
    public void SetSchoolHpToMaximum(ProgramSkillFrame frame) => getCoreGameEngineShanheSchoolVitalRuntime().Value.SetShanheSchoolHpToMaximum(frame);
}
