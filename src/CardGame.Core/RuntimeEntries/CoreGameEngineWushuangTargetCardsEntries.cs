#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangTargetCardsRuntime))]
internal sealed partial class CoreGameEngineWushuangTargetCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> _resolve;
    private CoreGameEngineWushuangTargetCardsEntries(Func<EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangTargetCardsEntries Create(Func<EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> resolve) => new(resolve);
}
