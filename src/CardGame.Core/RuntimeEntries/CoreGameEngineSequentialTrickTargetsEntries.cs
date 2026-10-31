#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime))]
internal sealed partial class CoreGameEngineSequentialTrickTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> _resolve;
    private CoreGameEngineSequentialTrickTargetsEntries(Func<EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSequentialTrickTargetsEntries Create(Func<EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> resolve) => new(resolve);
}
