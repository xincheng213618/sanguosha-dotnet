#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiGuoYuanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoYuanMobileShiEntries> getCharacterGuoYuanMobileShiRuntime) : IShiGuoYuanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoYuanMobileShiRuntime().BeginShiGy(f, op);
}
