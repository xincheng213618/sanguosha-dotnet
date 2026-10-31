#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineZhaoguMirrorRuntime))]
internal sealed partial class CoreGameEngineZhaoguMirrorEntries
{
    private readonly Func<EngineOwners.CoreGameEngineZhaoguMirrorRuntime> _resolve;
    private CoreGameEngineZhaoguMirrorEntries(Func<EngineOwners.CoreGameEngineZhaoguMirrorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhaoguMirrorEntries Create(Func<EngineOwners.CoreGameEngineZhaoguMirrorRuntime> resolve) => new(resolve);
}
