#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangSkillsBEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime> _resolve;
    private CoreGameEngineWushuangSkillsBEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangSkillsBEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime> resolve) => new(resolve);
}
