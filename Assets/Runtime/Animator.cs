// 日本語対応
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace NovelDataEditor
{
    public abstract class Animator : MonoBehaviour
    {
        public abstract UniTask PlayAnimationAsync(AnimationControlData animationData, CancellationToken token);
    }

    public class AnimationControlData
    {
        // TODO: スピードや、入力などの要素を含める。
    }
}