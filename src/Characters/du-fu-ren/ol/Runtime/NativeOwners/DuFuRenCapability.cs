#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DuFuRenCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime) : IDuFuRenHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDuFuRenOlRuntime().BeginDuFuRen(f, op);
}
