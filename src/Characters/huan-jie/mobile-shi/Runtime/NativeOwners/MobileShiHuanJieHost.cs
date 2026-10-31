#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiHuanJieHost(Func<RuntimeEntries.CharacterHuanJieMobileShiEntries> getCharacterHuanJieMobileShiRuntime) : IMobileShiHuanJieHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterHuanJieMobileShiRuntime().BeginShiHj(frame, op);
}
