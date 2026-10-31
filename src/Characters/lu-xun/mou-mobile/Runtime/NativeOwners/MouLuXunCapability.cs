#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuXunCapability(Func<RuntimeEntries.CharacterLuXunMouMobileEntries> getCharacterLuXunMouMobileRuntime) : IMouLuXunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterLuXunMouMobileRuntime().BeginMouLuXun(f, operation);
}
