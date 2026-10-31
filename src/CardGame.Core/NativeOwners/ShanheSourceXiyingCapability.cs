#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceXiyingCapability(Func<RuntimeEntries.CoreGameEngineShanheSourceXiyingEntries> getCoreGameEngineShanheSourceXiyingRuntime) : IShanheSourceXiyingHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCoreGameEngineShanheSourceXiyingRuntime().BeginSourceXiying(f);
}
