#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGrantedEntityDistanceInvariantsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityDistanceInvariantsRuntime> _resolve;
    private CoreGameEngineGrantedEntityDistanceInvariantsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityDistanceInvariantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGrantedEntityDistanceInvariantsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityDistanceInvariantsRuntime> resolve) => new(resolve);
}
