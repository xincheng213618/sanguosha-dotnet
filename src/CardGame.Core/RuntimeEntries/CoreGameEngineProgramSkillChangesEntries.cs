#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime))]
internal sealed partial class CoreGameEngineProgramSkillChangesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> _resolve;
    private CoreGameEngineProgramSkillChangesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramSkillChangesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> resolve) => new(resolve);
}
