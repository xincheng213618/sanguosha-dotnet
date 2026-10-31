#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDeathFramesRuntime))]
internal sealed partial class CoreNativeDeathFramesEntries
{
    private readonly Func<EngineOwners.CoreNativeDeathFramesRuntime> _resolve;
    private CoreNativeDeathFramesEntries(Func<EngineOwners.CoreNativeDeathFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDeathFramesEntries Create(Func<EngineOwners.CoreNativeDeathFramesRuntime> resolve) => new(resolve);
}
