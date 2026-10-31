#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceLiuSandaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceLiuSandaoEntries> getCoreGameEngineShanheSourceLiuSandaoRuntime) : IShanheSourceLiuSandaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreGameEngineShanheSourceLiuSandaoRuntime().ResolveShanheSourceLiuSandao(frame, operation);
}
