#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhangbaSelectionsRuntime))]
internal sealed partial class CoreGameEngineZhangbaSelectionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhangbaSelectionsRuntime> _resolve;
    private CoreGameEngineZhangbaSelectionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhangbaSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhangbaSelectionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhangbaSelectionsRuntime> resolve) => new(resolve);
}
