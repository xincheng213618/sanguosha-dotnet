#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenZhangFeiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries> getCharacterZhangFeiAnniversaryGodRuntime) : IShenZhangFeiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangFeiAnniversaryGodRuntime().BeginShenZf(f, op);
}
