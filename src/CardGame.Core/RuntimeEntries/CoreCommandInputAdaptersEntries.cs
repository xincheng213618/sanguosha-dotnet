#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCommandInputAdaptersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandInputAdaptersRuntime> _resolve;
    private CoreCommandInputAdaptersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandInputAdaptersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreCommandInputAdaptersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandInputAdaptersRuntime> resolve) => new(resolve);
}
