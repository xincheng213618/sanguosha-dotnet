#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiZhangYanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangYanMobileShiEntries> getCharacterZhangYanMobileShiRuntime) : IShiZhangYanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangYanMobileShiRuntime().BeginShiZy(f, op);
}
