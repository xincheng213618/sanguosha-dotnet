#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActivityBattleRulesEntries
{
    internal bool DrainActivityRuleChildren(long frameId) => _resolve().DrainActivityRuleChildren(frameId);
    internal global::CardGame.Core.IDragonBoatBattleRulesFeature? DragonBoatRules { get => _resolve().DragonBoatRules; }
    internal global::CardGame.Core.IGuanduBattleRulesFeature? GuanduRules { get => _resolve().GuanduRules; }
    internal global::CardGame.Core.IWenheBattleRulesFeature? WenheRules { get => _resolve().WenheRules; }
}
