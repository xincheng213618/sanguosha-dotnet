#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenHuaTuoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries> getCharacterHuaTuoAnniversaryGodRuntime) : IShenHuaTuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuaTuoAnniversaryGodRuntime().BeginShenHuaTuo(f, op);
}
