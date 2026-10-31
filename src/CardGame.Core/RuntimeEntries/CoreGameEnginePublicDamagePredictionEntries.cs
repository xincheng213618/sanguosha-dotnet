#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime))]
internal sealed partial class CoreGameEnginePublicDamagePredictionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> _resolve;
    private CoreGameEnginePublicDamagePredictionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicDamagePredictionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> resolve) => new(resolve);
}
