#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShownBoundGiftProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShownBoundGiftsEntries> getCoreGameEngineShownBoundGiftsRuntime) : IShownBoundGiftProgramHost
{
    public SkillProgramStepOutcome GiveShownBoundCardsAndGrantTurnHandLimit(ProgramSkillFrame f, string bind) => getCoreGameEngineShownBoundGiftsRuntime().GiveProgramShownBoundGift(f, bind);
}
