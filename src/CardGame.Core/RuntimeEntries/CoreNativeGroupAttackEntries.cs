#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeGroupAttackEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime> _resolve;
    private CoreNativeGroupAttackEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeGroupAttackEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime> resolve) => new(resolve);
}
