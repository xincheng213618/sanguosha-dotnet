#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime))]
internal sealed partial class CoreGameEngineZhaoguMirrorEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime> _resolve;
    private CoreGameEngineZhaoguMirrorEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhaoguMirrorEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime> resolve) => new(resolve);
}
