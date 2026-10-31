#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YiJiProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYiJiOlRuntime>> getCharacterYiJiOlRuntime) : IYiJiProgramHost
{
    public SkillProgramStepOutcome GiveDrawPileBottomCard(ProgramSkillFrame frame) => getCharacterYiJiOlRuntime().Value.BeginGiveDrawPileBottomCard(frame);
}
