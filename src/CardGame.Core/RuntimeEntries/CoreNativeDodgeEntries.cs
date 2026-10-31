#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDodgeRuntime))]
internal sealed partial class CoreNativeDodgeEntries
{
    private readonly Func<EngineOwners.CoreNativeDodgeRuntime> _resolve;
    private CoreNativeDodgeEntries(Func<EngineOwners.CoreNativeDodgeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDodgeEntries Create(Func<EngineOwners.CoreNativeDodgeRuntime> resolve) => new(resolve);
}
