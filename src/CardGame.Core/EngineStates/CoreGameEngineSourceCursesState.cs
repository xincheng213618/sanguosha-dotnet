#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineSourceCursesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCursesAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.SourceCurseLoss> _sourceCurseLosses = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.SourceCurseLoss> ReferenceToSourceCurseLosses => ref _sourceCurseLosses;

    private global::System.Collections.Generic.List<global::CardGame.Core.SourceCurseDeposit> _sourceCurses = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.SourceCurseDeposit> ReferenceToSourceCurses => ref _sourceCurses;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.SourceCurseLoss> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurseLosses => ref _sourceCurseLosses;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.SourceCurseDeposit> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses => ref _sourceCurses;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.SourceCurseDeposit> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCursesAccess._sourceCurses => ref _sourceCurses;
}
