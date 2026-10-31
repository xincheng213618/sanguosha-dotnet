#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileJwHost(Func<RuntimeEntries.CharacterJiangWeiMobileGodEntries> getCharacterJiangWeiMobileGodRuntime) : IMobileJiangWeiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterJiangWeiMobileGodRuntime().BeginMobileJw(f, op);
}
