#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXuJingHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXuJingAnniversaryRuntime>> getCharacterXuJingAnniversaryRuntime) : IAnniversaryXuJingHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterXuJingAnniversaryRuntime().Value.BeginAnniversaryXuJing(frame, operation);
}
