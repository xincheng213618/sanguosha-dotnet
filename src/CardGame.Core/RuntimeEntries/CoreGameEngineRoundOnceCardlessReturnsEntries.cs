#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime))]
internal sealed partial class CoreGameEngineRoundOnceCardlessReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> _resolve;
    private CoreGameEngineRoundOnceCardlessReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundOnceCardlessReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> resolve) => new(resolve);
}
