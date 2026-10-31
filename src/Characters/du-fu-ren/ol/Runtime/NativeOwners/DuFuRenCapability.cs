#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DuFuRenCapability(Func<RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime) : IDuFuRenHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDuFuRenOlRuntime().BeginDuFuRen(f, op);
}
