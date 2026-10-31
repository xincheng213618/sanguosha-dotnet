#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlayPhaseSkillGrantsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> _resolve;
    private CoreGameEnginePlayPhaseSkillGrantsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlayPhaseSkillGrantsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> resolve) => new(resolve);
}
