#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime))]
internal sealed partial class CoreHappyWeiYanGainDestinationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> _resolve;
    private CoreHappyWeiYanGainDestinationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHappyWeiYanGainDestinationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> resolve) => new(resolve);
}
