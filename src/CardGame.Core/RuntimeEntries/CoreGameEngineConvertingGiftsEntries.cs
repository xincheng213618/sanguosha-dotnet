#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConvertingGiftsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> _resolve;
    private CoreGameEngineConvertingGiftsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConvertingGiftsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> resolve) => new(resolve);
}
