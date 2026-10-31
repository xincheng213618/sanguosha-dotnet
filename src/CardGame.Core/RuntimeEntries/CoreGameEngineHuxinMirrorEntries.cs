#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHuxinMirrorRuntime))]
internal sealed partial class CoreGameEngineHuxinMirrorEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHuxinMirrorRuntime> _resolve;
    private CoreGameEngineHuxinMirrorEntries(Func<EngineOwners.CoreGameEngineHuxinMirrorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHuxinMirrorEntries Create(Func<EngineOwners.CoreGameEngineHuxinMirrorRuntime> resolve) => new(resolve);
}
