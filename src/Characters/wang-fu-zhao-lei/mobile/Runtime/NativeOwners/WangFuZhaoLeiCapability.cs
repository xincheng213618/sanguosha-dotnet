#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WangFuZhaoLeiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangFuZhaoLeiMobileEntries> getCharacterWangFuZhaoLeiMobileRuntime) : IWangFuZhaoLeiProgramHost
{
    public SkillProgramStepOutcome ExecuteXunyi(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterWangFuZhaoLeiMobileRuntime().ExecuteXunyi(frame, operation);
}
