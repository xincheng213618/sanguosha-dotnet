#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSchoolLateEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime> _resolve;
    private CoreGameEngineShanheSchoolLateEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolLateEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime> resolve) => new(resolve);
}
