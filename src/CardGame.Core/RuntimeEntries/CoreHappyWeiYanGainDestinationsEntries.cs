#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreHappyWeiYanGainDestinationsRuntime))]
internal sealed partial class CoreHappyWeiYanGainDestinationsEntries
{
    private readonly Func<EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> _resolve;
    private CoreHappyWeiYanGainDestinationsEntries(Func<EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHappyWeiYanGainDestinationsEntries Create(Func<EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> resolve) => new(resolve);
}
