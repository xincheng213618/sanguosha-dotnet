#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiWangChangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangChangMobileShiEntries> getCharacterWangChangMobileShiRuntime) : IMobileShiWangChangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterWangChangMobileShiRuntime().ResolveShiWangChang(frame, op);
}
