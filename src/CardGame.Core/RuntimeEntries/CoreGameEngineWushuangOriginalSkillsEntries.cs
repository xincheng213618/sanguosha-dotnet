#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime))]
internal sealed partial class CoreGameEngineWushuangOriginalSkillsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime> _resolve;
    private CoreGameEngineWushuangOriginalSkillsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangOriginalSkillsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime> resolve) => new(resolve);
}
