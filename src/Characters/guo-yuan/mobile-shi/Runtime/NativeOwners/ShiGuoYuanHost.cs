#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiGuoYuanHost(Func<RuntimeEntries.CharacterGuoYuanMobileShiEntries> getCharacterGuoYuanMobileShiRuntime) : IShiGuoYuanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoYuanMobileShiRuntime().BeginShiGy(f, op);
}
