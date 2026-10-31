#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDyingRuntime))]
internal sealed partial class CoreNativeDyingEntries
{
    private readonly Func<EngineOwners.CoreNativeDyingRuntime> _resolve;
    private CoreNativeDyingEntries(Func<EngineOwners.CoreNativeDyingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDyingEntries Create(Func<EngineOwners.CoreNativeDyingRuntime> resolve) => new(resolve);
}
