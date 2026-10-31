#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed partial class CoreNativeCardUseFramesEntries
{
    private readonly Func<EngineOwners.CoreNativeCardUseFramesRuntime> _resolve;
    private CoreNativeCardUseFramesEntries(Func<EngineOwners.CoreNativeCardUseFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardUseFramesEntries Create(Func<EngineOwners.CoreNativeCardUseFramesRuntime> resolve) => new(resolve);
}
