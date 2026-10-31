#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedCardAcquisitionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime> _resolve;
    private CoreGameEngineNamedCardAcquisitionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedCardAcquisitionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime> resolve) => new(resolve);
}
