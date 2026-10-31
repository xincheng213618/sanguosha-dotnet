#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DuXiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuXiOlEntries> getCharacterDuXiOlRuntime) : IDuXiProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDuXiOlRuntime().BeginDuXi(f, op);
}
