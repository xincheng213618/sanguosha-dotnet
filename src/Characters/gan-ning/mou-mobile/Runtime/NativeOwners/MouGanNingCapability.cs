#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouGanNingCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGanNingMouMobileEntries> getCharacterGanNingMouMobileRuntime) : IMouGanNingHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGanNingMouMobileRuntime().BeginMouGn(f, op);
}
