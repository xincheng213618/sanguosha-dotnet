#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceBiriCapability(Func<RuntimeEntries.CoreGameEngineShanheSourceBiriEntries> getCoreGameEngineShanheSourceBiriRuntime) : IShanheSourceBiriHost
{
    public SkillProgramStepOutcome PreventFireRecoverDraw(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceBiriRuntime().BeginShanheSourceBiri(frame);
}
