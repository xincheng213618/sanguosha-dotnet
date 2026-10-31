#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime))]
internal sealed partial class CoreGameEngineWushuangPublicPredictionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime> _resolve;
    private CoreGameEngineWushuangPublicPredictionEntries(Func<EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangPublicPredictionEntries Create(Func<EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime> resolve) => new(resolve);
}
