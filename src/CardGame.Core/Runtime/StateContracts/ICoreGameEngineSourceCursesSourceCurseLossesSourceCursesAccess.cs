#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.SourceCurseLoss> _sourceCurseLosses { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.SourceCurseDeposit> _sourceCurses { get; }
}
