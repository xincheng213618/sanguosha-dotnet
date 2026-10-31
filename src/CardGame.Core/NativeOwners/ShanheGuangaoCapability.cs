#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheGuangaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheGuangaoEntries> getCoreGameEngineShanheGuangaoRuntime) : IShanheGuangaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCoreGameEngineShanheGuangaoRuntime().BeginShanheGuangao(frame);
}
