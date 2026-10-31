#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime))]
internal sealed partial class CoreGameEngineOwnTrickDrawTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> _resolve;
    private CoreGameEngineOwnTrickDrawTargetsEntries(Func<EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnTrickDrawTargetsEntries Create(Func<EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> resolve) => new(resolve);
}
