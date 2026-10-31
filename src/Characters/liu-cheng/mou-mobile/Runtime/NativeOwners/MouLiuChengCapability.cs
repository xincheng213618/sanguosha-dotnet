#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouLiuChengCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries> getCharacterLiuChengMouMobileRuntime) : IMouLiuChengHost
{
    public SkillProgramStepOutcome Accrue(ProgramSkillFrame f) => getCharacterLiuChengMouMobileRuntime().AccrueMouLiuCheng(f);
    public SkillProgramStepOutcome Complete(ProgramSkillFrame f) => getCharacterLiuChengMouMobileRuntime().BeginMouLiuChengCompletion(f);
}
