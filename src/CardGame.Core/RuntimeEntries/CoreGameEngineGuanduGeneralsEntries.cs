#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralsRuntime> _resolve;
    private CoreGameEngineGuanduGeneralsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralsRuntime> resolve) => new(resolve);
}
