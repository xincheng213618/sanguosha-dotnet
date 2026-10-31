#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSkillInvocationNativeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillInvocationNativeRuntime> _resolve;
    private CoreGameEngineSkillInvocationNativeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillInvocationNativeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSkillInvocationNativeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillInvocationNativeRuntime> resolve) => new(resolve);
}
