#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuFengBoundaryRuntime))]
internal sealed partial class CharacterLiuFengBoundaryEntries
{
    private readonly Func<EngineOwners.CharacterLiuFengBoundaryRuntime> _resolve;
    private CharacterLiuFengBoundaryEntries(Func<EngineOwners.CharacterLiuFengBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuFengBoundaryEntries Create(Func<EngineOwners.CharacterLiuFengBoundaryRuntime> resolve) => new(resolve);
}
