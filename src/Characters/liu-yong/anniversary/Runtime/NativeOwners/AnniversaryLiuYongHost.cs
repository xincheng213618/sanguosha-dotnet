#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLiuYongHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiuYongAnniversaryRuntime>> getCharacterLiuYongAnniversaryRuntime) : IAnniversaryLiuYongHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuYongAnniversaryRuntime().Value.BeginAnniversaryLiuYong(f, op);
}
