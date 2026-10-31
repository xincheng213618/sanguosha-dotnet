#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime))]
internal sealed partial class CoreNativePlayerDeathEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime> _resolve;
    private CoreNativePlayerDeathEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativePlayerDeathEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime> resolve) => new(resolve);
}
