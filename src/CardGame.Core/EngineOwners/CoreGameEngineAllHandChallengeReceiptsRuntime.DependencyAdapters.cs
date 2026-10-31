#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreAllHandChallengeReceiptsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAllHandChallengeReceiptsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime))]
internal sealed class CoreAllHandChallengeReceiptsCoreSequentialDiscardProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAllHandChallengeReceiptsCoreSequentialDiscardProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardConversionSource SequentialDiscardSource(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().SequentialDiscardSource(f);
    public global::System.Collections.Generic.IReadOnlyList<int> SequentialDiscardOrder(int owner, int start, bool ring) => this._targetFactory().SequentialDiscardOrder(owner, start, ring);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime))]
internal sealed class CoreAllHandChallengeReceiptsCoreSequentialDiscardReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAllHandChallengeReceiptsCoreSequentialDiscardReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> _targetFactory = targetFactory;
    public bool ValidSequentialDiscardPayment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramSequentialDiscardDraft d, global::CardGame.Core.ProgramSequentialDiscardPayment p) => this._targetFactory().ValidSequentialDiscardPayment(f, d, p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreAllHandChallengeReceiptsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAllHandChallengeReceiptsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}
