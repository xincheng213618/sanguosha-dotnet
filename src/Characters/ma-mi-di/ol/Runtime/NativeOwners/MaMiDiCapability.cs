#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MaMiDiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMaMiDiOlEntries> getCharacterMaMiDiOlRuntime) : IMaMiDiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaMiDiOlRuntime().BeginMaMiDi(f, op);
}
