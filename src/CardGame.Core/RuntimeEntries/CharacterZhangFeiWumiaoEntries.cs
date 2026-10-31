#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime))]
internal sealed partial class CharacterZhangFeiWumiaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> _resolve;
    private CharacterZhangFeiWumiaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangFeiWumiaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> resolve) => new(resolve);
}
