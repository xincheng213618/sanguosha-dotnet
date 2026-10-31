#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class FuRongProgramCapability(Func<RuntimeEntries.CharacterFuRongOlEntries> getCharacterFuRongOlRuntime) : IFuRongProgramHost
{
    public SkillProgramStepOutcome DiscardBasicPairAndReuse(ProgramSkillFrame f, int target) => getCharacterFuRongOlRuntime().BeginFuRongPair(f, target);
}
