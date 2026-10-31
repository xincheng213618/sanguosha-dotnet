#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramSkillChangesRuntime))]
internal sealed partial class CoreGameEngineProgramSkillChangesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramSkillChangesRuntime> _resolve;
    private CoreGameEngineProgramSkillChangesEntries(Func<EngineOwners.CoreGameEngineProgramSkillChangesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramSkillChangesEntries Create(Func<EngineOwners.CoreGameEngineProgramSkillChangesRuntime> resolve) => new(resolve);
}
