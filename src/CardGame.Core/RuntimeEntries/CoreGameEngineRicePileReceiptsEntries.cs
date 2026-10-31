#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRicePileReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileReceiptsRuntime> _resolve;
    private CoreGameEngineRicePileReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRicePileReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileReceiptsRuntime> resolve) => new(resolve);
}
