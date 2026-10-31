#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime))]
internal sealed partial class CoreNativeIceSwordEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> _resolve;
    private CoreNativeIceSwordEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeIceSwordEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> resolve) => new(resolve);
}
