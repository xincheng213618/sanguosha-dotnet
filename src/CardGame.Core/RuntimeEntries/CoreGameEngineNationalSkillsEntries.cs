#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNationalSkillsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _resolve;
    private CoreGameEngineNationalSkillsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNationalSkillsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> resolve) => new(resolve);
}
