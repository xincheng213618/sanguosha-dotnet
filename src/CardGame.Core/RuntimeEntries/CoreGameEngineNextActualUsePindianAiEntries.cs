#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime))]
internal sealed partial class CoreGameEngineNextActualUsePindianAiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime> _resolve;
    private CoreGameEngineNextActualUsePindianAiEntries(Func<EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNextActualUsePindianAiEntries Create(Func<EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime> resolve) => new(resolve);
}
