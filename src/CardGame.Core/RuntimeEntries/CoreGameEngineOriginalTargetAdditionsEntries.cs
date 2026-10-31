#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime))]
internal sealed partial class CoreGameEngineOriginalTargetAdditionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> _resolve;
    private CoreGameEngineOriginalTargetAdditionsEntries(Func<EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalTargetAdditionsEntries Create(Func<EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> resolve) => new(resolve);
}
