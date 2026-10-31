#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceXiyingEndingRuntime))]
internal sealed partial class CoreGameEngineShanheSourceXiyingEndingEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceXiyingEndingRuntime> _resolve;
    private CoreGameEngineShanheSourceXiyingEndingEntries(Func<EngineOwners.CoreGameEngineShanheSourceXiyingEndingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceXiyingEndingEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceXiyingEndingRuntime> resolve) => new(resolve);
}
