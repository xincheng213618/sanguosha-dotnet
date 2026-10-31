#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhangYiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangYiOlEntries> getCharacterZhangYiOlRuntime) : IZhangYiProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangYiOlRuntime().BeginZhangYi(f, op);
}
