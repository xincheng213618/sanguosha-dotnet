#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime))]
internal sealed partial class CoreGameEngineCurrentTurnSkillSuppressionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> _resolve;
    private CoreGameEngineCurrentTurnSkillSuppressionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentTurnSkillSuppressionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> resolve) => new(resolve);
}
