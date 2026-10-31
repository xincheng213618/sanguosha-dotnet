#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiCaoZHost(Func<RuntimeEntries.CharacterCaoZhenMobileShiEntries> getCharacterCaoZhenMobileShiRuntime) : IShiCaoZHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterCaoZhenMobileShiRuntime().BeginShiCaoZ(f, operation);
}
