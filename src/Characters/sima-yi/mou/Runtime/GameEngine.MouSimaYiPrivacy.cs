using System.Collections.Frozen;
namespace CardGame.Core;
public sealed partial class GameEngine
{
internal sealed record MouSmPublicProjection(IReadOnlySet<long> Events,IReadOnlySet<long> Logs,IReadOnlySet<long> Declarations);
}
