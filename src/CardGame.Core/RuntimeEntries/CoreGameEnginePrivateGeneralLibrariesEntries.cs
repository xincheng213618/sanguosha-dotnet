#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateGeneralLibrariesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> _resolve;
    private CoreGameEnginePrivateGeneralLibrariesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateGeneralLibrariesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> resolve) => new(resolve);
}
