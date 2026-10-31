#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiDengAiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDengAiMobileShiEntries> getCharacterDengAiMobileShiRuntime) : IShiDengAiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDengAiMobileShiRuntime().BeginShiDa(f, op);
}
