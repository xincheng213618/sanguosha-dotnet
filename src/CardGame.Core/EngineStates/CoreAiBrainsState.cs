#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreAiBrainsState : Contracts.ICoreAiBrainsAiBrainsAccess
{
    private Dictionary<int, SimpleAiBrain> _aiBrains = [];
    internal ref readonly Dictionary<int, SimpleAiBrain> ReferenceToAiBrains => ref _aiBrains;

    ref readonly Dictionary<int, SimpleAiBrain> Contracts.ICoreAiBrainsAiBrainsAccess._aiBrains => ref _aiBrains;
}
