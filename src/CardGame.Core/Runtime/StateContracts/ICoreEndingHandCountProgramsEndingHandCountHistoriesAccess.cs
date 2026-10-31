#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreEndingHandCountProgramsEndingHandCountHistoriesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int Owner, string Skill, string Instance, string State), global::CardGame.Core.ProgramEndingHandCountHistory> _endingHandCountHistories { get; }
}
