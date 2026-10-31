#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShownBoundGiftProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShownBoundGiftsRuntime>> getCoreGameEngineShownBoundGiftsRuntime) : IShownBoundGiftProgramHost
{
    public SkillProgramStepOutcome GiveShownBoundCardsAndGrantTurnHandLimit(ProgramSkillFrame f, string bind) => getCoreGameEngineShownBoundGiftsRuntime().Value.GiveProgramShownBoundGift(f, bind);
}
