// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NovelDataEditor
{
    public class AnimationController : MonoBehaviour
    {
        public async UniTask<bool> PlayAnimation(IEnumerable<Animator> animations, AnimationControlData controlData, CancellationToken token)
        {
            List<UniTask> animationTasks = new List<UniTask>();

            foreach (var anim in animations)
            {
                animationTasks.Add(anim.PlayAnimationAsync(controlData, token));
            }

            // 全てのアニメーションが正常に終了するか、キャンセルされるまで待機
            try
            {
                await UniTask.WhenAll(animationTasks);
                return true; // すべてのアニメーションが正常に終了した場合
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Canceled");
                return false; // キャンセルされた場合
            }
        }
    }
}