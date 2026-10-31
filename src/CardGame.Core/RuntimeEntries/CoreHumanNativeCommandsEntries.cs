#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHumanNativeCommandsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHumanNativeCommandsRuntime> _resolve;
    private CoreHumanNativeCommandsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreHumanNativeCommandsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHumanNativeCommandsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreHumanNativeCommandsRuntime> resolve) => new(resolve);
}
