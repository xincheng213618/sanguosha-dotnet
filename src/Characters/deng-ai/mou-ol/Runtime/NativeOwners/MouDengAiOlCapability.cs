#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouDengAiOlCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDengAiMouOlRuntime>> getCharacterDengAiMouOlRuntime) : IMouDengAiOlHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDengAiMouOlRuntime().Value.StartMouDengAiOl(f, op);
}
