#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineNextSlashProgramsNamedNextSlashReservesAccess : IProgramRuntimeOperations
{
    ref readonly List<GameEngine.NamedNextSlashReserve> _namedNextSlashReserves { get; }
}
