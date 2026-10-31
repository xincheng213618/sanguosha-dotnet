#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKunlunMirrorEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKunlunMirrorRuntime> _resolve;
    private CoreGameEngineKunlunMirrorEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKunlunMirrorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineKunlunMirrorEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKunlunMirrorRuntime> resolve) => new(resolve);
}
