#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouDengAiOlCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDengAiMouOlEntries> getCharacterDengAiMouOlRuntime) : IMouDengAiOlHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDengAiMouOlRuntime().StartMouDengAiOl(f, op);
}
