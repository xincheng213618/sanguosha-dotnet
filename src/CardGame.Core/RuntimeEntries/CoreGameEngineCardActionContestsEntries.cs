#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardActionContestsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionContestsRuntime> _resolve;
    private CoreGameEngineCardActionContestsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionContestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardActionContestsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionContestsRuntime> resolve) => new(resolve);
}
