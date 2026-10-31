#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileJiKangHost(Func<RuntimeEntries.CharacterJiKangMobileEntries> getCharacterJiKangMobileRuntime) : IMobileJiKangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterJiKangMobileRuntime().BeginMobileJiKang(f, op);
}
