#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiCaoZHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoZhenMobileShiEntries> getCharacterCaoZhenMobileShiRuntime) : IShiCaoZHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterCaoZhenMobileShiRuntime().BeginShiCaoZ(f, operation);
}
