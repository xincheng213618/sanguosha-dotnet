#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WushuangDeckCardsCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineWushuangDeckCardsRuntime>> getCoreGameEngineWushuangDeckCardsRuntime) : IWushuangDeckCardsHost
{
    public SkillProgramStepOutcome ObtainWushuangDeckCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineWushuangDeckCardsRuntime().Value.ObtainWushuangDeckCards(frame, effect);
}
