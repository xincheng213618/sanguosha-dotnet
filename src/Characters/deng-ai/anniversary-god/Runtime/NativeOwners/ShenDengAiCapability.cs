#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenDengAiCapability(Func<RuntimeEntries.CharacterDengAiAnniversaryGodEntries> getCharacterDengAiAnniversaryGodRuntime) : IShenDengAiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDengAiAnniversaryGodRuntime().BeginShenDa(f, op);
}
