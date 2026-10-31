#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiDongZhaoHost(Func<RuntimeEntries.CharacterDongZhaoMobileShiEntries> getCharacterDongZhaoMobileShiRuntime) : IMobileShiDongZhaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterDongZhaoMobileShiRuntime().BeginShiDz(frame, op);
}
