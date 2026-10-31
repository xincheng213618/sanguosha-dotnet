#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime))]
internal sealed partial class CoreGameEngineGroupTargetNullificationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> _resolve;
    private CoreGameEngineGroupTargetNullificationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGroupTargetNullificationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> resolve) => new(resolve);
}
