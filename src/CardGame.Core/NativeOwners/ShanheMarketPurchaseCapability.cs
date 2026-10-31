#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheMarketPurchaseCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheMarketApplicationRuntime>> getCoreGameEngineShanheMarketApplicationRuntime) : IShanheMarketPurchaseHost
{
    public SkillProgramStepOutcome ApplyShanheMarketPurchase(ProgramSkillFrame frame) => getCoreGameEngineShanheMarketApplicationRuntime().Value.BeginShanheMarketApplication(frame);
}
