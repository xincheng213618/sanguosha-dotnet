#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouDengAiOlCapability(Func<RuntimeEntries.CharacterDengAiMouOlEntries> getCharacterDengAiMouOlRuntime) : IMouDengAiOlHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDengAiMouOlRuntime().StartMouDengAiOl(f, op);
}
