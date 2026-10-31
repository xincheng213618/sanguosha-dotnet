#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXiahouEnHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXiahouEnAnniversaryRuntime>> getCharacterXiahouEnAnniversaryRuntime) : IAnniversaryXiahouEnHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterXiahouEnAnniversaryRuntime().Value.BeginAnniversaryXiahouEn(f, operation);
}
