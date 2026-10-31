#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineComparedBlackSlashPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> _resolve;
    private CoreGameEngineComparedBlackSlashPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineComparedBlackSlashPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> resolve) => new(resolve);
}
