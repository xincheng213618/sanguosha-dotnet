#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativePlayerDeathRuntime))]
internal sealed partial class CoreNativePlayerDeathEntries
{
    private readonly Func<EngineOwners.CoreNativePlayerDeathRuntime> _resolve;
    private CoreNativePlayerDeathEntries(Func<EngineOwners.CoreNativePlayerDeathRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativePlayerDeathEntries Create(Func<EngineOwners.CoreNativePlayerDeathRuntime> resolve) => new(resolve);
}
