#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenDengAiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDengAiAnniversaryGodEntries> getCharacterDengAiAnniversaryGodRuntime) : IShenDengAiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDengAiAnniversaryGodRuntime().BeginShenDa(f, op);
}
