#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheGuangaoCapability(Func<RuntimeEntries.CoreGameEngineShanheGuangaoEntries> getCoreGameEngineShanheGuangaoRuntime) : IShanheGuangaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCoreGameEngineShanheGuangaoRuntime().BeginShanheGuangao(frame);
}
