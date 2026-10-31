#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime))]
internal sealed partial class CoreGameEnginePairObtainShownGiftsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> _resolve;
    private CoreGameEnginePairObtainShownGiftsEntries(Func<EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairObtainShownGiftsEntries Create(Func<EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> resolve) => new(resolve);
}
