#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime))]
internal sealed partial class CoreGameEngineShanheSchoolLateEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime> _resolve;
    private CoreGameEngineShanheSchoolLateEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolLateEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime> resolve) => new(resolve);
}
