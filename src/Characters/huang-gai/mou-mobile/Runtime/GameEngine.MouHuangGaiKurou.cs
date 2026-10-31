using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record MouKurouAiCandidate(int CardId, int RecipientSeat, ProgramAiEstimate Estimate);
}
