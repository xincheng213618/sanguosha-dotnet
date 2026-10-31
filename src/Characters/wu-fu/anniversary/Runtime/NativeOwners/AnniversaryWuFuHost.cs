#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryWuFuHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWuFuAnniversaryRuntime>> getCharacterWuFuAnniversaryRuntime) : IAnniversaryWuFuHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWuFuAnniversaryRuntime().Value.BeginWuFu(f, op);
}
