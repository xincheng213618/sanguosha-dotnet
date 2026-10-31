#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeTrickEffectsRuntime))]
internal sealed partial class CoreNativeTrickEffectsEntries
{
    private readonly Func<EngineOwners.CoreNativeTrickEffectsRuntime> _resolve;
    private CoreNativeTrickEffectsEntries(Func<EngineOwners.CoreNativeTrickEffectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeTrickEffectsEntries Create(Func<EngineOwners.CoreNativeTrickEffectsRuntime> resolve) => new(resolve);
}
