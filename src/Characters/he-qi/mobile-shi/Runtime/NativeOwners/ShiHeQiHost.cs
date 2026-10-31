#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiHeQiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHeQiMobileShiEntries> getCharacterHeQiMobileShiRuntime) : IShiHeQiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHeQiMobileShiRuntime().BeginShiHq(f, op);
}
