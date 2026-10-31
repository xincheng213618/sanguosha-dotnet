#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheMarketPurchaseCapability(Func<RuntimeEntries.CoreGameEngineShanheMarketApplicationEntries> getCoreGameEngineShanheMarketApplicationRuntime) : IShanheMarketPurchaseHost
{
    public SkillProgramStepOutcome ApplyShanheMarketPurchase(ProgramSkillFrame frame) => getCoreGameEngineShanheMarketApplicationRuntime().BeginShanheMarketApplication(frame);
}
