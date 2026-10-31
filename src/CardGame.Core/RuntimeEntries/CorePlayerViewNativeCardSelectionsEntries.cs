#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CorePlayerViewNativeCardSelectionsRuntime))]
internal sealed partial class CorePlayerViewNativeCardSelectionsEntries
{
    private readonly Func<EngineOwners.CorePlayerViewNativeCardSelectionsRuntime> _resolve;
    private CorePlayerViewNativeCardSelectionsEntries(Func<EngineOwners.CorePlayerViewNativeCardSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePlayerViewNativeCardSelectionsEntries Create(Func<EngineOwners.CorePlayerViewNativeCardSelectionsRuntime> resolve) => new(resolve);
}
