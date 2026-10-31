#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime))]
internal sealed partial class CoreGameEngineTargetedTrickHandHpAiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime> _resolve;
    private CoreGameEngineTargetedTrickHandHpAiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTargetedTrickHandHpAiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime> resolve) => new(resolve);
}
