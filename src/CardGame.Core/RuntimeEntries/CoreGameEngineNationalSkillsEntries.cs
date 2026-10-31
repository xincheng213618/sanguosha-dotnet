#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed partial class CoreGameEngineNationalSkillsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNationalSkillsRuntime> _resolve;
    private CoreGameEngineNationalSkillsEntries(Func<EngineOwners.CoreGameEngineNationalSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNationalSkillsEntries Create(Func<EngineOwners.CoreGameEngineNationalSkillsRuntime> resolve) => new(resolve);
}
