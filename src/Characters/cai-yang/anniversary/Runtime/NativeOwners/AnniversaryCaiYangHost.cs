#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryCaiYangHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaiYangAnniversaryEntries> getCharacterCaiYangAnniversaryRuntime) : IAnniversaryCaiYangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaiYangAnniversaryRuntime().BeginCaiYang(f, op);
}
