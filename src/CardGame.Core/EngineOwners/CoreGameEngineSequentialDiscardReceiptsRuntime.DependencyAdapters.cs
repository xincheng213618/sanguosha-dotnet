#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreSequentialDiscardReceiptsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardReceiptsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreSequentialDiscardReceiptsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardReceiptsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime))]
internal sealed class CoreSequentialDiscardReceiptsCoreAllHandChallengeReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardReceiptsCoreAllHandChallengeReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime> _targetFactory = targetFactory;
    public bool ValidAllHandChallenge(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidAllHandChallenge(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreSequentialDiscardReceiptsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardReceiptsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime))]
internal sealed class CoreSequentialDiscardReceiptsCoreSequentialDiscardProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardReceiptsCoreSequentialDiscardProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardConversionSource SequentialDiscardSource(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().SequentialDiscardSource(f);
    public global::System.Collections.Generic.IReadOnlyList<int> SequentialDiscardOrder(int owner, int start, bool ring) => this._targetFactory().SequentialDiscardOrder(owner, start, ring);
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation Location)> SequentialDiscardCards(global::CardGame.Core.ProgramSkillFrame frame, int chooser, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().SequentialDiscardCards(frame, chooser, effect);
    public long SequentialDiscardMovementSequence { get => this._targetFactory().SequentialDiscardMovementSequence; }

    public string SequentialDiscardReason(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().SequentialDiscardReason(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed class CoreSequentialDiscardReceiptsCoreEphemeralCardMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardReceiptsCoreEphemeralCardMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _targetFactory = targetFactory;
    public bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().IsEphemeralCardDestroyedMaterial(movement);
}
