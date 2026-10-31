#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiHuangZuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangZuMobileShiEntries> getCharacterHuangZuMobileShiRuntime) : IShiHuangZuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuangZuMobileShiRuntime().BeginShiHz(f, op);
}
