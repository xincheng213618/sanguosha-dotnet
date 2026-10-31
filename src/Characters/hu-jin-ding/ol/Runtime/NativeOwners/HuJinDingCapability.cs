#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HuJinDingCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuJinDingOlEntries> getCharacterHuJinDingOlRuntime) : IHuJinDingProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuJinDingOlRuntime().BeginQingyuan(f, op);
}
