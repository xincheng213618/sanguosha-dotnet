#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduActivitiesEntries
{
    internal void ApplyGuanduDeathReward(CharacterState victim, CharacterState? killer) => _resolve().ApplyGuanduDeathReward(victim, killer);
    internal long CaptureGuanduQualificationStamp(long underlying) => _resolve().CaptureGuanduQualificationStamp(underlying);
    internal GuanduActivitySnapshot? GetGuanduSnapshot() => _resolve().GetGuanduSnapshot();
    internal IEnumerable<RuleQueryContribution> GuanduActivityRuleContributions(CharacterState player, SkillRuleQuery query) => _resolve().GuanduActivityRuleContributions(player, query);
    internal bool GuanduAllies(int first, int second) => _resolve().GuanduAllies(first, second);
    internal bool IsGuanduCommander(int seat) => _resolve().IsGuanduCommander(seat);
    internal bool IsGuanduCommanderPrintedLordGrant(CharacterState player, SkillGrant grant) => _resolve().IsGuanduCommanderPrintedLordGrant(player, grant);
    internal void RecordGuanduDeckExhaustion() => _resolve().RecordGuanduDeckExhaustion();
    internal void RecordGuanduSetupFact() => _resolve().RecordGuanduSetupFact();
    internal bool TryCompleteGuanduBattle() => _resolve().TryCompleteGuanduBattle();
    internal GuanduBattleConfiguredEvent? GuanduConfiguration { get => _resolve().GuanduConfiguration; }
    internal bool IsGuanduActivity { get => _resolve().IsGuanduActivity; }
}
