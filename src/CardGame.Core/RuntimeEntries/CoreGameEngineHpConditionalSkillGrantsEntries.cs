#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpConditionalSkillGrantsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> _resolve;
    private CoreGameEngineHpConditionalSkillGrantsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHpConditionalSkillGrantsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> resolve) => new(resolve);
}
