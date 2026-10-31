#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileHuaTuoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuaTuoMobileGodEntries> getCharacterHuaTuoMobileGodRuntime) : IMobileHuaTuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterHuaTuoMobileGodRuntime().BeginMobileHt(frame, op);
}
