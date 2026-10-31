#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeCardEffectDrawsRuntime))]
internal sealed partial class CoreNativeCardEffectDrawsEntries
{
    private readonly Func<EngineOwners.CoreNativeCardEffectDrawsRuntime> _resolve;
    private CoreNativeCardEffectDrawsEntries(Func<EngineOwners.CoreNativeCardEffectDrawsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardEffectDrawsEntries Create(Func<EngineOwners.CoreNativeCardEffectDrawsRuntime> resolve) => new(resolve);
}
