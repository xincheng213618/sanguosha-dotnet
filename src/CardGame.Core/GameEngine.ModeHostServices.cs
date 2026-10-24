namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ModeHostRegistry CreateModeHostServices() => new ModeHostRegistry()
        .Register<IWushuangModeHost>(() => new WushuangModeHost(this))
        .Register<IModeSetupRandom>(() => new ModeSetupRandom(this));

    private sealed class ModeSetupRandom(GameEngine engine) : IModeSetupRandom
    {
        public int Next(int exclusiveMax) => engine._random.Next(exclusiveMax);
        public void Shuffle<T>(IList<T> values) => engine._random.Shuffle(values);
    }
}
