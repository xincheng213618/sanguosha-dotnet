#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHzCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries> getCharacterHuangZhongAnniversaryGodRuntime) : IAnniversaryHzHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuangZhongAnniversaryGodRuntime().BeginAnniversaryHz(f, op);
}
