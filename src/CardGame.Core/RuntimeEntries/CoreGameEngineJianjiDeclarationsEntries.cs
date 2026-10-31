#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJianjiDeclarationsRuntime))]
internal sealed partial class CoreGameEngineJianjiDeclarationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJianjiDeclarationsRuntime> _resolve;
    private CoreGameEngineJianjiDeclarationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJianjiDeclarationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJianjiDeclarationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJianjiDeclarationsRuntime> resolve) => new(resolve);
}
