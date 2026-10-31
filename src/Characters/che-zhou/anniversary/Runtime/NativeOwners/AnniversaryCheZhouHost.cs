#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryCheZhouHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCheZhouAnniversaryEntries> getCharacterCheZhouAnniversaryRuntime) : IAnniversaryCheZhouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCheZhouAnniversaryRuntime().BeginAnniversaryCheZhouPigua(f);
}
