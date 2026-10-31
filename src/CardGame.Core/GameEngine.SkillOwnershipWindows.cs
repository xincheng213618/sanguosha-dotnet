namespace CardGame.Core;

public sealed partial class GameEngine
{
    private readonly SkillGrantRevisionStore _skillGrantRevisions = new();
    private readonly Dictionary<int, long> _observedSkillGrantRevisions;
}
