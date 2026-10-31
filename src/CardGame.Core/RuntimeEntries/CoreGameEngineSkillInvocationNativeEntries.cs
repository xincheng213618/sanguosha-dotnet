#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSkillInvocationNativeRuntime))]
internal sealed partial class CoreGameEngineSkillInvocationNativeEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSkillInvocationNativeRuntime> _resolve;
    private CoreGameEngineSkillInvocationNativeEntries(Func<EngineOwners.CoreGameEngineSkillInvocationNativeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSkillInvocationNativeEntries Create(Func<EngineOwners.CoreGameEngineSkillInvocationNativeRuntime> resolve) => new(resolve);
}
