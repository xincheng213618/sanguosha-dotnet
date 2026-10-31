#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMouLuMengEntries
{
    internal void BeginRoundForTurn(CharacterState current) => _resolve().BeginRoundForTurn(current);
    internal bool UsesFormalMouLuMeng { get => _resolve().UsesFormalMouLuMeng; }
}
