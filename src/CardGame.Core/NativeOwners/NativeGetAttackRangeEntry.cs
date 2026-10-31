#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeGetAttackRangeEntry
{
private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> resolve;
private NativeGetAttackRangeEntry(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> resolve) { this.resolve = resolve; }
internal static NativeGetAttackRangeEntry Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> resolve) => new(resolve);
internal int Invoke(int sourceSeat) => resolve().GetAttackRange(sourceSeat);
}
