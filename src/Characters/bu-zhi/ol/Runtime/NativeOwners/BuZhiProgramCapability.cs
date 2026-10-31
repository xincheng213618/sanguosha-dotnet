#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BuZhiProgramCapability(Func<RuntimeEntries.CharacterBuZhiOlEntries> getCharacterBuZhiOlRuntime) : IBuZhiProgramHost
{
    public SkillProgramStepOutcome ResolveBuZhi(ProgramSkillFrame f, bool gainLoss) => getCharacterBuZhiOlRuntime().ResolveBuZhi(f, gainLoss);
}
