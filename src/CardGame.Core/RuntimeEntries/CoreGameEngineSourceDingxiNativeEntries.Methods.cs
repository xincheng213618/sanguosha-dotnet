#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceDingxiNativeEntries
{
    internal void BindSourceDingxiNativeUse(CardUseFrame use) => _resolve().BindSourceDingxiNativeUse(use);
    internal bool IsSourceDingxiOriginalActionCost(CardActionContext action, CardActionCost cost) => _resolve().IsSourceDingxiOriginalActionCost(action, cost);
    internal bool IsSourceDingxiRetainedEntity(long useId, int cardId) => _resolve().IsSourceDingxiRetainedEntity(useId, cardId);
    internal void IssueSourceDingxiUse(ProgramSkillFrame f, GameEngine.SourceDingxiUsePlan plan) => _resolve().IssueSourceDingxiUse(f, plan);
    internal bool ResumeSourceDingxiAttack() => _resolve().ResumeSourceDingxiAttack();
    internal bool ReturnSourceDingxiUse(CardUseFrame use) => _resolve().ReturnSourceDingxiUse(use);
    internal bool SourceDingxiEntry(ProgramSkillFrame f, SourceDingxiReceipt r) => _resolve().SourceDingxiEntry(f, r);
    internal bool SourceDingxiUseRidesOn(ProgramSkillFrame f, CardUseFrame use) => _resolve().SourceDingxiUseRidesOn(f, use);
    internal void ValidateSourceDingxiUse(CardUseFrame use) => _resolve().ValidateSourceDingxiUse(use);
}
