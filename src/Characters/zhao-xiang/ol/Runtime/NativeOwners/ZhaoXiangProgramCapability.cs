#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ZhaoXiangProgramCapability(Func<RuntimeEntries.CharacterZhaoXiangOlEntries> getCharacterZhaoXiangOlRuntime) : IZhaoXiangProgramHost
{
    public void GainMeiYingAfterActualSlashDamage(ProgramSkillFrame f) => getCharacterZhaoXiangOlRuntime().GainMeiYingAfterActualSlashDamage(f);
    public SkillProgramStepOutcome ExchangeFiveShuBySpentMeiYing(ProgramSkillFrame f) => getCharacterZhaoXiangOlRuntime().ExchangeFiveShuBySpentMeiYing(f);
    public SkillProgramStepOutcome DrawAfterPaidMeiYingConversion(ProgramSkillFrame f) => getCharacterZhaoXiangOlRuntime().DrawAfterPaidMeiYingConversion(f);
}
