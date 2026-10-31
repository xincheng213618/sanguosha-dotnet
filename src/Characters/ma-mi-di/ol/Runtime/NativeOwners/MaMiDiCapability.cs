#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MaMiDiCapability(Func<RuntimeEntries.CharacterMaMiDiOlEntries> getCharacterMaMiDiOlRuntime) : IMaMiDiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaMiDiOlRuntime().BeginMaMiDi(f, op);
}
