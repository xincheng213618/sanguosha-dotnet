#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPipelineHooksEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPipelineHooksRuntime> _resolve;
    private CoreProgramPipelineHooksEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPipelineHooksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPipelineHooksEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPipelineHooksRuntime> resolve) => new(resolve);
}
