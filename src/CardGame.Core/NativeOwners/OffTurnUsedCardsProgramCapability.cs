#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class OffTurnUsedCardsProgramCapability(Func<RuntimeEntries.CoreGameEngineOffTurnUsedCardsEntries> getCoreGameEngineOffTurnUsedCardsRuntime) : IOffTurnUsedCardsProgramHost
{
    public SkillProgramStepOutcome GiveOffTurnUsedCards(ProgramSkillFrame f) => getCoreGameEngineOffTurnUsedCardsRuntime().BeginOffTurnUsedCardGift(f);
}
