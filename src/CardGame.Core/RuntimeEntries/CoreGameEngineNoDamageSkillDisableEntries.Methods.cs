#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNoDamageSkillDisableEntries
{
    internal void AssertNoDamageSkillDisablePolicy(global::CardGame.Core.CardUseFrame use) => _resolve().AssertNoDamageSkillDisablePolicy(use);
    internal void IssueNoDamageSkillDisablePolicy(long frameId) => _resolve().IssueNoDamageSkillDisablePolicy(frameId);
    internal void SettleNoDamageSkillDisable(global::CardGame.Core.CardUseFrame use) => _resolve().SettleNoDamageSkillDisable(use);
}
