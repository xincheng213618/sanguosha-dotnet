#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNoDamageSkillDisableEntries
{
    internal void AssertNoDamageSkillDisablePolicy(CardUseFrame use) => _resolve().AssertNoDamageSkillDisablePolicy(use);
    internal void IssueNoDamageSkillDisablePolicy(long frameId) => _resolve().IssueNoDamageSkillDisablePolicy(frameId);
    internal void SettleNoDamageSkillDisable(CardUseFrame use) => _resolve().SettleNoDamageSkillDisable(use);
}
