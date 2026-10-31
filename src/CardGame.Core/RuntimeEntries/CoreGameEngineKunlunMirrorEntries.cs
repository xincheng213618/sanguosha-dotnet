#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKunlunMirrorEntries
{
    private readonly Func<EngineOwners.CoreGameEngineKunlunMirrorRuntime> _resolve;
    private CoreGameEngineKunlunMirrorEntries(Func<EngineOwners.CoreGameEngineKunlunMirrorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineKunlunMirrorEntries Create(Func<EngineOwners.CoreGameEngineKunlunMirrorRuntime> resolve) => new(resolve);
}
