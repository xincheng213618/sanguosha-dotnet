#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeSkillInvocationObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillInvocationObserversRuntime> _resolve;
    private CoreNativeSkillInvocationObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillInvocationObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeSkillInvocationObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillInvocationObserversRuntime> resolve) => new(resolve);
}
