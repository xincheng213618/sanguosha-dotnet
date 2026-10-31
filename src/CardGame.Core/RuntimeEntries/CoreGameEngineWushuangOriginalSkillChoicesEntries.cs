#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangOriginalSkillChoicesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangOriginalSkillChoicesRuntime> _resolve;
    private CoreGameEngineWushuangOriginalSkillChoicesEntries(Func<EngineOwners.CoreGameEngineWushuangOriginalSkillChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangOriginalSkillChoicesEntries Create(Func<EngineOwners.CoreGameEngineWushuangOriginalSkillChoicesRuntime> resolve) => new(resolve);
}
