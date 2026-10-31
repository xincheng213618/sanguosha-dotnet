#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduActivitiesEntries
{
    internal void ApplyGuanduDeathReward(global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer) => _resolve().ApplyGuanduDeathReward(victim, killer);
    internal long CaptureGuanduQualificationStamp(long underlying) => _resolve().CaptureGuanduQualificationStamp(underlying);
    internal global::CardGame.Core.GuanduActivitySnapshot? GetGuanduSnapshot() => _resolve().GetGuanduSnapshot();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> GuanduActivityRuleContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().GuanduActivityRuleContributions(player, query);
    internal bool GuanduAllies(int first, int second) => _resolve().GuanduAllies(first, second);
    internal bool IsGuanduCommander(int seat) => _resolve().IsGuanduCommander(seat);
    internal bool IsGuanduCommanderPrintedLordGrant(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillGrant grant) => _resolve().IsGuanduCommanderPrintedLordGrant(player, grant);
    internal void RecordGuanduDeckExhaustion() => _resolve().RecordGuanduDeckExhaustion();
    internal void RecordGuanduSetupFact() => _resolve().RecordGuanduSetupFact();
    internal bool TryCompleteGuanduBattle() => _resolve().TryCompleteGuanduBattle();
    internal global::CardGame.Core.GuanduBattleConfiguredEvent? GuanduConfiguration { get => _resolve().GuanduConfiguration; }
    internal bool IsGuanduActivity { get => _resolve().IsGuanduActivity; }
}
