#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime))]
internal sealed partial class CoreGameEngineHuxinMirrorEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> _resolve;
    private CoreGameEngineHuxinMirrorEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHuxinMirrorEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> resolve) => new(resolve);
}
