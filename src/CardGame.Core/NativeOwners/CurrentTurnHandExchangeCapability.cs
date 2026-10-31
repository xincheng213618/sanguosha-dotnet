#nullable enable
using System.Collections.ObjectModel;
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class CurrentTurnHandExchangeCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnHandExchangeEntries> getCoreGameEngineCurrentTurnHandExchangeRuntime) : ICurrentTurnHandExchangeHost
{
    public SkillProgramStepOutcome OfferCurrentTurnHandExchange(ProgramSkillFrame frame) => getCoreGameEngineCurrentTurnHandExchangeRuntime().BeginCurrentTurnHandExchange(frame);
}
