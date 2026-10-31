#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed partial class CoreNativeJudgmentEntries
{
    private readonly Func<EngineOwners.CoreNativeJudgmentRuntime> _resolve;
    private CoreNativeJudgmentEntries(Func<EngineOwners.CoreNativeJudgmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeJudgmentEntries Create(Func<EngineOwners.CoreNativeJudgmentRuntime> resolve) => new(resolve);
}
