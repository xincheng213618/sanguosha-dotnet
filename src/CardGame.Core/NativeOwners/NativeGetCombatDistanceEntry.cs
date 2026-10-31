#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeGetCombatDistanceEntry
{
private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> resolve;
private NativeGetCombatDistanceEntry(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> resolve) { this.resolve = resolve; }
internal static NativeGetCombatDistanceEntry Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> resolve) => new(resolve);
internal int Invoke(int sourceSeat, int targetSeat) => resolve().GetCombatDistance(sourceSeat, targetSeat);
}
