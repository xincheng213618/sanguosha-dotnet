#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMarketApplicationEntries
{
    internal IReadOnlyList<string> ApplyShanheMarketSkillSources(ProgramSkillFrame program, ShanheMarketPurchaseFrame market, Campaigns.ShanheRewardDefinition reward) => _resolve().ApplyShanheMarketSkillSources(program, market, reward);
    internal SkillProgramStepOutcome BeginShanheMarketApplication(ProgramSkillFrame supplied) => _resolve().BeginShanheMarketApplication(supplied);
    internal void ContinueShanheMarketApplication(long id) => _resolve().ContinueShanheMarketApplication(id);
    internal bool DrainShanheMarketChildren(ProgramSkillFrame frame) => _resolve().DrainShanheMarketChildren(frame);
    internal bool IsShanheMarketResource(int id) => _resolve().IsShanheMarketResource(id);
    internal Campaigns.ShanheRewardDefinition MarketPurchaseReward(ShanheMarketPurchaseFrame market) => _resolve().MarketPurchaseReward(market);
    internal void PlaceShanheMarketMaterials(ProgramSkillFrame program, ShanheMarketPurchaseFrame market, Campaigns.ShanheRewardDefinition reward) => _resolve().PlaceShanheMarketMaterials(program, market, reward);
    internal ShanheMarketPurchaseFrame RequireShanheMarketProgram(ProgramSkillFrame program) => _resolve().RequireShanheMarketProgram(program);
    internal bool ResumeShanheMarketApplication(long frameId) => _resolve().ResumeShanheMarketApplication(frameId);
    internal bool ReturnShanheMarketMovement(ProgramSkillFrame frame) => _resolve().ReturnShanheMarketMovement(frame);
}
