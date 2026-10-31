#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheMarketPurchaseCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheMarketApplicationEntries> getCoreGameEngineShanheMarketApplicationRuntime) : IShanheMarketPurchaseHost
{
    public SkillProgramStepOutcome ApplyShanheMarketPurchase(ProgramSkillFrame frame) => getCoreGameEngineShanheMarketApplicationRuntime().BeginShanheMarketApplication(frame);
}
