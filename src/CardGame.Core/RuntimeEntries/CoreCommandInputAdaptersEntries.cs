#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCommandInputAdaptersEntries
{
    private readonly Func<EngineOwners.CoreCommandInputAdaptersRuntime> _resolve;
    private CoreCommandInputAdaptersEntries(Func<EngineOwners.CoreCommandInputAdaptersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreCommandInputAdaptersEntries Create(Func<EngineOwners.CoreCommandInputAdaptersRuntime> resolve) => new(resolve);
}
