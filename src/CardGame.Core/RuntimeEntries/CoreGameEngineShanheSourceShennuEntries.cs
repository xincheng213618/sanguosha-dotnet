#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceShennuRuntime))]
internal sealed partial class CoreGameEngineShanheSourceShennuEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceShennuRuntime> _resolve;
    private CoreGameEngineShanheSourceShennuEntries(Func<EngineOwners.CoreGameEngineShanheSourceShennuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceShennuEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceShennuRuntime> resolve) => new(resolve);
}
