#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGroupTargetNullificationRuntime))]
internal sealed partial class CoreGameEngineGroupTargetNullificationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> _resolve;
    private CoreGameEngineGroupTargetNullificationEntries(Func<EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGroupTargetNullificationEntries Create(Func<EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> resolve) => new(resolve);
}
