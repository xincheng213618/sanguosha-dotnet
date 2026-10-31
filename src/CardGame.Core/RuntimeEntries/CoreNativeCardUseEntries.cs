#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeCardUseRuntime))]
internal sealed partial class CoreNativeCardUseEntries
{
    private readonly Func<EngineOwners.CoreNativeCardUseRuntime> _resolve;
    private CoreNativeCardUseEntries(Func<EngineOwners.CoreNativeCardUseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardUseEntries Create(Func<EngineOwners.CoreNativeCardUseRuntime> resolve) => new(resolve);
}
