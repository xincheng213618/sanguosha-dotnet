#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShiSunChenHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunChenMobileShiEntries> getCharacterSunChenMobileShiRuntime) : IShiSunChenHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunChenMobileShiRuntime().BeginShiSc(f, op);
}
