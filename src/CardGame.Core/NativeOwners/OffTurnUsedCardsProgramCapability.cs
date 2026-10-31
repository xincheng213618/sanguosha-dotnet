#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class OffTurnUsedCardsProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOffTurnUsedCardsEntries> getCoreGameEngineOffTurnUsedCardsRuntime) : IOffTurnUsedCardsProgramHost
{
    public SkillProgramStepOutcome GiveOffTurnUsedCards(ProgramSkillFrame f) => getCoreGameEngineOffTurnUsedCardsRuntime().BeginOffTurnUsedCardGift(f);
}
