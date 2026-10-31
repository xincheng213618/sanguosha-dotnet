#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryJiedaoCapability(Func<RuntimeEntries.CharacterMangYaChangAnniversaryEntries> getCharacterMangYaChangAnniversaryRuntime) : IAnniversaryJiedaoHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterMangYaChangAnniversaryRuntime().BeginAnniversaryJiedao(frame, operation);
}
