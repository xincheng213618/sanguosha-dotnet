#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime))]
internal sealed partial class CoreGameEngineZhenyiJudgmentWindowsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> _resolve;
    private CoreGameEngineZhenyiJudgmentWindowsEntries(Func<EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhenyiJudgmentWindowsEntries Create(Func<EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> resolve) => new(resolve);
}
