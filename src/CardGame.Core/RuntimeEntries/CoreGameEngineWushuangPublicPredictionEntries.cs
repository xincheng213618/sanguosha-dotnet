#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime))]
internal sealed partial class CoreGameEngineWushuangPublicPredictionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime> _resolve;
    private CoreGameEngineWushuangPublicPredictionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangPublicPredictionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime> resolve) => new(resolve);
}
