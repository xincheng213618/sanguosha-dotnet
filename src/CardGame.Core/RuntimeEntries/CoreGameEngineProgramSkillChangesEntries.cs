#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramSkillChangesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> _resolve;
    private CoreGameEngineProgramSkillChangesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramSkillChangesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> resolve) => new(resolve);
}
