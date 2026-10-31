#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LeCaiYongFeibaiCapability(Func<RuntimeEntries.CharacterCaiYongLeEntries> getCharacterCaiYongLeRuntime) : ILeCaiYongFeibaiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaiYongLeRuntime().BeginLeCaiYongFeibai(f);
}
