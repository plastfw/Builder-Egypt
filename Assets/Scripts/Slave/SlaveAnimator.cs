using UnityEngine;

public class SlaveAnimator : MonoBehaviour
{
    public static class States
    {
        public const string Run = nameof(Run);
        public const string TakeStone = nameof(TakeStone);
        public const string Throw = nameof(Throw);
        public const string Dance = nameof(Dance);
        public const string Hitted = nameof(Hitted);
        public const string Tired = nameof(Tired);
    }
}
