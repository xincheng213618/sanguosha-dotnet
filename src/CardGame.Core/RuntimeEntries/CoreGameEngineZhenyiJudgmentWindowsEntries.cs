#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineZhenyiJudgmentWindowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> _resolve;
    private CoreGameEngineZhenyiJudgmentWindowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhenyiJudgmentWindowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> resolve) => new(resolve);
}
