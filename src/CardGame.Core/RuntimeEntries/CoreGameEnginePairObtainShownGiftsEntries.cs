#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairObtainShownGiftsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> _resolve;
    private CoreGameEnginePairObtainShownGiftsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairObtainShownGiftsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> resolve) => new(resolve);
}
