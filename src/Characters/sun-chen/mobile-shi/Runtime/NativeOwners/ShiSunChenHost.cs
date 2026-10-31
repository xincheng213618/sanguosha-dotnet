#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShiSunChenHost(Func<RuntimeEntries.CharacterSunChenMobileShiEntries> getCharacterSunChenMobileShiRuntime) : IShiSunChenHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunChenMobileShiRuntime().BeginShiSc(f, op);
}
