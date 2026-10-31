#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WangFuZhaoLeiCapability(Func<RuntimeEntries.CharacterWangFuZhaoLeiMobileEntries> getCharacterWangFuZhaoLeiMobileRuntime) : IWangFuZhaoLeiProgramHost
{
    public SkillProgramStepOutcome ExecuteXunyi(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterWangFuZhaoLeiMobileRuntime().ExecuteXunyi(frame, operation);
}
