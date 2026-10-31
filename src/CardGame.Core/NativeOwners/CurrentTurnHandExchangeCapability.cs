#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CurrentTurnHandExchangeCapability(Func<RuntimeEntries.CoreGameEngineCurrentTurnHandExchangeEntries> getCoreGameEngineCurrentTurnHandExchangeRuntime) : ICurrentTurnHandExchangeHost
{
    public SkillProgramStepOutcome OfferCurrentTurnHandExchange(ProgramSkillFrame frame) => getCoreGameEngineCurrentTurnHandExchangeRuntime().BeginCurrentTurnHandExchange(frame);
}
