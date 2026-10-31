#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianClaimsRuntime> _resolve;
    private CoreGameEnginePindianClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianClaimsRuntime> resolve) => new(resolve);
}
