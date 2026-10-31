#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeIceSwordRuntime))]
internal sealed partial class CoreNativeIceSwordEntries
{
    private readonly Func<EngineOwners.CoreNativeIceSwordRuntime> _resolve;
    private CoreNativeIceSwordEntries(Func<EngineOwners.CoreNativeIceSwordRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeIceSwordEntries Create(Func<EngineOwners.CoreNativeIceSwordRuntime> resolve) => new(resolve);
}
