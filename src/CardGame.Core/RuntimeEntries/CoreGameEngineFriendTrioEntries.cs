#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFriendTrioRuntime))]
internal sealed partial class CoreGameEngineFriendTrioEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFriendTrioRuntime> _resolve;
    private CoreGameEngineFriendTrioEntries(Func<EngineOwners.CoreGameEngineFriendTrioRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFriendTrioEntries Create(Func<EngineOwners.CoreGameEngineFriendTrioRuntime> resolve) => new(resolve);
}
