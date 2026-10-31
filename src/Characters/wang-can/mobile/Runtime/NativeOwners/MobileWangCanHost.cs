#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileWangCanHost(Func<RuntimeEntries.CharacterWangCanMobileEntries> getCharacterWangCanMobileRuntime) : IMobileWangCanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterWangCanMobileRuntime().BeginMobileWangCanXi(frame, op);
}
