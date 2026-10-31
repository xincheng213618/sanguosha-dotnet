#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineIdentityAppearanceInvariantsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityAppearanceInvariantsRuntime> _resolve;
    private CoreGameEngineIdentityAppearanceInvariantsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityAppearanceInvariantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineIdentityAppearanceInvariantsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityAppearanceInvariantsRuntime> resolve) => new(resolve);
}
