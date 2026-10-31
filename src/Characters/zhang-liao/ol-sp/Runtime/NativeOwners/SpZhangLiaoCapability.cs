#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SpZhangLiaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangLiaoOlSpRuntime>> getCharacterZhangLiaoOlSpRuntime) : ISpZhangLiaoProgramHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangLiaoOlSpRuntime().Value.BeginSpZhangLiao(f, op);
}
