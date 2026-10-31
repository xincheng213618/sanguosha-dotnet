#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeNullificationRuntime))]
internal sealed partial class CoreNativeNullificationEntries
{
    private readonly Func<EngineOwners.CoreNativeNullificationRuntime> _resolve;
    private CoreNativeNullificationEntries(Func<EngineOwners.CoreNativeNullificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeNullificationEntries Create(Func<EngineOwners.CoreNativeNullificationRuntime> resolve) => new(resolve);
}
