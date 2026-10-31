#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime))]
internal sealed partial class CoreGameEngineRoundOnceCardlessBasicEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime> _resolve;
    private CoreGameEngineRoundOnceCardlessBasicEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundOnceCardlessBasicEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime> resolve) => new(resolve);
}
