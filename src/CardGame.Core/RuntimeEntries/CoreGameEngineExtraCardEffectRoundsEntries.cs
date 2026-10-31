#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraCardEffectRoundsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> _resolve;
    private CoreGameEngineExtraCardEffectRoundsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraCardEffectRoundsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> resolve) => new(resolve);
}
