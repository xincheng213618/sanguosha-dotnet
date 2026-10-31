#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouLiuChengCapability(Func<RuntimeEntries.CharacterLiuChengMouMobileEntries> getCharacterLiuChengMouMobileRuntime) : IMouLiuChengHost
{
    public SkillProgramStepOutcome Accrue(ProgramSkillFrame f) => getCharacterLiuChengMouMobileRuntime().AccrueMouLiuCheng(f);
    public SkillProgramStepOutcome Complete(ProgramSkillFrame f) => getCharacterLiuChengMouMobileRuntime().BeginMouLiuChengCompletion(f);
}
