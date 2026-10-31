#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicDamagePredictionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> _resolve;
    private CoreGameEnginePublicDamagePredictionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicDamagePredictionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> resolve) => new(resolve);
}
