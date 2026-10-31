#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeGetAttackRangeEntry
{
private readonly Func<EngineOwners.CoreNativeRuleQueriesRuntime> resolve;
private NativeGetAttackRangeEntry(Func<EngineOwners.CoreNativeRuleQueriesRuntime> resolve) { this.resolve = resolve; }
internal static NativeGetAttackRangeEntry Create(Func<EngineOwners.CoreNativeRuleQueriesRuntime> resolve) => new(resolve);
internal int Invoke(int sourceSeat) => resolve().GetAttackRange(sourceSeat);
}
