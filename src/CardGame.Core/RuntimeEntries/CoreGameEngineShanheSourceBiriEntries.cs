#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceBiriRuntime))]
internal sealed partial class CoreGameEngineShanheSourceBiriEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceBiriRuntime> _resolve;
    private CoreGameEngineShanheSourceBiriEntries(Func<EngineOwners.CoreGameEngineShanheSourceBiriRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceBiriEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceBiriRuntime> resolve) => new(resolve);
}
