#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed partial class CoreGameEngineNationalSkillsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _resolve;
    private CoreGameEngineNationalSkillsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNationalSkillsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> resolve) => new(resolve);
}
