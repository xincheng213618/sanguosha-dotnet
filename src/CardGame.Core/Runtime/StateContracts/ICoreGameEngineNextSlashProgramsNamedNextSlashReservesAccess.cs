#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineNextSlashProgramsNamedNextSlashReservesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.NamedNextSlashReserve> _namedNextSlashReserves { get; }
}
