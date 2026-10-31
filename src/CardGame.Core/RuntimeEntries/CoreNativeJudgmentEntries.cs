#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeJudgmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _resolve;
    private CoreNativeJudgmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeJudgmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> resolve) => new(resolve);
}
