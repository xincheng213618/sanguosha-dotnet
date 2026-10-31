#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWangFuZhaoLeiMobileRuntime))]
internal sealed partial class CharacterWangFuZhaoLeiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangFuZhaoLeiMobileRuntime> _resolve;
    private CharacterWangFuZhaoLeiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangFuZhaoLeiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangFuZhaoLeiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangFuZhaoLeiMobileRuntime> resolve) => new(resolve);
}
