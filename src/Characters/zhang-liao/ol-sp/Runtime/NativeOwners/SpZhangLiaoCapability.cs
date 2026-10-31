#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SpZhangLiaoCapability(Func<RuntimeEntries.CharacterZhangLiaoOlSpEntries> getCharacterZhangLiaoOlSpRuntime) : ISpZhangLiaoProgramHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangLiaoOlSpRuntime().BeginSpZhangLiao(f, op);
}
