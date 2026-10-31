#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNextActualTurnStartSkillGrantsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualTurnStartSkillGrantsRuntime> _resolve;
    private CoreGameEngineNextActualTurnStartSkillGrantsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualTurnStartSkillGrantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNextActualTurnStartSkillGrantsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualTurnStartSkillGrantsRuntime> resolve) => new(resolve);
}
