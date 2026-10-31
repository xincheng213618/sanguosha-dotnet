#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheGuideSkillsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuideSkillsRuntime> _resolve;
    private CoreGameEngineShanheGuideSkillsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuideSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheGuideSkillsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuideSkillsRuntime> resolve) => new(resolve);
}
