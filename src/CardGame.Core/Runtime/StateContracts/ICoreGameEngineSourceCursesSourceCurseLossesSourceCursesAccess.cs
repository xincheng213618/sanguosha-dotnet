#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess : IProgramRuntimeOperations
{
    ref readonly List<SourceCurseLoss> _sourceCurseLosses { get; }

    ref readonly List<SourceCurseDeposit> _sourceCurses { get; }
}
