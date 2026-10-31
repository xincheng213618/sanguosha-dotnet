#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SourceCurseSelectionQueries(Func<RuntimeEntries.CoreGameEngineSourceCursesEntries> getCoreGameEngineSourceCursesRuntime) : ISourceCurseSelectionQueries
{
    public bool HasCurse(int targetSeat) => getCoreGameEngineSourceCursesRuntime().SourceCurseAt(targetSeat)is not null;
}
