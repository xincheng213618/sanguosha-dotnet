#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed partial class CoreNativeSkillContextQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> _resolve;
    private CoreNativeSkillContextQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeSkillContextQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> resolve) => new(resolve);
}
