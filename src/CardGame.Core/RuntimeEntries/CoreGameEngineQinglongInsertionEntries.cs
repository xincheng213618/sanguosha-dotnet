#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineQinglongInsertionRuntime))]
internal sealed partial class CoreGameEngineQinglongInsertionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineQinglongInsertionRuntime> _resolve;
    private CoreGameEngineQinglongInsertionEntries(Func<EngineOwners.CoreGameEngineQinglongInsertionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineQinglongInsertionEntries Create(Func<EngineOwners.CoreGameEngineQinglongInsertionRuntime> resolve) => new(resolve);
}
