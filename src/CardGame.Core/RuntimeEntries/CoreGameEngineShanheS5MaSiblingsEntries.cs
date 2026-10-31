#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5MaSiblingsRuntime))]
internal sealed partial class CoreGameEngineShanheS5MaSiblingsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5MaSiblingsRuntime> _resolve;
    private CoreGameEngineShanheS5MaSiblingsEntries(Func<EngineOwners.CoreGameEngineShanheS5MaSiblingsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5MaSiblingsEntries Create(Func<EngineOwners.CoreGameEngineShanheS5MaSiblingsRuntime> resolve) => new(resolve);
}
