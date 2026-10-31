#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed partial class CoreGameEngineWenheCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWenheCardsRuntime> _resolve;
    private CoreGameEngineWenheCardsEntries(Func<EngineOwners.CoreGameEngineWenheCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenheCardsEntries Create(Func<EngineOwners.CoreGameEngineWenheCardsRuntime> resolve) => new(resolve);
}
