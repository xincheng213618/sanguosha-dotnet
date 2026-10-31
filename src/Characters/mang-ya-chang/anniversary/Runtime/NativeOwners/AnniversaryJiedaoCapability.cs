#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryJiedaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMangYaChangAnniversaryEntries> getCharacterMangYaChangAnniversaryRuntime) : IAnniversaryJiedaoHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterMangYaChangAnniversaryRuntime().BeginAnniversaryJiedao(frame, operation);
}
