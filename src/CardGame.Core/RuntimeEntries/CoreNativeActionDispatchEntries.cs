#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeActionDispatchRuntime))]
internal sealed partial class CoreNativeActionDispatchEntries
{
    private readonly Func<EngineOwners.CoreNativeActionDispatchRuntime> _resolve;
    private CoreNativeActionDispatchEntries(Func<EngineOwners.CoreNativeActionDispatchRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeActionDispatchEntries Create(Func<EngineOwners.CoreNativeActionDispatchRuntime> resolve) => new(resolve);
}
