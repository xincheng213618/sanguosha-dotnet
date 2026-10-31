#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime))]
internal sealed partial class CoreGameEngineRoundOnceCardlessReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> _resolve;
    private CoreGameEngineRoundOnceCardlessReturnsEntries(Func<EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundOnceCardlessReturnsEntries Create(Func<EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> resolve) => new(resolve);
}
