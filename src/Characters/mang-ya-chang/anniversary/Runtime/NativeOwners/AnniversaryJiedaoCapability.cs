#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryJiedaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterMangYaChangAnniversaryRuntime>> getCharacterMangYaChangAnniversaryRuntime) : IAnniversaryJiedaoHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterMangYaChangAnniversaryRuntime().Value.BeginAnniversaryJiedao(frame, operation);
}
