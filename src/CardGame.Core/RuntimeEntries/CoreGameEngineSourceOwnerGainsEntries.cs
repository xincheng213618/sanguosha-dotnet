#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceOwnerGainsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> _resolve;
    private CoreGameEngineSourceOwnerGainsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceOwnerGainsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> resolve) => new(resolve);
}
