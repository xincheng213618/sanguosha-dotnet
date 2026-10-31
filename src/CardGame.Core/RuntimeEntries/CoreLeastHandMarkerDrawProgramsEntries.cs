#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime))]
internal sealed partial class CoreLeastHandMarkerDrawProgramsEntries
{
    private readonly Func<EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> _resolve;
    private CoreLeastHandMarkerDrawProgramsEntries(Func<EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreLeastHandMarkerDrawProgramsEntries Create(Func<EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> resolve) => new(resolve);
}
