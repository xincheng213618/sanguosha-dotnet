#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WangFuZhaoLeiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWangFuZhaoLeiMobileRuntime>> getCharacterWangFuZhaoLeiMobileRuntime) : IWangFuZhaoLeiProgramHost
{
    public SkillProgramStepOutcome ExecuteXunyi(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterWangFuZhaoLeiMobileRuntime().Value.ExecuteXunyi(frame, operation);
}
