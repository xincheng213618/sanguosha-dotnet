#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDamageFramesRuntime))]
internal sealed partial class CoreNativeDamageFramesEntries
{
    private readonly Func<EngineOwners.CoreNativeDamageFramesRuntime> _resolve;
    private CoreNativeDamageFramesEntries(Func<EngineOwners.CoreNativeDamageFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageFramesEntries Create(Func<EngineOwners.CoreNativeDamageFramesRuntime> resolve) => new(resolve);
}
