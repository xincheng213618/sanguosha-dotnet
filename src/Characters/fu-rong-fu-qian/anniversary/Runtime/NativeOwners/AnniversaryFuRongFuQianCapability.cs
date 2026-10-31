#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryFuRongFuQianCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterFuRongFuQianAnniversaryRuntime>> getCharacterFuRongFuQianAnniversaryRuntime) : IAnniversaryFuRongFuQianHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterFuRongFuQianAnniversaryRuntime().Value.BeginAnniversaryFuPair(f, op);
}
