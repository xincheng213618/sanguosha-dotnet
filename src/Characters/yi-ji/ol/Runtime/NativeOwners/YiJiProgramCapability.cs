#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YiJiProgramCapability(Func<RuntimeEntries.CharacterYiJiOlEntries> getCharacterYiJiOlRuntime) : IYiJiProgramHost
{
    public SkillProgramStepOutcome GiveDrawPileBottomCard(ProgramSkillFrame frame) => getCharacterYiJiOlRuntime().BeginGiveDrawPileBottomCard(frame);
}
