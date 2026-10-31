#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime))]
internal sealed partial class CharacterLiuFengBoundaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> _resolve;
    private CharacterLiuFengBoundaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuFengBoundaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> resolve) => new(resolve);
}
