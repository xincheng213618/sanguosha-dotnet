#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HuJinDingCapability(Func<RuntimeEntries.CharacterHuJinDingOlEntries> getCharacterHuJinDingOlRuntime) : IHuJinDingProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuJinDingOlRuntime().BeginQingyuan(f, op);
}
