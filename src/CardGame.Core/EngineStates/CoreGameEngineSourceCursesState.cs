#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineSourceCursesState : Contracts.ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess, Contracts.ICoreGameEngineSourceCursesSourceCursesAccess
{
    private List<SourceCurseLoss> _sourceCurseLosses = [];
    internal ref readonly List<SourceCurseLoss> ReferenceToSourceCurseLosses => ref _sourceCurseLosses;

    private List<SourceCurseDeposit> _sourceCurses = [];
    internal ref readonly List<SourceCurseDeposit> ReferenceToSourceCurses => ref _sourceCurses;

    ref readonly List<SourceCurseLoss> Contracts.ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurseLosses => ref _sourceCurseLosses;

    ref readonly List<SourceCurseDeposit> Contracts.ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses => ref _sourceCurses;

    ref readonly List<SourceCurseDeposit> Contracts.ICoreGameEngineSourceCursesSourceCursesAccess._sourceCurses => ref _sourceCurses;
}
