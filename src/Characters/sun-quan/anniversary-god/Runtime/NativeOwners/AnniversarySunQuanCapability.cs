#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversarySunQuanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunQuanAnniversaryGodEntries> getCharacterSunQuanAnniversaryGodRuntime) : IAnniversarySunQuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterSunQuanAnniversaryGodRuntime().BeginAnniversarySunQuan(f, operation);
}
