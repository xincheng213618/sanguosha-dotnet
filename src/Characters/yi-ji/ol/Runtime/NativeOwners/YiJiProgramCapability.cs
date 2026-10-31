#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YiJiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYiJiOlEntries> getCharacterYiJiOlRuntime) : IYiJiProgramHost
{
    public SkillProgramStepOutcome GiveDrawPileBottomCard(ProgramSkillFrame frame) => getCharacterYiJiOlRuntime().BeginGiveDrawPileBottomCard(frame);
}
