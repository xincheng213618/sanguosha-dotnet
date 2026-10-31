#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouFaZhengCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterFaZhengMouMobileEntries> getCharacterFaZhengMouMobileRuntime) : IMouFaZhengHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterFaZhengMouMobileRuntime().StartMouFaZheng(f, op);
}
