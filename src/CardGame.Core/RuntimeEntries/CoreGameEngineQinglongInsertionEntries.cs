#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineQinglongInsertionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime> _resolve;
    private CoreGameEngineQinglongInsertionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineQinglongInsertionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime> resolve) => new(resolve);
}
