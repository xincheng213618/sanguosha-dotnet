#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryCaiYangHost(Func<RuntimeEntries.CharacterCaiYangAnniversaryEntries> getCharacterCaiYangAnniversaryRuntime) : IAnniversaryCaiYangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaiYangAnniversaryRuntime().BeginCaiYang(f, op);
}
