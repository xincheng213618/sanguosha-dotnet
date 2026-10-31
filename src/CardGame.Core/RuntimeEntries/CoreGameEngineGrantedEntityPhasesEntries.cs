#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime))]
internal sealed partial class CoreGameEngineGrantedEntityPhasesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> _resolve;
    private CoreGameEngineGrantedEntityPhasesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGrantedEntityPhasesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> resolve) => new(resolve);
}
