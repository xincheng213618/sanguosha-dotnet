#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnSkillGrantsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnSkillGrantsRuntime> _resolve;
    private CoreGameEngineActualTurnSkillGrantsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnSkillGrantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnSkillGrantsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnSkillGrantsRuntime> resolve) => new(resolve);
}
