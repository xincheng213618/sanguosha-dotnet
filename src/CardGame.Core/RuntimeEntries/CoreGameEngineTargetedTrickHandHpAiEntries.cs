#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime))]
internal sealed partial class CoreGameEngineTargetedTrickHandHpAiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime> _resolve;
    private CoreGameEngineTargetedTrickHandHpAiEntries(Func<EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTargetedTrickHandHpAiEntries Create(Func<EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime> resolve) => new(resolve);
}
