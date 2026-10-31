#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime))]
internal sealed partial class CoreGameEngineFinalizedCardTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> _resolve;
    private CoreGameEngineFinalizedCardTargetsEntries(Func<EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalizedCardTargetsEntries Create(Func<EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> resolve) => new(resolve);
}
