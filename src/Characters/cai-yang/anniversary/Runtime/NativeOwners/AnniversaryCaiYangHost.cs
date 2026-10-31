#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryCaiYangHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaiYangAnniversaryRuntime>> getCharacterCaiYangAnniversaryRuntime) : IAnniversaryCaiYangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaiYangAnniversaryRuntime().Value.BeginCaiYang(f, op);
}
