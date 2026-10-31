#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRequestedDeckBasicOwnershipEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime> _resolve;
    private CoreGameEngineRequestedDeckBasicOwnershipEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRequestedDeckBasicOwnershipEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime> resolve) => new(resolve);
}
