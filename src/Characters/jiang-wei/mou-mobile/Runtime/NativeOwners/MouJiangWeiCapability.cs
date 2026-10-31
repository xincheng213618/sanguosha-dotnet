#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouJiangWeiCapability(Func<RuntimeEntries.CharacterJiangWeiMouMobileEntries> getCharacterJiangWeiMouMobileRuntime) : IMouJiangWeiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterJiangWeiMouMobileRuntime().BeginMouJw(f, op);
}
