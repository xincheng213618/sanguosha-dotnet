#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreAiBrainsState : global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess
{
    private global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.SimpleAiBrain> _aiBrains = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.SimpleAiBrain> ReferenceToAiBrains => ref _aiBrains;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.SimpleAiBrain> global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess._aiBrains => ref _aiBrains;
}
