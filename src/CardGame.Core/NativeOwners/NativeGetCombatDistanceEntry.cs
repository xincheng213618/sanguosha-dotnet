#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeGetCombatDistanceEntry
{
private readonly Func<EngineOwners.CoreNativeRuleQueriesRuntime> resolve;
private NativeGetCombatDistanceEntry(Func<EngineOwners.CoreNativeRuleQueriesRuntime> resolve) { this.resolve = resolve; }
internal static NativeGetCombatDistanceEntry Create(Func<EngineOwners.CoreNativeRuleQueriesRuntime> resolve) => new(resolve);
internal int Invoke(int sourceSeat, int targetSeat) => resolve().GetCombatDistance(sourceSeat, targetSeat);
}
