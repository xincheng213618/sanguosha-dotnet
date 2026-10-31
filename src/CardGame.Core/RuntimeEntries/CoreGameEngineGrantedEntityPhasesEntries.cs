#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime))]
internal sealed partial class CoreGameEngineGrantedEntityPhasesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> _resolve;
    private CoreGameEngineGrantedEntityPhasesEntries(Func<EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGrantedEntityPhasesEntries Create(Func<EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> resolve) => new(resolve);
}
