#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WushuangDeckCardsCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangDeckCardsEntries> getCoreGameEngineWushuangDeckCardsRuntime) : IWushuangDeckCardsHost
{
    public SkillProgramStepOutcome ObtainWushuangDeckCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineWushuangDeckCardsRuntime().ObtainWushuangDeckCards(frame, effect);
}
