#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceXiyingRuntime))]
internal sealed partial class CoreGameEngineShanheSourceXiyingEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceXiyingRuntime> _resolve;
    private CoreGameEngineShanheSourceXiyingEntries(Func<EngineOwners.CoreGameEngineShanheSourceXiyingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceXiyingEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceXiyingRuntime> resolve) => new(resolve);
}
