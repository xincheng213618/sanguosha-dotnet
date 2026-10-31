#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeQinglongRuntime))]
internal sealed partial class CoreNativeQinglongEntries
{
    private readonly Func<EngineOwners.CoreNativeQinglongRuntime> _resolve;
    private CoreNativeQinglongEntries(Func<EngineOwners.CoreNativeQinglongRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeQinglongEntries Create(Func<EngineOwners.CoreNativeQinglongRuntime> resolve) => new(resolve);
}
