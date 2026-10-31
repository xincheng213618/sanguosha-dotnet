#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeCardClassificationsRuntime))]
internal sealed partial class CoreNativeCardClassificationsEntries
{
    private readonly Func<EngineOwners.CoreNativeCardClassificationsRuntime> _resolve;
    private CoreNativeCardClassificationsEntries(Func<EngineOwners.CoreNativeCardClassificationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardClassificationsEntries Create(Func<EngineOwners.CoreNativeCardClassificationsRuntime> resolve) => new(resolve);
}
