#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePassiveDamageCapsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> _resolve;
    private CoreGameEnginePassiveDamageCapsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePassiveDamageCapsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> resolve) => new(resolve);
}
