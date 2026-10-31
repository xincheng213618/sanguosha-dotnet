#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceDingxiNativeEntries
{
    internal void BindSourceDingxiNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindSourceDingxiNativeUse(use);
    internal bool IsSourceDingxiOriginalActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost) => _resolve().IsSourceDingxiOriginalActionCost(action, cost);
    internal bool IsSourceDingxiRetainedEntity(long useId, int cardId) => _resolve().IsSourceDingxiRetainedEntity(useId, cardId);
    internal void IssueSourceDingxiUse(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.GameEngine.SourceDingxiUsePlan plan) => _resolve().IssueSourceDingxiUse(f, plan);
    internal bool ResumeSourceDingxiAttack() => _resolve().ResumeSourceDingxiAttack();
    internal bool ReturnSourceDingxiUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnSourceDingxiUse(use);
    internal bool SourceDingxiEntry(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SourceDingxiReceipt r) => _resolve().SourceDingxiEntry(f, r);
    internal bool SourceDingxiUseRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame use) => _resolve().SourceDingxiUseRidesOn(f, use);
    internal void ValidateSourceDingxiUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateSourceDingxiUse(use);
}
