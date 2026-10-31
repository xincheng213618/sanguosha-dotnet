#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMuluEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMuluRuntime> _resolve;
    private CoreGameEngineShanheMuluEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMuluRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMuluEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMuluRuntime> resolve) => new(resolve);
}
