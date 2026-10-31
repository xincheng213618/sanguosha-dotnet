#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreHumanNativeCommandsRuntime))]
internal sealed partial class CoreHumanNativeCommandsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHumanNativeCommandsRuntime> _resolve;
    private CoreHumanNativeCommandsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreHumanNativeCommandsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHumanNativeCommandsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreHumanNativeCommandsRuntime> resolve) => new(resolve);
}
