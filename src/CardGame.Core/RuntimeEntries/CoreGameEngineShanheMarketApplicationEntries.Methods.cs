#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMarketApplicationEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<string> ApplyShanheMarketSkillSources(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.ShanheMarketPurchaseFrame market, global::CardGame.Core.Campaigns.ShanheRewardDefinition reward) => _resolve().ApplyShanheMarketSkillSources(program, market, reward);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheMarketApplication(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheMarketApplication(supplied);
    internal void ContinueShanheMarketApplication(long id) => _resolve().ContinueShanheMarketApplication(id);
    internal bool DrainShanheMarketChildren(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainShanheMarketChildren(frame);
    internal bool IsShanheMarketResource(int id) => _resolve().IsShanheMarketResource(id);
    internal global::CardGame.Core.Campaigns.ShanheRewardDefinition MarketPurchaseReward(global::CardGame.Core.ShanheMarketPurchaseFrame market) => _resolve().MarketPurchaseReward(market);
    internal void PlaceShanheMarketMaterials(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.ShanheMarketPurchaseFrame market, global::CardGame.Core.Campaigns.ShanheRewardDefinition reward) => _resolve().PlaceShanheMarketMaterials(program, market, reward);
    internal global::CardGame.Core.ShanheMarketPurchaseFrame RequireShanheMarketProgram(global::CardGame.Core.ProgramSkillFrame program) => _resolve().RequireShanheMarketProgram(program);
    internal bool ResumeShanheMarketApplication(long frameId) => _resolve().ResumeShanheMarketApplication(frameId);
    internal bool ReturnShanheMarketMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnShanheMarketMovement(frame);
}
