#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5ZhulingRuntime))]
internal sealed partial class CoreGameEngineShanheS5ZhulingEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5ZhulingRuntime> _resolve;
    private CoreGameEngineShanheS5ZhulingEntries(Func<EngineOwners.CoreGameEngineShanheS5ZhulingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ZhulingEntries Create(Func<EngineOwners.CoreGameEngineShanheS5ZhulingRuntime> resolve) => new(resolve);
}
