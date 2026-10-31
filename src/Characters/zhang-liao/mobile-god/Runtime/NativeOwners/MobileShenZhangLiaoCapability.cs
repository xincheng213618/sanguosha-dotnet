#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShenZhangLiaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries> getCharacterZhangLiaoMobileGodRuntime) : IMobileShenZhangLiaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangLiaoMobileGodRuntime().BeginMobileShenZhangLiao(f, op);
}
