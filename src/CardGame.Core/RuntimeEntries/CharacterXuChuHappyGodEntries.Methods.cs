#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuChuHappyGodEntries
{
    internal int HappyXuChuPreviousAward(ProgramSkillFrame frame) => _resolve().HappyXuChuPreviousAward(frame);
    internal IReadOnlyList<ShenXuChuDraw> ShenXuChuRoundDraws(ProgramSkillFrame frame, int winner, int maximum) => _resolve().ShenXuChuRoundDraws(frame, winner, maximum);
}
