#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreEndingHandCountProgramsState : global::CardGame.Core.EngineStates.Contracts.ICoreEndingHandCountProgramsEndingHandCountHistoriesAccess
{
    private global::System.Collections.Generic.Dictionary<(int Owner, string Skill, string Instance, string State), global::CardGame.Core.ProgramEndingHandCountHistory> _endingHandCountHistories = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Owner, string Skill, string Instance, string State), global::CardGame.Core.ProgramEndingHandCountHistory> ReferenceToEndingHandCountHistories => ref _endingHandCountHistories;

    ref readonly global::System.Collections.Generic.Dictionary<(int Owner, string Skill, string Instance, string State), global::CardGame.Core.ProgramEndingHandCountHistory> global::CardGame.Core.EngineStates.Contracts.ICoreEndingHandCountProgramsEndingHandCountHistoriesAccess._endingHandCountHistories => ref _endingHandCountHistories;
}
