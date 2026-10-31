#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineZhaoguMirrorEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime> _resolve;
    private CoreGameEngineZhaoguMirrorEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhaoguMirrorEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime> resolve) => new(resolve);
}
