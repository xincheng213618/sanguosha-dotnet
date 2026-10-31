#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGrantedEntityDistanceInvariantsEntries
{
    internal void AssertGrantedEntityDistancePolicies() => _resolve().AssertGrantedEntityDistancePolicies();
    internal bool IsExactGrantedEntityDistancePolicy(global::CardGame.Core.CardUseFrame use) => _resolve().IsExactGrantedEntityDistancePolicy(use);
}
