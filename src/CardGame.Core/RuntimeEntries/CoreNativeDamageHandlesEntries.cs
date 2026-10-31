#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed partial class CoreNativeDamageHandlesEntries
{
    private readonly Func<EngineOwners.CoreNativeDamageHandlesRuntime> _resolve;
    private CoreNativeDamageHandlesEntries(Func<EngineOwners.CoreNativeDamageHandlesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageHandlesEntries Create(Func<EngineOwners.CoreNativeDamageHandlesRuntime> resolve) => new(resolve);
}
