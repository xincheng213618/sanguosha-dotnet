namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record NativeFactionSkillSuspensionProjection(ResolutionFrame Parent,
        SkillInvocationFrame Invocation, int ParentIndex);
}
