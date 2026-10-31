#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DuXiCapability(Func<RuntimeEntries.CharacterDuXiOlEntries> getCharacterDuXiOlRuntime) : IDuXiProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDuXiOlRuntime().BeginDuXi(f, op);
}
