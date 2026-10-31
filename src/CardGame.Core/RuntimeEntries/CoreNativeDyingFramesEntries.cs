#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDyingFramesRuntime))]
internal sealed partial class CoreNativeDyingFramesEntries
{
    private readonly Func<EngineOwners.CoreNativeDyingFramesRuntime> _resolve;
    private CoreNativeDyingFramesEntries(Func<EngineOwners.CoreNativeDyingFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDyingFramesEntries Create(Func<EngineOwners.CoreNativeDyingFramesRuntime> resolve) => new(resolve);
}
