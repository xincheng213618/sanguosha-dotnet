#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPairedColorResponseAiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPairedColorResponseAiRuntime> _resolve;
    private CoreProgramPairedColorResponseAiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPairedColorResponseAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPairedColorResponseAiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPairedColorResponseAiRuntime> resolve) => new(resolve);
}
