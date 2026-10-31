#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiDongZhaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDongZhaoMobileShiEntries> getCharacterDongZhaoMobileShiRuntime) : IMobileShiDongZhaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterDongZhaoMobileShiRuntime().BeginShiDz(frame, op);
}
