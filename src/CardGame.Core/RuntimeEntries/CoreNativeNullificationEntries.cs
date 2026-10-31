#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeNullificationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _resolve;
    private CoreNativeNullificationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeNullificationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> resolve) => new(resolve);
}
