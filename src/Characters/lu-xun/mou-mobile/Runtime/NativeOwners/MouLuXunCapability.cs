#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuXunCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuXunMouMobileEntries> getCharacterLuXunMouMobileRuntime) : IMouLuXunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterLuXunMouMobileRuntime().BeginMouLuXun(f, operation);
}
