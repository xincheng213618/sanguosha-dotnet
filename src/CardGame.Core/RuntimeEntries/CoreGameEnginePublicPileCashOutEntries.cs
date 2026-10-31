#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileCashOutEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileCashOutRuntime> _resolve;
    private CoreGameEnginePublicPileCashOutEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileCashOutRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPileCashOutEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileCashOutRuntime> resolve) => new(resolve);
}
