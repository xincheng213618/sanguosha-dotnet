#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class BuZhiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBuZhiOlEntries> getCharacterBuZhiOlRuntime) : IBuZhiProgramHost
{
    public SkillProgramStepOutcome ResolveBuZhi(ProgramSkillFrame f, bool gainLoss) => getCharacterBuZhiOlRuntime().ResolveBuZhi(f, gainLoss);
}
