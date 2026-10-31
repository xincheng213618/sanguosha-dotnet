#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLianZhuCountsRuntime))]
internal sealed partial class CoreGameEngineLianZhuCountsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLianZhuCountsRuntime> _resolve;
    private CoreGameEngineLianZhuCountsEntries(Func<EngineOwners.CoreGameEngineLianZhuCountsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLianZhuCountsEntries Create(Func<EngineOwners.CoreGameEngineLianZhuCountsRuntime> resolve) => new(resolve);
}
