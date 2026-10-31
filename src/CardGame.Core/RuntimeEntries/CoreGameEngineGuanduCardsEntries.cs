#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed partial class CoreGameEngineGuanduCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduCardsRuntime> _resolve;
    private CoreGameEngineGuanduCardsEntries(Func<EngineOwners.CoreGameEngineGuanduCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduCardsEntries Create(Func<EngineOwners.CoreGameEngineGuanduCardsRuntime> resolve) => new(resolve);
}
