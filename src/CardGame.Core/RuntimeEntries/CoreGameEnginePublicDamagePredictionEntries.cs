#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePublicDamagePredictionRuntime))]
internal sealed partial class CoreGameEnginePublicDamagePredictionEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> _resolve;
    private CoreGameEnginePublicDamagePredictionEntries(Func<EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicDamagePredictionEntries Create(Func<EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> resolve) => new(resolve);
}
