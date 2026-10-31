#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineJianjiDeclarationsRuntime))]
internal sealed partial class CoreGameEngineJianjiDeclarationsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineJianjiDeclarationsRuntime> _resolve;
    private CoreGameEngineJianjiDeclarationsEntries(Func<EngineOwners.CoreGameEngineJianjiDeclarationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJianjiDeclarationsEntries Create(Func<EngineOwners.CoreGameEngineJianjiDeclarationsRuntime> resolve) => new(resolve);
}
