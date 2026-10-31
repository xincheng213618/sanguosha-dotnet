#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class FuRongProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterFuRongOlEntries> getCharacterFuRongOlRuntime) : IFuRongProgramHost
{
    public SkillProgramStepOutcome DiscardBasicPairAndReuse(ProgramSkillFrame f, int target) => getCharacterFuRongOlRuntime().BeginFuRongPair(f, target);
}
