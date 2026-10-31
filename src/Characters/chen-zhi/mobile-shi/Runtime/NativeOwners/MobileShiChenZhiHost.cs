#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiChenZhiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChenZhiMobileShiEntries> getCharacterChenZhiMobileShiRuntime) : IMobileShiChenZhiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterChenZhiMobileShiRuntime().BeginShiCz(frame, operation);
}
