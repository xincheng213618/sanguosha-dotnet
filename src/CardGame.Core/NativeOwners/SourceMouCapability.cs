#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SourceMouCapability(Func<RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoEntries> getCoreGameEngineShanheSourceMouDongZhuoRuntime) : IShanheSourceMouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreGameEngineShanheSourceMouDongZhuoRuntime().StartSourceMou(frame, operation);
}
