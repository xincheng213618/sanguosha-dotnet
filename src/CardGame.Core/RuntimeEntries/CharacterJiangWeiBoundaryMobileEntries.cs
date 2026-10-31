#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterJiangWeiBoundaryMobileRuntime))]
internal sealed partial class CharacterJiangWeiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiBoundaryMobileRuntime> _resolve;
    private CharacterJiangWeiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangWeiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiBoundaryMobileRuntime> resolve) => new(resolve);
}
