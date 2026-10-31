#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceGrainRuntime))]
internal sealed partial class CoreGameEngineShanheSourceGrainEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceGrainRuntime> _resolve;
    private CoreGameEngineShanheSourceGrainEntries(Func<EngineOwners.CoreGameEngineShanheSourceGrainRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceGrainEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceGrainRuntime> resolve) => new(resolve);
}
