#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiLuSuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries> getCharacterLuSuMobileShiRuntime) : IShiLuSuHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuSuMobileShiRuntime().BeginShiLs(f, op);
}
