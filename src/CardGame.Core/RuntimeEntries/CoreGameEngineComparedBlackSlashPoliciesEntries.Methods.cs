#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineComparedBlackSlashPoliciesEntries
{
    internal bool ApplyComparedBlackSlashPolicies(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ApplyComparedBlackSlashPolicies(attack);
    internal bool HasComparedBlackSlashPolicies { get => _resolve().HasComparedBlackSlashPolicies; }
}
