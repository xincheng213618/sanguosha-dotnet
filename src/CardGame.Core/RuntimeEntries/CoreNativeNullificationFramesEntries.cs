#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeNullificationFramesRuntime))]
internal sealed partial class CoreNativeNullificationFramesEntries
{
    private readonly Func<EngineOwners.CoreNativeNullificationFramesRuntime> _resolve;
    private CoreNativeNullificationFramesEntries(Func<EngineOwners.CoreNativeNullificationFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeNullificationFramesEntries Create(Func<EngineOwners.CoreNativeNullificationFramesRuntime> resolve) => new(resolve);
}
