#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramPipelineHooksRuntime))]
internal sealed partial class CoreProgramPipelineHooksEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPipelineHooksRuntime> _resolve;
    private CoreProgramPipelineHooksEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPipelineHooksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPipelineHooksEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPipelineHooksRuntime> resolve) => new(resolve);
}
