#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiWeiYanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries> getCharacterWeiYanMobileShiRuntime) : IMobileShiWeiYanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterWeiYanMobileShiRuntime().BeginMobileShiWeiYan(frame, operation);
}
