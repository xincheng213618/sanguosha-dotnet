#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime))]
internal sealed partial class CoreGameEngineComparedBlackSlashPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> _resolve;
    private CoreGameEngineComparedBlackSlashPoliciesEntries(Func<EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineComparedBlackSlashPoliciesEntries Create(Func<EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> resolve) => new(resolve);
}
