#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineComparedBlackSlashPoliciesEntries
{
    internal bool ApplyComparedBlackSlashPolicies(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ApplyComparedBlackSlashPolicies(attack);
    internal void AssertComparedBlackSlashReceipt(global::CardGame.Core.CardUseFrame use) => _resolve().AssertComparedBlackSlashReceipt(use);
    internal bool ComparedBlackSlashPolicyMatches(global::CardGame.Core.CardConversionSource? source, string? hash, int owner, global::CardGame.Core.SkillProgramCardPolicyKind kind) => _resolve().ComparedBlackSlashPolicyMatches(source, hash, owner, kind);
    internal bool HasComparedBlackSlashPolicies { get => _resolve().HasComparedBlackSlashPolicies; }
}
