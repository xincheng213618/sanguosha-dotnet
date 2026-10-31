#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramPipelineHooksRuntime))]
internal sealed partial class CoreProgramPipelineHooksEntries
{
    private readonly Func<EngineOwners.CoreProgramPipelineHooksRuntime> _resolve;
    private CoreProgramPipelineHooksEntries(Func<EngineOwners.CoreProgramPipelineHooksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPipelineHooksEntries Create(Func<EngineOwners.CoreProgramPipelineHooksRuntime> resolve) => new(resolve);
}
