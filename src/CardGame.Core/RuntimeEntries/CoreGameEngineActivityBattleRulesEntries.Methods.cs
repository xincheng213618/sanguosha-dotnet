#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActivityBattleRulesEntries
{
    internal bool DrainActivityRuleChildren(long frameId) => _resolve().DrainActivityRuleChildren(frameId);
    internal IDragonBoatBattleRulesFeature? DragonBoatRules { get => _resolve().DragonBoatRules; }
    internal IGuanduBattleRulesFeature? GuanduRules { get => _resolve().GuanduRules; }
    internal IWenheBattleRulesFeature? WenheRules { get => _resolve().WenheRules; }
}
