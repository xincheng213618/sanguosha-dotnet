#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiLuSuHost(Func<RuntimeEntries.CharacterLuSuMobileShiEntries> getCharacterLuSuMobileShiRuntime) : IShiLuSuHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuSuMobileShiRuntime().BeginShiLs(f, op);
}
