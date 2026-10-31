#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed partial class CoreNativeSkillContextQueriesEntries
{
    private readonly Func<EngineOwners.CoreNativeSkillContextQueriesRuntime> _resolve;
    private CoreNativeSkillContextQueriesEntries(Func<EngineOwners.CoreNativeSkillContextQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeSkillContextQueriesEntries Create(Func<EngineOwners.CoreNativeSkillContextQueriesRuntime> resolve) => new(resolve);
}
