#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryGuoZhaoHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGuoZhaoAnniversaryRuntime>> getCharacterGuoZhaoAnniversaryRuntime) : IAnniversaryGuoZhaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoZhaoAnniversaryRuntime().Value.BeginGuoZhao(f, op);
}
