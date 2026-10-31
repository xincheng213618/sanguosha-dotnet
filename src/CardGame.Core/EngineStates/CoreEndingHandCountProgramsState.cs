#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreEndingHandCountProgramsState : Contracts.ICoreEndingHandCountProgramsEndingHandCountHistoriesAccess
{
    private Dictionary<(int Owner, string Skill, string Instance, string State), ProgramEndingHandCountHistory> _endingHandCountHistories = [];
    internal ref readonly Dictionary<(int Owner, string Skill, string Instance, string State), ProgramEndingHandCountHistory> ReferenceToEndingHandCountHistories => ref _endingHandCountHistories;

    ref readonly Dictionary<(int Owner, string Skill, string Instance, string State), ProgramEndingHandCountHistory> Contracts.ICoreEndingHandCountProgramsEndingHandCountHistoriesAccess._endingHandCountHistories => ref _endingHandCountHistories;
}
