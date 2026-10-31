#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShownBoundGiftProgramCapability(Func<RuntimeEntries.CoreGameEngineShownBoundGiftsEntries> getCoreGameEngineShownBoundGiftsRuntime) : IShownBoundGiftProgramHost
{
    public SkillProgramStepOutcome GiveShownBoundCardsAndGrantTurnHandLimit(ProgramSkillFrame f, string bind) => getCoreGameEngineShownBoundGiftsRuntime().GiveProgramShownBoundGift(f, bind);
}
