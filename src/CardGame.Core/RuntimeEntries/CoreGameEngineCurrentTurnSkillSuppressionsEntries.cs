#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime))]
internal sealed partial class CoreGameEngineCurrentTurnSkillSuppressionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> _resolve;
    private CoreGameEngineCurrentTurnSkillSuppressionsEntries(Func<EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentTurnSkillSuppressionsEntries Create(Func<EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> resolve) => new(resolve);
}
