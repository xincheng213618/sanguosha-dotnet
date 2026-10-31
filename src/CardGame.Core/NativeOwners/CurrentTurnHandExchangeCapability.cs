#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CurrentTurnHandExchangeCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime>> getCoreGameEngineCurrentTurnHandExchangeRuntime) : ICurrentTurnHandExchangeHost
{
    public SkillProgramStepOutcome OfferCurrentTurnHandExchange(ProgramSkillFrame frame) => getCoreGameEngineCurrentTurnHandExchangeRuntime().Value.BeginCurrentTurnHandExchange(frame);
}
