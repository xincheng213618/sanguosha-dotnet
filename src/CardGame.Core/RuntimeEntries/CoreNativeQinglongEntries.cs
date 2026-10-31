#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime))]
internal sealed partial class CoreNativeQinglongEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> _resolve;
    private CoreNativeQinglongEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeQinglongEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> resolve) => new(resolve);
}
